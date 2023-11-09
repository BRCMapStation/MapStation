using System.Collections.Generic;
using UnityEngine;

namespace Reptile
{
	public class WallrunLine : MonoBehaviour
	{
		public Transform node0;

		public Transform node1;

		public WallrunLine attachedWallrunLine0;

		public WallrunLine attachedWallrunLine1;

		private List<WallrunLine> wallrunLines = new List<WallrunLine>();

		public void Awake()
		{
			AddBrothersToFamily(ref wallrunLines);
		}

		public Transform NearestNode(Vector3 pos)
		{
			if ((node0.position - pos).sqrMagnitude < (node1.position - pos).sqrMagnitude)
			{
				return node0;
			}
			return node1;
		}

		public bool HasNode(Transform node)
		{
			if (!(node == node1))
			{
				return node == node0;
			}
			return true;
		}

		public bool IsSameFamily(WallrunLine line)
		{
			return wallrunLines.Contains(line);
		}

		public void AddBrothersToFamily(ref List<WallrunLine> family)
		{
			family.Add(this);
			if (attachedWallrunLine0 != null && !family.Contains(attachedWallrunLine0))
			{
				attachedWallrunLine0.AddBrothersToFamily(ref family);
			}
			if (attachedWallrunLine1 != null && !family.Contains(attachedWallrunLine1))
			{
				attachedWallrunLine1.AddBrothersToFamily(ref family);
			}
		}

		public float DotToCenter(Vector3 pos, Vector3 dir)
		{
			return Vector3.Dot((base.transform.position - pos).normalized, dir);
		}
	}
}
