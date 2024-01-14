using System.Collections.Generic;
using UnityEngine;

namespace Reptile
{
	public static class TransformExtentions
	{
		public static Vector3 down(this Transform transform)
		{
			return transform.up * -1f;
		}

		public static Vector3 left(this Transform transform)
		{
			return transform.right * -1f;
		}

		public static Vector3 back(this Transform transform)
		{
			return transform.forward * -1f;
		}

		public static void SetToIdentity(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localRotation = Quaternion.identity;
			trans.localScale = Vector3.one;
		}

		public static Transform[] GetAllChildren(this Transform transform)
		{
			Transform[] array = new Transform[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				array[i] = transform.GetChild(i);
			}
			return array;
		}

		public static Transform FindRecursive(this Transform transform, string name)
		{
			if (transform.name == name)
			{
				return transform;
			}
			Transform transform2 = null;
			foreach (Transform item in transform)
			{
				transform2 = item.FindRecursive(name);
				if ((bool)transform2)
				{
					break;
				}
			}
			return transform2;
		}

		public static Transform[] FindChilden(this Transform transform, string name)
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform item in transform)
			{
				if (item.name == name)
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		public static RectTransform RectTransform(this Transform t)
		{
			return t as RectTransform;
		}

		public static Transform LastChild(this Transform transform)
		{
			return transform.GetChild(transform.childCount - 1);
		}
	}
}
