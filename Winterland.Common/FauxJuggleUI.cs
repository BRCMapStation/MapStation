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
        private float counterLabelDefaultSize = 0f;

        private void Awake() {
            Visible = false;
            highScoreLabelDefaultSize = highScoreLabel.fontSize;
            counterLabelDefaultSize = counterLabel.fontSize;
        }

        public void EndJuggle() {
            StopAllCoroutines();
            var progress = WinterProgress.Instance.LocalProgress;
            if (CurrentJuggleAmount > progress.FauxJuggleHighScore) {
                progress.FauxJuggleHighScore = CurrentJuggleAmount;
                progress.Save();
                UpdateHighScoreLabelNew();
                StartCoroutine(PlayHighScoreAnimation());
            }
            StartCoroutine(PlayCounterAnimation());
            StartCoroutine(PlayDisableUIAnimation());
            CurrentJuggleAmount = 0;
        }

        private void UpdateHighScoreLabel() {
            var progress = WinterProgress.Instance.LocalProgress;
            highScoreLabel.text = $"High score: {progress.FauxJuggleHighScore}";
        }

        private void UpdateHighScoreLabelNew() {
            var progress = WinterProgress.Instance.LocalProgress;
            highScoreLabel.text = $"High score: {progress.FauxJuggleHighScore}(NEW!)";
        }

        public void UpdateCounter(int number) {
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
        
        private IEnumerator PlayCounterAnimation() {
            var animationTimer = 0f;
            var animationCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            while (animationTimer < highScoreLabelDisplayDuration) {
                animationTimer += Core.dt;
                var function = animationCurve.Evaluate(Mathf.Min(1f, animationTimer / highScoreLabelAnimationDuration));
                counterLabel.fontSize = counterLabelDefaultSize + (highScoreLabelGrowSize * function);
                yield return null;
            }
        }
    }
}
