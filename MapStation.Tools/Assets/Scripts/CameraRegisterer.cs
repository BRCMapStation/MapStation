using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reptile
{
    public class CameraRegisterer : MonoBehaviour
    {
		private const float ASPECT_RATIO_16_9 = 1.78f;

		private const float SCENE_VIEW_PERCENTAGE = 0.78f;

		private const float MIN_FOV_WIDTH_FACTOR = 0f;

		private const float MAX_FOV_WIDTH_FACTOR = 0.33f;

		private const float LERP_RANGE_WIDTH_MULTIPLIER = 0.2f;

		private const float MIN_FOV_HEIGHT_FACTOR = -0.22f;

		private const float MAX_FOV_HEIGHT_FACTOR = 0.25f;

		private const float LERP_RANGE_HEIGHT_MULTIPLIER = 1.44f;

		[SerializeField]
		private bool useCustomCullingMask;

		[SerializeField]
		private LayerMask customCullingMask;

		private Camera storyCamera;

		private float calibratedCameraFov;

		private float currentCameraFov;

		private float currentAspectRatio;
	}
}
