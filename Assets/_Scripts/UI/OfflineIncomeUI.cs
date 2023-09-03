#region

using System;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class OfflineIncomeUI : StaticInstance<OfflineIncomeUI>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button closeButton;

        private ResourcesInteractor _resourcesInteractor;

        [Inject]
        public void Construct(ResourcesInteractor resourcesInteractor)
        {
            _resourcesInteractor = resourcesInteractor;
        }

        protected override void Awake()
        {
            base.Awake();

            closeButton.onClick.AddListener(Hide);
            Hide();
        }
      
        public void Setup(TimeSpan timeElapsed, float totalSeconds, OfflineIncomeManager offlineIncomeManager)
        {
            text.text = FormatTimeSpan(timeElapsed) + "\n" + totalSeconds + " seconds";
            closeButton.onClick.AddListener(() =>
            {
                var income = offlineIncomeManager.CalculateOfflineIncome(totalSeconds);
                foreach (var (resourceSO, quantity) in income)
                {
                    Debug.Log($"You got {quantity.ToScientificNotationString()} {resourceSO.name} for offline income");
                }

                _resourcesInteractor.AddResources(income);
                Hide();
            });
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            string formattedTime = $"{timeSpan.Minutes} min {timeSpan.Seconds} sec";
            return formattedTime;
        }
    }
}