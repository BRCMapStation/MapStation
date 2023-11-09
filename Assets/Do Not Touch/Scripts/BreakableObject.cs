using System.Collections;
using UnityEngine;

namespace Reptile
{
	public class BreakableObject : MonoBehaviour
	{
		public enum BreakOn
		{
			HITBOX = 0,
			BOOST = 1,
			RUSH_MODE = 2,
			MOVESTYLE_IN_AIR = 3,
			INLINE_SLIDE = 4
		}

		private int HP = 100;

		public BreakOn breakOn;

		private Collider curCollider;

		private Collision curCollision;

		public GameObject breakEffect;

		public AudioSource audioSource;

		private Coroutine waitForBreakingSoundRoutine;

		private Renderer[] breakableRenderers;

		private Collider[] breakableColliders;



		private void OnDestroy()
		{
			if (waitForBreakingSoundRoutine != null)
			{
				StopCoroutine(waitForBreakingSoundRoutine);
			}
		}



		private void ToggleRenderers(bool toggle)
		{
			for (int i = 0; i < breakableRenderers.Length; i++)
			{
				breakableRenderers[i].enabled = toggle;
			}
		}

		private void ToggleColliders(bool toggle)
		{
			for (int i = 0; i < breakableColliders.Length; i++)
			{
				breakableColliders[i].enabled = toggle;
			}
		}

		private IEnumerator WaitForBreakSoundToFinish()
		{
			while (audioSource.isPlaying)
			{
				yield return null;
			}
			ToggleRenderers(toggle: true);
			ToggleColliders(toggle: true);
			base.gameObject.SetActive(value: false);
		}
	}
}
