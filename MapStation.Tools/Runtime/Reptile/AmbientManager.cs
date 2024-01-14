using UnityEngine;

namespace Reptile
{
	[ExecuteInEditMode]
	public class AmbientManager : MonoBehaviour
	{
		[Header("Standard Ambient")]
		public Color AmbientEnvLight;

		public Color AmbientEnvShadow;

		private float revertTransitionDuration;

		private Color oldLight;

		private Color oldShadow;

		private Color curLight;

		private Color curShadow;

		private Color targetLight;

		private Color targetShadow;

		private bool ambientTriggerActive;

		private AmbientTrigger curAmbientTrigger;

		private bool transitioning;

		private float timer;

		private float transitionDuration;

		private float revertCheckTimer;

		private void Awake()
		{
			ApplyAmbientChange(AmbientEnvLight, AmbientEnvShadow);
		}

		private void Update()
		{
		    ApplyAmbientChange(AmbientEnvLight, AmbientEnvShadow);
		}

		private void ApplyAmbientChange(Color light, Color shadow)
		{
			curLight = light;
			curShadow = shadow;
			Shader.SetGlobalColor("LightColor", curLight);
			Shader.SetGlobalColor("ShadowColor", curShadow);
			Shader.SetGlobalFloat("shadowFadeDistance", QualitySettings.shadowDistance - 5f);
		}

		private void StartAmbientTransition(Color light, Color shadow, float duration, AmbientTrigger newAT)
		{
			oldLight = curLight;
			oldShadow = curShadow;
			targetLight = light;
			targetShadow = shadow;
			timer = 0f;
			transitionDuration = duration;
			transitioning = true;
			curAmbientTrigger = newAT;
			ambientTriggerActive = curAmbientTrigger != null;
			revertCheckTimer = 0f;
		}

		public void RevertAmbient(float duration)
		{
			StartAmbientTransition(AmbientEnvLight, AmbientEnvShadow, duration, null);
		}
	}
}
