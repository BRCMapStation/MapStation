using System.Collections.Generic;
using UnityEngine;

namespace Reptile
{
	[ExecuteInEditMode]
	public class GrindPath : MonoBehaviour
	{
		public bool hardCornerBoostsAllowed = true;

		public bool softCornerBoostsAllowed = true;

		public bool upwardsGrindJumpAllowed = true;

		public void CreatePath(List<Vector3> nodes)
		{
		}
	}
}
