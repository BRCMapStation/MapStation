using System.Reflection;
using UnityEngine;

namespace Reptile
{
	public class SunFlareGPU : MonoBehaviour
	{
		[SerializeField]
		private Camera sunOcclusionCamera;

		[SerializeField]
		private Transform sunFlareTransform;

		[SerializeField]
		private Shader sunFlareReplacementShader;

		[SerializeField]
		private RenderTexture sunRenderTexture;
	}
}
