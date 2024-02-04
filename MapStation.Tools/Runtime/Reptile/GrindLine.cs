using MapStation.Components;
using UnityEditor;
using UnityEngine;

namespace Reptile
{
	[RequireComponent(typeof(CapsuleCollider))]
	[ExecuteInEditMode]
	[SelectionBase]
	public class GrindLine : MonoBehaviour
	{
		public enum LineState
		{
			none = -1,
			valid = 0,
			missingNode = 1,
			misaligned = 2
		}

		public GrindPath PathReference;

		[SerializeField]
		public GrindNode[] nodes = new GrindNode[2];

		[Header("Line specifics, can be overruled by path's settings")]
		public bool isPole;

		public bool cornerBoost = true;

		public bool upwardsGrindJump = true;

		public bool alwaysFlipBack;

		public ref GrindNode n0 => ref nodes[0];

		public ref GrindNode n1 => ref nodes[1];

		public Vector3 defaultDir => (n1 - n0).normalized;

		public GrindPath Path
		{
			get
			{
				GrindPath grindPath = PathReference;
				if (grindPath == null)
				{
					grindPath = (PathReference = GetComponentInParent<GrindPath>());
				}
				return grindPath;
			}
		}

		public Vector3 GetLineDir(Vector3 dir)
		{
			Vector3 rhs = n1 - n0;
			if (Vector3.Dot(dir, rhs) < 0f)
			{
				rhs = n0 - n1;
			}
			return rhs.normalized;
		}

		public Vector3 GetLineDir(GrindNode nextNode)
		{
			return ((nextNode == n1) ? (n1 - n0) : (n0 - n1)).normalized;
		}

		public Vector3 GetLineDir(Vector3 dir, Vector3 up)
		{
			Vector3 to = n1 - n0;
			if (Vector3.SignedAngle(dir, to, up) < 0f)
			{
				to = n0 - n1;
			}
			return to.normalized;
		}

		public GrindNode GetOtherNode(GrindNode n)
		{
			if (n == n0)
			{
				return n1;
			}
			if (n == n1)
			{
				return n0;
			}
			Debug.LogWarningFormat("Node <b>\"{0}\"<\b> does not belong to line <b>\"{1}\"<\b>", n.name, base.name);
			return null;
		}

		public int RemoveNode(GrindNode nodeToRemove)
		{
			int result = -1;
			if (n0 == nodeToRemove)
			{
				n0.grindLines.Remove(this);
				result = 0;
				n0 = null;
			}
			else if (n1 == nodeToRemove)
			{
				n1.grindLines.Remove(this);
				result = 1;
				n1 = null;
			}
			else
			{
				Debug.LogWarning(nodeToRemove.name + " does not belong to " + base.name);
			}
			return result;
		}

		internal bool ContainsNode(GrindNode grindNode)
		{
			if (!(n0 == grindNode))
			{
				return n1 == grindNode;
			}
			return true;
		}

		public LineState IsValid()
		{
			LineState result = LineState.valid;
			if (n0 == null || n1 == null)
			{
				result = LineState.missingNode;
			}
			return result;
		}

		[ContextMenu("Build Grind Line")]
		public void Rebuild()
		{	
			if (IsValid() != LineState.missingNode)
			{
				if (n0.transform.parent != base.transform.parent || n1.transform.parent != base.transform.parent)
				{
					RebuildW();
					return;
				}
				Vector3 vector = n1.transform.localPosition - n0.transform.localPosition;
				base.transform.localPosition = n0.transform.localPosition + vector.normalized * vector.magnitude * 0.5f;
				base.transform.localRotation = Quaternion.LookRotation(vector.normalized);
				CapsuleCollider component = GetComponent<CapsuleCollider>();
				component.isTrigger = true;
				component.direction = 2;
				// Seems silly but avoids log spam about SendMessage within `OnValidate`
				if(base.gameObject.layer != 11) base.gameObject.layer = 11;
				component.height = vector.magnitude + component.radius * 2f;
			}
		}

		public void RebuildW()
		{
			if (IsValid() != LineState.missingNode)
			{
				Vector3 vector = n1.transform.position - n0.transform.position;
				base.transform.position = n0.transform.position + vector.normalized * vector.magnitude * 0.5f;
				if (vector.magnitude != 0)
					base.transform.rotation = Quaternion.LookRotation(vector.normalized);
				CapsuleCollider component = GetComponent<CapsuleCollider>();
				component.isTrigger = true;
				component.direction = 2;
				// Seems silly but avoids log spam about SendMessage within `OnValidate`
				if(base.gameObject.layer != 11) base.gameObject.layer = 11;
				component.height = vector.magnitude + component.radius * 2f;
			}
		}

		public Vector3 ProjectOnLine(Vector3 characterPos)
		{
			return n0 + Vector3.Project(characterPos - n0, n1 - n0);
		}

		public Vector3 SnapPosToLine(Vector3 pos)
		{
			Vector3 vector = ProjectOnLine(pos);
			Vector3 vector2 = n1 - n0;
			float num = ((!(vector == n0.position)) ? ((vector - n0).magnitude / vector2.magnitude) : 0f);
			if (Vector3.Dot((vector - n0).normalized, vector2.normalized) < 0f)
			{
				num *= -1f;
			}
			if (num <= 0f)
			{
				vector = n0.position;
			}
			else if (num >= 1f)
			{
				vector = n1.position;
			}
			return vector;
		}

		private Vector3 GetNormalAtRelLinePos(float posOnLine)
		{
			return Vector3.Slerp(n0.normal, n1.normal, posOnLine);
		}

