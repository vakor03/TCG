using System;
using _Scripts.Helpers;
using _Scripts.Interactors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class OfflineIncomeUI : StaticInstance<OfflineIncomeUI>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button closeButton;


        protected override void Awake()
        {
            base.Awake();
            
            closeButton.onClick.AddListener(Hide);
            Hide();
        }

        public void Setup(TimeSpan timeElapsed,float totalSeconds, OfflineIncomeManager offlineIncomeManager)
        {
            text.text = FormatTimeSpan(timeElapsed) + "\n" + totalSeconds + " seconds";
            closeButton.onClick.AddListener(() =>
            {
                var income = offlineIncomeManager.CalculateOfflineIncome(totalSeconds);
                foreach (var(resourceSO, quantity) in income)
                {
                    Debug.Log($"You got {quantity.ToScientificNotationString()} {resourceSO.name} for offline income");
                }
                
                InteractorsHelper.GetInteractor<ResourcesInteractor>().AddResources(income);
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