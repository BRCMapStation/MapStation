using System.Collections;
using UnityEngine;

namespace Reptile
{
	public class Teleport : MonoBehaviour
	{
		[Header("In case of a teleport, it will place you at the spawner of that teleport")]
		[Header("You can put a teleport OR a spawner in here, both work")]
		public Transform teleportTo;

		public bool giveSpeedAtSpawn = true;

		public bool letAIGoToTheNextWaypoint;

		public bool automaticallyReturnPlayerToLastSafeLocation;

		public bool doDamage;
	}
}
