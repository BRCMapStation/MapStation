using System;
using UnityEngine;

namespace Reptile
{
	// Token: 0x02000219 RID: 537
	public class RotateObject : MonoBehaviour
	{
		// Token: 0x040010AF RID: 4271
		[SerializeField]
		private Space rotationSpace;

		// Token: 0x040010B0 RID: 4272
		[SerializeField]
		private Vector3 rotateVelocity;

		// Token: 0x040010B1 RID: 4273
		private Transform referenceTransform;

		// Token: 0x040010B2 RID: 4274
		private Transform myTransform;

		// Token: 0x040010B3 RID: 4275
		private Rigidbody body;

		// Token: 0x040010B4 RID: 4276
		private Renderer[] renderers;

		// Token: 0x040010B5 RID: 4277
		private bool updateRendererVisibility;
	}
}
