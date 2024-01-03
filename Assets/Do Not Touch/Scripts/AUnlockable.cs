using UnityEngine;

namespace Reptile
{
	public abstract class AUnlockable : ScriptableObject
	{
		[SerializeField]
		[HideInInspector]
		protected string uid = string.Empty;

		[SerializeField]
		protected bool isDefault;
	}
}
