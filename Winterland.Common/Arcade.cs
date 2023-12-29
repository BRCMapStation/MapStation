using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class Arcade : MonoBehaviour {
        public Texture2D MapPinTexture = null;
#if !UNITY_EDITOR
        private MapPin pin = null;
#endif
        private void Awake() {
            ArcadeManager.Instance.RegisterArcade(this);
        }

        private void MakePin() {
#if !UNITY_EDITOR
            var mapController = Mapcontroller.Instance;
            pin = mapController.CreatePin(MapPin.PinType.CypherPin);
            pin.AssignGameplayEvent(this.gameObject);
            pin.InitMapPin(MapPin.PinType.Pin);
            pin.OnPinEnable();
            var pinRenderer = pin.GetComponentInChildren<MeshRenderer>();
            pinRenderer.material.mainTexture = MapPinTexture;
#endif
        }

        private void Start() {
            MakePin();
            UpdateAvailability();
        }

        public void UpdateAvailability() {
            gameObject.SetActive(WinterProgress.Instance.LocalProgress.ArcadeUnlocked);
#if !UNITY_EDITOR
            if (pin != null) {
                if (WinterProgress.Instance.LocalProgress.ArcadeUnlocked)
                    pin.EnablePin();
                else
                    pin.DisablePin();
            }
#endif
        }
    }
}
