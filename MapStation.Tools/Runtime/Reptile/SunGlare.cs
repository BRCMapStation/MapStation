using UnityEngine;

namespace Reptile
{
	[RequireComponent(typeof(LensFlare))]
	[RequireComponent(typeof(Light))]
	public class SunGlare : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float minLightIntensityFactor = 0.25f;

		[Range(0f, 100f)]
		public float maxFlareBrightness = 50f;

		[Range(0f, 0.2f)]
		public float smoothValue = 0.05f;

		[Range(10f, 55f)]
		public float minLookAtAngle = 45f;
	}
}
