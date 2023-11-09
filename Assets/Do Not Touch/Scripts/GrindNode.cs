using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Reptile
{
	[ExecuteInEditMode]
	public class GrindNode : MonoBehaviour
	{
		public List<GrindLine> grindLines = new List<GrindLine>();

		public bool retour;

		public GrindPath PathReference;

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

        private void Update()
        {
            foreach(GrindLine line in grindLines)
            {
				if (line == null)
					grindLines.Remove(line);
                else
                {
					line.Rebuild();
					line.transform.GetChild(0).localScale = new Vector3(0.3f, 0.3f, line.GetComponent<CapsuleCollider>().height);
				}
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

		private void OnDestroy()
		{
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
	}
}
