using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private float defaultFillAmount;

        [SerializeField] private Image background;
        [SerializeField] private Image fillImage;
        private TweenerCore<float, float, FloatOptions> _a;
        [field: SerializeField] public Button Button { get; private set; }

        private void Awake()
        {
            SetDefaultFillAmount();
        }

        public void StartFillAnimation(float targetFillAmount, float time)
        {
            fillImage.DOFillAmount(targetFillAmount, time);
        }

        public void ResetProgressBar()
        {
            fillImage.fillAmount = 0f;
        }

        public void FillAndReset(float time)
        {
            fillImage.DOFillAmount(1f, time).OnComplete(ResetProgressBar);
        }

        private void SetDefaultFillAmount()
        {
            fillImage.fillAmount = defaultFillAmount;
        }
    }
}