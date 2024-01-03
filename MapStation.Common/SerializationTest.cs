using System;
using UnityEngine;

namespace MapStation.Common {
    public class SerializationTest : MonoBehaviour {
        public SerializedStruct data = new SerializedStruct();
        [Serializable]
        public struct SerializedStruct {
            public string Foo;
            public string Bar;
        }

        private void Awake() {
            Debug.Log(String.Format("Serialized data: foo={0}, bar={1}", data.Foo, data.Bar));
        }
    }

}