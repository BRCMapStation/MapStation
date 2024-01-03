using UnityEngine;

namespace Reptile
{
	public class Pickup : MonoBehaviour
	{
		public enum PickUpType
		{
			BOOST_CHARGE = 0,
			BOOST_BIG_CHARGE = 1,
			MUSIC_UNLOCKABLE = 3,
			GRAFFITI_UNLOCKABLE = 4,
			OUTFIT_UNLOCKABLE = 5,
			FORTUNE_BOOST = 6,
			BRIBE = 7,
			DYNAMIC_REP = 8,
			REP = 9,
			MAP = 10,
			MOVESTYLE_SKIN_UNLOCKABLE = 11
		}

		public static float boostChargeValue = 1.5f;

		public static float boostBigChargeValue = 10f;

		public float respawnTime = 60f;

		public PickUpType pickupType;

		public AUnlockable unlock;

		public GameObject pickupObject;

		[SerializeField]
		private GameObject pickupEffect;
	}
}
