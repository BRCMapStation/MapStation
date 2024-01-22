using UnityEngine;

namespace Reptile
{
	public abstract class AProgressable : MonoBehaviour
	{
		[SerializeField]
		[Uid]
		public string uid = string.Empty;

		public string Uid => uid;
	}
}
