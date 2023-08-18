﻿using _Scripts.Helpers;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private ProductionSO productionSO;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI productionRate;
        [SerializeField] private TextMeshProUGUI productionCount;
        [SerializeField] private TextMeshProUGUI countText;

        [SerializeField] private ProgressBarUI progressBarUI;

        private Production _production;

        private void Start()
        {
            _production = ProductionsRepository.Instance.GetProduction(productionSO);

            _production.OnProductionStarted += ProducerOnProductionStarted;
            _production.OnProductionCountChanged += ProductionOnProductionCountChanged;
            _production.OnProductionRateChanged += ProductionOnProductionRateChanged;
            ResourcesRepository.Instance.OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;
            
            progressBarUI.Button.onClick.AddListener(StartProduction);

            SetDefaultValues();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == productionSO.ConnectedResource)
            {
                SetResourceCountText();
            }
        }


        private void ProductionOnProductionCountChanged()
        {
            SetProductionCountText();
        }

        private void SetProductionCountText()
        {
            productionCount.text = _production.ProductionCount.ToScientificNotationString();
        }

        private void ProductionOnProductionRateChanged()
        {
            productionRate.text = _production.ProductionRate.ToString();
        }
        
        private void SetResourceCountText()
        {
            countText.text = ResourcesRepository.Instance.GetResourceQuantity(productionSO.ConnectedResource).ToScientificNotationString();
        }

        private void SetDefaultValues()
        {
            image.sprite = productionSO.Sprite;
            productionRate.text = _production.ProductionRate.ToString();
            SetProductionCountText();

           SetResourceCountText();
        }

        private void ProducerOnProductionStarted()
        {
            progressBarUI.FillAndReset(ProductionsRepository.Instance.GetProduction(productionSO).ProductionRate);
        }

        private void StartProduction()
        {
            _production.StartProduction();
        }
    }
}