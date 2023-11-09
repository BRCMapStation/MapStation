using System.Collections;
using UnityEngine;

namespace Reptile
{
	public class Teleport : MonoBehaviour
	{
		private Coroutine teleportRoutine;

		[Header("In case of a teleport, it will place you at the spawner of that teleport")]
		[Header("You can put a teleport OR a spawner in here, both work")]
		public Transform teleportTo;

		public bool giveSpeedAtSpawn = false;

		public bool letAIGoToTheNextWaypoint;

		public bool automaticallyReturnPlayerToLastSafeLocation;

		public bool doDamage;

		private float fadeToBlackDurationDeathzone = 0.5f;

		private float blackDurationDeathzone = 1.75f;

		private float fadeOpenDurationDeathzone = 4.5f;

		private float fadeToBlackDurationDoor = 0.5f;

		private float blackDurationDoor = 0.8f;

		private float fadeOpenDurationDoor = 1.6f;

		private float fadeToBlackDuration;

		private float blackDuration;

		private float fadeOpenDuration;

		private int runHash = Animator.StringToHash("run");

		private void OnEnable()
		{
			teleportRoutine = null;
		}
	}
}
