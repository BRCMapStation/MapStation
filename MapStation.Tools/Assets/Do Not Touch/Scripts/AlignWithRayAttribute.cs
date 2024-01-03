using UnityEngine;

namespace Reptile
{
	public class AlignWithRayAttribute : PropertyAttribute
	{
		public LayerMask layerMask;

		public AlignWithRayAttribute(int layerMask)
		{
			this.layerMask = layerMask;
		}
	}
}
