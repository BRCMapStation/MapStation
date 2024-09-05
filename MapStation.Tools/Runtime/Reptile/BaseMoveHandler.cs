using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reptile {
    public abstract class BaseMoveHandler : MonoBehaviour {
        protected readonly Vector3 vectorUpTenThousand = Vector3.up * 10000f;
    }
}
