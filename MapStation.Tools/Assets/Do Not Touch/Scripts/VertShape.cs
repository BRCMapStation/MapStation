using System;
using UnityEngine;

namespace Reptile
{
	[ExecuteInEditMode]
	public class VertShape : MonoBehaviour
	{
		public Vector3 topLeft;

		public Vector3 topRight;

		public Vector3 bottomLeft;


		public Vector3 bottomLeftWorld;


		public Quaternion rotLocal;


		public Quaternion rotWorld;


		public Vector3 size;

		private bool inited;

		[ContextMenu("Initialize")]

		public void Init()
		{
			{
				bottomLeft = new Vector3(GetComponent<MeshCollider>().sharedMesh.bounds.extents.x, -GetComponent<MeshCollider>().sharedMesh.bounds.extents.y, GetComponent<MeshCollider>().sharedMesh.bounds.extents.z);
				bottomLeft += GetComponent<MeshCollider>().sharedMesh.bounds.center;
				topLeft = new Vector3(GetComponent<MeshCollider>().sharedMesh.bounds.extents.x, GetComponent<MeshCollider>().sharedMesh.bounds.extents.y, -GetComponent<MeshCollider>().sharedMesh.bounds.extents.z);
				topLeft += GetComponent<MeshCollider>().sharedMesh.bounds.center;
				topRight = new Vector3(-GetComponent<MeshCollider>().sharedMesh.bounds.extents.x, GetComponent<MeshCollider>().sharedMesh.bounds.extents.y, -GetComponent<MeshCollider>().sharedMesh.bounds.extents.z);
				topRight += GetComponent<MeshCollider>().sharedMesh.bounds.center;
				bottomLeftWorld = base.transform.TransformPoint(bottomLeft);
				Vector3 from = base.transform.TransformDirection(topRight - topLeft);
				from.y = 0f;
				float angle = Vector3.SignedAngle(from, Vector3.right, Vector3.up);
				rotLocal = Quaternion.AngleAxis(angle, Vector3.up);
				rotWorld = Quaternion.Inverse(rotLocal);
				size = topRight - bottomLeft;
				size.x = Mathf.Abs(size.x) * base.transform.lossyScale.x;
				size.y = Mathf.Abs(size.y) * base.transform.lossyScale.y;
				size.z = Mathf.Abs(size.z) * base.transform.lossyScale.z;
				if (Mathf.Abs(topRight.x - topLeft.x) < Mathf.Abs(topRight.z - topLeft.z))
				{
					float x = size.x;
					size.x = size.z;
					size.z = x;
				}
			}
		}

        private void Update()
        {
			Init();
        }

        public float GetArcY(float y)
		{
			y -= base.transform.position.y;
			y /= topLeft.y - bottomLeft.y;
			return Mathf.Acos(Mathf.Clamp01(1f - y)) / (MathF.PI / 2f);
		}

		public Vector3 VertPosToWorld(Vector2 vp)
		{
			float f = MathF.PI / 2f * vp.y / size.y;
			Vector3 vector = default(Vector3);
			vector.x = vp.x;
			vector.y = (1f - Mathf.Cos(f)) * size.y;
			vector.z = Mathf.Sin(f) * size.z;
			return bottomLeftWorld + rotWorld * vector;
		}

		public Vector2 WorldToVertPos(Vector3 w)
		{
			Vector3 vector = rotLocal * (w - bottomLeftWorld);
			float num = Mathf.Asin(Mathf.Clamp01(vector.z / size.z));
			float num2 = Mathf.Acos(Mathf.Clamp01(1f - vector.y / size.y));
			float num3 = (num + num2) * 0.5f;
			vector.y = size.y * num3 / (MathF.PI / 2f);
			return vector;
		}

		public void CreateDot(Vector3 pos, string name = "Dot", float size = 0.03f)
		{
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.name = name;
			Transform obj2 = obj.transform;
			UnityEngine.Object.Destroy(obj2.GetComponent<Collider>());
			obj2.localScale = Vector3.one * size;
			obj2.position = pos;
		}
	}
}
