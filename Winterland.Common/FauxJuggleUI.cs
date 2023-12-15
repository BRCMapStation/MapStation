using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Reptile;

namespace Winterland.Common {
    public class FauxJuggleUI : MonoBehaviour {
        [HideInInspector]
        public int CurrentJuggleAmount = 0;

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

        [SerializeField]
        private TextMeshProUGUI counterLabel;
        [SerializeField]
        private TextMeshProUGUI highScoreLabel;
        [SerializeField]
        private float highScoreLabelGrowSize = 5f;
        [SerializeField]
        private float highScoreLabelAnimationDuration = 0.5f;
        [SerializeField]
        private float highScoreLabelDisplayDuration = 2f;

        private float highScoreLabelDefaultSize = 0f;
        private Color counterLabelOriginalColor;

        private void Awake() {
            Visible = false;
            highScoreLabelDefaultSize = highScoreLabel.fontSize;
            counterLabelOriginalColor = counterLabel.color;
        }

        public void EndJuggle() {
            counterLabel.color = counterLabelOriginalColor;
            StopAllCoroutines();
            var progress = WinterProgress.Instance.LocalProgress;
            if (CurrentJuggleAmount > progress.FauxJuggleHighScore) {
                progress.FauxJuggleHighScore = CurrentJuggleAmount;
                progress.Save();
                UpdateHighScoreLabelNew();
                StartCoroutine(PlayHighScoreAnimation());
                StartCoroutine(PlayDisableUIAnimation());
            } else
                StartCoroutine(PlayDisableUIAnimation());
            CurrentJuggleAmount = 0;
        }

        private void UpdateHighScoreLabel() {
            var progress = WinterProgress.Instance.LocalProgress;
            highScoreLabel.text = $"High score: <color=white>{progress.FauxJuggleHighScore}</color>";
        }

        private void UpdateHighScoreLabelNew() {
            var progress = WinterProgress.Instance.LocalProgress;
            highScoreLabel.text = $"High score: <color=white>{progress.FauxJuggleHighScore}</color> (NEW!)";
        }

        public void UpdateCounter(int number) {
            counterLabel.color = Color.white;
            StopAllCoroutines();
            Visible = true;
            UpdateHighScoreLabel();
            highScoreLabel.fontSize = highScoreLabelDefaultSize;
            CurrentJuggleAmount = number;
            counterLabel.text = CurrentJuggleAmount.ToString();
        }

        private IEnumerator PlayDisableUIAnimation() {
            yield return new WaitForSeconds(highScoreLabelDisplayDuration);
            Visible = false;
        }

        private IEnumerator PlayHighScoreAnimation() {
            var animationTimer = 0f;
            var animationCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            while (animationTimer < highScoreLabelDisplayDuration) {
                animationTimer += Core.dt;
                var function = animationCurve.Evaluate(Mathf.Min(1f, animationTimer / highScoreLabelAnimationDuration));
                highScoreLabel.fontSize = highScoreLabelDefaultSize + (highScoreLabelGrowSize * function);
                yield return null;
            }
        }
    }
}
