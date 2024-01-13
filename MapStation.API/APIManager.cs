using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.API {
    public static class APIManager {
        public static IMapStationAPI API = null;
        /// <summary>
        /// Whether the API has been initialized by MapStation and is available for use.
        /// </summary>
        public static bool Initialized = false;
        /// <summary>
        /// Called when the API gets initialized by MapStation and is available for use.
        /// </summary>
        public static event Action OnInitialized;
        public static void Initialize(IMapStationAPI api) {
            API = api;
            Initialized = true;
            OnInitialized?.Invoke();
        }
    }
}
