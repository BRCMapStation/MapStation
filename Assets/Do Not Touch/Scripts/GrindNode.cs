using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Reptile
{
	[ExecuteInEditMode]
	[SelectionBase]
	public class GrindNode : MonoBehaviour
	{
		public const string GizmoIcon = Constants.GizmoIconBaseDir + "/Grind.png";
		public const string GizmoSelectedIcon = Constants.GizmoIconBaseDir + "/GrindSelected.png";

		public GrindPath PathReference;

		public List<GrindLine> grindLines = new();

		public bool retour;

		public Vector3 position
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

        public Vector3 normal => base.transform.up;

		public bool IsEndpoint => grindLines.Count <= 1;

		public GrindPath Path
		{
			get
			{
				GrindPath grindPath = PathReference;
				if (grindPath == null)
				{
					grindPath = (PathReference = GetComponentInParent<GrindPath>());
					if (grindPath == null && grindLines.Count > 0)
					{
						grindPath = (PathReference = grindLines[0].Path);
					}
				}
				return grindPath;
			}
		}

        public static Vector3 operator +(GrindNode n0, GrindNode n1)
		{
			return n0.transform.position + n1.transform.position;
		}

		public static Vector3 operator +(GrindNode n, Vector3 p)
		{
			return n.transform.position + p;
		}

		public static Vector3 operator +(Vector3 p, GrindNode n)
		{
			return p + n.transform.position;
		}

		public static Vector3 operator -(GrindNode n0, GrindNode n1)
		{
			return n0.transform.position - n1.transform.position;
		}

		public static Vector3 operator -(GrindNode n, Vector3 p)
		{
			return n.transform.position - p;
		}

		public static Vector3 operator -(Vector3 p, GrindNode n)
		{
			return p - n.transform.position;
		}

		public bool IsValid()
		{
			bool result = true;
			foreach (GrindLine grindLine in grindLines)
			{
				if (grindLine == null)
				{
					result = false;
				}
			}
			return result;
		}

		public bool IsConnectedTo(GrindNode grindNode)
		{
			foreach (GrindLine grindLine in grindLines)
			{
				if (grindLine.ContainsNode(grindNode))
				{
					return true;
				}
			}
			return false;
		}

		private void Update() {
			#if UNITY_EDITOR
			if(Application.isEditor) {
				EditorUpdate();
				return;
			}
			#endif
		}

		#if UNITY_EDITOR

		private Grind grind_;
        public Grind Grind => grind_ = grind_ != null ? grind_ : GetComponentInParent<Grind>();

		private bool isControlledBySpline_ = false;
		/// <summary>
		/// When this node is puppeteered by a spline, you cannot directly edit its position.
		/// TODO this is a lie; you should be able to edit rotation.
		/// </summary>
        public bool IsControlledBySpline {
			get => isControlledBySpline_;
			set {
				isControlledBySpline_ = value;
				if(isControlledBySpline_) transform.hideFlags |= HideFlags.NotEditable;
				else transform.hideFlags &= ~HideFlags.NotEditable;
			}
		}

		private void EditorUpdate() {
			// Make out OnValidate also trigger for transform changes
			if(transform.hasChanged) {
				OnValidate();
				transform.hasChanged = false;
			}
		}

		private void OnValidate() {
			if(EditorUtility.IsPersistent(this)) return;
			// Auto-set PathReference
			if (PathReference == null) {
				PathReference = gameObject.GetComponentInParent<GrindPath>();
			}

			// Keep GrindLines synced to GrindNodes
            foreach(GrindLine line in new List<GrindLine>(grindLines))
            {
				if (line == null)
					grindLines.Remove(line);
                else
                {
					line.RebuildWithRedDebugShape();
				}
            }
		}

		private void OnDrawGizmos() {
			if (IsControlledBySpline) return;

			var prefs = Preferences.instance.grinds;
			// Show grind icon, different color when selected so you can easily ctrl-select two nodes at once for actions like linking nodes
			Gizmos.DrawIcon(transform.position, GizmoIcon, true, Selection.Contains(gameObject) ? Color.white : Color.black);
			Gizmos.DrawLine(transform.position, transform.position + transform.up * prefs.nodePostureDirectionGizmoLength);
		}

		public void Button_OrientUp() {
			transform.localRotation = Quaternion.identity;
		}
		public void Button_OrientDown() {
			transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.back);
		}

		private void OnDestroy() {
			// Auto-destroy attached GrindLines when node is deleted
			// This is meant for user interaction, to execute when the user deletes the object
			// But it's flaky when triggered in other scenarios.

			// TODO find a better way to respond when user presses "delete"

			foreach(var grindLine in new List<GrindLine>(grindLines)) {
				if(grindLine != null)
					DestroyImmediate(grindLine.gameObject);
			}
		}

		public void AddLine(GrindLine line) {
			if(!grindLines.Contains(line)) grindLines.Add(line);
		}
		public void RemoveLine(GrindLine line) {
			grindLines.Remove(line);
		}
		public void ClearLines() {
			grindLines.Clear();
		}
		#endif
	}
}
