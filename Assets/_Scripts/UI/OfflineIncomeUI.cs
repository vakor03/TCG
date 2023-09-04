#region

using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Managers;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class OfflineIncomeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button closeButton;

        private OfflineIncomeManager _offlineIncomeManager;

        [Inject]
        private void Construct(OfflineIncomeManager offlineIncomeManager)
        {
            _offlineIncomeManager = offlineIncomeManager;
        }

        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
            Hide();
        }

        public void UpdateText()
        {
            text.text = FormatTimeSpan(_offlineIncomeManager.GetTimeSinceLastOnline());
        }

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                LogIncome(_offlineIncomeManager.OfflineIncome);
                _offlineIncomeManager.ReceiveIncome();
                
                Hide();
            });
        }

        private void LogIncome(Dictionary<ResourceSO, BigInteger> income)
        {
            foreach (var (resourceSO, quantity) in income)
            {
                Debug.Log($"You got {quantity.ToScientificNotationString()} {resourceSO.name} for offline income");
            }
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