		public Vector3 GetNormalAtPos(Vector3 pos)
		{
			Vector3 vector = ProjectOnLine(pos);
			Vector3 vector2 = n1 - n0;
			float posOnLine = (vector - n0).magnitude / vector2.magnitude;
			return GetNormalAtRelLinePos(posOnLine);
		}

		public bool IsOnLine(float posOnLine)
		{
			if (posOnLine >= 0f)
			{
				return posOnLine <= 1f;
			}
			return false;
		}

		public bool IsOnLine(Vector3 pos, Vector3 dir)
		{
			return IsOnLine(GetRelativePosOnLine(pos, dir));
		}

		public float GetRelativePosOnLine(Vector3 pos, Vector3 dir)
		{
			float relativePosOnLine = GetRelativePosOnLine(pos);
			if (Vector3.Dot(dir.normalized, (n1 - n0).normalized) < 0f)
			{
				return 1f - relativePosOnLine;
			}
			return relativePosOnLine;
		}

		public float GetRelativePosOnLine(Vector3 pos, GrindNode nextNode)
		{
			float relativePosOnLine = GetRelativePosOnLine(pos);
			if (nextNode == n0)
			{
				return 1f - relativePosOnLine;
			}
			return relativePosOnLine;
		}

		private float GetRelativePosOnLine(Vector3 pos)
		{
			Vector3 vector = ProjectOnLine(pos);
			Vector3 vector2 = n1 - n0;
			float num = ((!(vector == n0.position)) ? ((vector - n0.position).magnitude / vector2.magnitude) : 0f);
			if (Vector3.Dot((vector - n0).normalized, vector2.normalized) < 0f)
			{
				num *= -1f;
			}
			return num;
		}

		public float GetAbsoluteLinePos(Vector3 pos, Vector3 dir)
		{
			float absoluteLinePos = GetAbsoluteLinePos(pos);
			if (Vector3.Dot(dir.normalized, (n1 - n0).normalized) < 0f)
			{
				return (n1 - n0).magnitude - absoluteLinePos;
			}
			return absoluteLinePos;
		}

		public float GetAbsoluteLinePos(Vector3 pos, GrindNode nextNode)
		{
			float absoluteLinePos = GetAbsoluteLinePos(pos);
			if (nextNode == n0)
			{
				return (n1 - n0).magnitude - absoluteLinePos;
			}
			return absoluteLinePos;
		}

		private float GetAbsoluteLinePos(Vector3 pos)
		{
			Vector3 vector = ProjectOnLine(pos);
			_ = n1 - n0;
			return (vector - n0).magnitude;
		}

		public float Length()
		{
			return (n1 - n0).magnitude;
		}

		public float Radius()
		{
			return GetComponent<CapsuleCollider>().radius;
		}

		internal GrindNode GetNextNode(Vector3 dir)
		{
			if (!(Vector3.Dot(dir.normalized, defaultDir) < 0f))
			{
				return n1;
			}
			return n0;
		}

		internal GrindLine GetNextLine(Vector3 dir, GrindNode nextNode = null)
		{
			GrindLine result = null;
			if (nextNode == null)
			{
				nextNode = GetNextNode(dir);
			}
			if (nextNode.grindLines.Count == 2)
			{
				if (!(nextNode.grindLines[0] == this))
				{
					return nextNode.grindLines[0];
				}
				return nextNode.grindLines[1];
			}
			float num = 360f;
			foreach (GrindLine grindLine in nextNode.grindLines)
			{
				if (!(grindLine == this) && !grindLine.isPole)
				{
					Vector3 to = grindLine.GetOtherNode(nextNode) - nextNode;
					float num2 = Vector3.Angle(dir, to);
					if (num2 < num)
					{
						num = num2;
						result = grindLine;
					}
				}
			}
			return result;
		}

		#if UNITY_EDITOR

		private Grind grind_;
        public Grind Grind => grind_ = grind_ != null ? grind_ : GetComponentInParent<Grind>();

		public CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();

		void OnEnable() {
			transform.hideFlags |= HideFlags.NotEditable;
			capsuleCollider.hideFlags |= HideFlags.NotEditable;
		}

		public GameObject redDebugShape => transform.childCount > 0 ? transform.GetChild(0).gameObject : null;

		public void RebuildWithRedDebugShape() {
			ReferenceRecoveryUtil.Repair(ref n0);
			ReferenceRecoveryUtil.Repair(ref n1);

			Rebuild();

			// TODO conditional is hack to avoid errors while spline grinds are being hooked up. they don't have red debug shapes
			var r = redDebugShape;
			if(r) {
				Undo.RecordObject(r.transform, null);
				r.transform.localScale = new Vector3(0.3f, 0.3f, capsuleCollider.height);
				// When you accidentally drag-select the red debug shape alongside nodes, you will accidentally move
				// the debug shape. Causes wonky drag behavior. Prevent this by resetting localPosition
				// TODO EXCEPT THIS DOESN'T FIX THE PROBLEM
				r.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			}
		}

		private void OnDestroy() {
			if(n0 != null) {
				GrindActions.RemoveLineFromNode(this, n0);
			}
			if(n1 != null) {
				GrindActions.RemoveLineFromNode(this, n1);
			}
		}

		private void OnValidate() {
			if(EditorUtility.IsPersistent(this) || MapBuilderStatus.IsBuilding) return;
			PathReference = PathReference != null ? PathReference : GetComponentInParent<GrindPath>();
		}
		#endif
	}
}
