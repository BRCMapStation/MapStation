using UnityEngine;

namespace Reptile
{
	public abstract class AProgressable : MonoBehaviour
	{
		[SerializeField]
		protected string uid = string.Empty;

		public string Uid => uid;
	}
}
