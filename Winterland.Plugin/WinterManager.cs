using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Plugin
{
    public class WinterManager : MonoBehaviour
    {
        public static WinterManager Instance
        {
            get;
            private set;
        }

        public static WinterManager Create()
        {
            var gameObject = new GameObject("Winter Manager");
            return gameObject.AddComponent<WinterManager>();
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
