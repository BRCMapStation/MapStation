using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Globalization;

namespace Winterland.Common.Challenge {
    public class ChallengeUI : MonoBehaviour {
        public bool Visible {
            get {
                return gameObject.activeSelf;
            }

            set {
                if (value && !gameObject.activeSelf)
                    gameObject.SetActive(true);
                else if (!value && gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
        }

        public TextMeshProUGUI TimerLabel = null;

        private void Awake() {
            Visible = false;
        }

        private void Update() {
            var currentChallenge = ChallengeLevel.CurrentChallengeLevel;
            if (currentChallenge == null)
                return;
            TimerLabel.text = SecondsToMMSS(currentChallenge.Timer);
        }

        private string SecondsToMMSS(float seconds) {
            var minutesTotal = seconds / 60f;
            var secondsTotal = seconds;

            seconds = secondsTotal - (Mathf.Floor(minutesTotal) * 60f);
            var minutes = Mathf.Floor(minutesTotal);
            return $"{minutes.ToString("00", CultureInfo.InvariantCulture)}:{seconds.ToString("00.000", CultureInfo.InvariantCulture)}";
        }
    }
}
