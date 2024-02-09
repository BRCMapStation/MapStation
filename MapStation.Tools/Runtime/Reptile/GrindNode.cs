using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapStation.Components;
using MapStation.Common.Doctor;

namespace Reptile
{
	[ExecuteInEditMode]
	[SelectionBase]
	public class GrindNode : MonoBehaviour
	{
		public const string GizmoIcon = UIConstants.GizmoIconBaseDir + "/Grind.png";
		public const string GizmoSelectedIcon = UIConstants.GizmoIconBaseDir + "/GrindSelected.png";

		public GrindPath PathReference;

		public List<GrindLine> grindLines = new();

        [Tooltip("If enabled, the player will automatically turn around and grind in the opposite direction when they hit this node. Use for nodes that are against a wall.")]
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
				if (grindLine != null && grindLine.ContainsNode(grindNode))
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
			if(EditorUtility.IsPersistent(this) || MapBuilderStatus.IsBuilding) return;
			// Auto-set PathReference
			if (PathReference == null) {
				PathReference = gameObject.GetComponentInParent<GrindPath>();
			}

			// Keep GrindLines synced to GrindNodes
			for(var i = 0; i < grindLines.Count; i++) {
				if(ReferenceRecoveryUtil.GetCurrent(grindLines[i], out var current)) {
					grindLines[i] = current;
				}

				var line = grindLines[i];
				if (line != null) {
					line.RebuildWithRedDebugShape();
				} else {
					// Cannot synchronously remove missing references from the list, because they might be recreated in a moment!

					// When undoing a line deletion, Unity restores the line in this order:
					// 1. Nodes' references to the line are restored first, show as "missing"
					// 2. This OnValidate is called, sees "missing" references
					// 3. Deleted line is restored, references are no longer "missing"

					EditorApplication.delayCall += () => {
						if(this == null) return;
						ReferenceRecoveryUtil.GetCurrent(line, out var current);
						if(current == null) {
							RemoveLine(line);
						}
					};
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

		private void OnDestroy() {
			// Auto-destroy attached GrindLines when node is deleted
			// This is meant for user interaction, to execute when the user deletes the object
			// But it's flaky when triggered in other scenarios.

			// TODO find a better way to respond when user presses "delete"
			foreach(var grindLine in grindLines) {
				if(grindLine != null) {
					// If we run synchronously, we trigger a bug when user deletes entire `Grind`.
					// Unity records the grindLine deletion twice on the undo stack, so undo-ing
					// re-creates the same grindLine twice.  Two objects w/same ID == Unity crashes.
					CoroutineUtils.RunNextTick(() => {
						if(grindLine != null) {
							Undo.DestroyObjectImmediate(grindLine.gameObject);
						}
					});
				}
			}
		}

		public void AddLine(GrindLine line) {
			if(!grindLines.Contains(line)) grindLines.Add(line);
		}
		public void RemoveLine(GrindLine line) {
			for(var i = 0; i < grindLines.Count; i++) {
				if(line == grindLines[i]) {
					grindLines.RemoveAt(i);
					return;
				}
			}
		}
		public void ClearLines() {
			grindLines.Clear();
		}
		#endif
	}
}
