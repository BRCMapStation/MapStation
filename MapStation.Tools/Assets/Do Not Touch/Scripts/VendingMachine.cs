using UnityEngine;
using UnityEngine.Events;

namespace Reptile
{
	public class VendingMachine : MonoBehaviour
	{
		public enum Reward
		{
			NOTHING = 0,
			SINGLE_NORMAL_DROP = 1,
			BUNCH_OF_SPECIAL_DROPS = 2,
			PILE_OF_NORMAL_DROPS = 3,
			UNLOCKABLE_DROP = 4
		}

		public UnityEvent OnFirstHit;

		public Reward[] rewards;

		public float singleDropForce;

		public float bunchDropForce = 0.5f;

		public float pileDropForce = 3.5f;

		public GameObject normalDrop;

		public GameObject specialDrop;

		public GameObject unlockableDrop;

		public Transform dropPoint;
	}
}
