#region

using System;
using System.Numerics;
using _Scripts.Core.Productions;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using Codice.Client.Commands.WkTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class ProductionUI : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI productionRate;
        [SerializeField] private TextMeshProUGUI productionCount;
        [SerializeField] private TextMeshProUGUI countText;

        [SerializeField] private ProgressBarUI progressBarUI;
        [SerializeField] private BuyResourceButtonUI buyResourceButtonUI;

        private Producer _producer;
        private ProductionSO _productionSO;

        private ResourcesInteractor _resourcesInteractor;
        private ProductionDatabase _productionDatabase;

        [Inject]
        public void Construct(ResourcesInteractor resourcesInteractor,
            ProductionDatabase productionDatabase)
        {
            _resourcesInteractor = resourcesInteractor;
            _productionDatabase = productionDatabase;
        }

        public void Init(ProductionSO productionSO)
        {
            _productionSO = productionSO;
            _producer = _productionDatabase.GetProducer(_productionSO);
        }

        private void Start()
        {
            buyResourceButtonUI.Init(_productionSO.ConnectedResource);

            _resourcesInteractor.OnResourceQuantityChanged += ResourcesInteractorOnResourceQuantityChanged;
            _producer.OnProductionStarted += ProducerOnProducerStarted;
            _producer.OnStatsChanged += ProducerOnStatsChanged;
            
            _resourcesInteractor.OnResourceQuantityChanged +=
                ResourcesRepositoryOnResourceQuantityChanged;

            progressBarUI.Button.onClick.AddListener(StartProduction);

            SetDefaultValues();
        }

        private void ResourcesInteractorOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == _productionSO.ConnectedResource)
            {
                SetProductionCountText();
            }
        }

        private void ProducerOnStatsChanged()
        {
            SetProductionCountText();
            SetProductionRateText();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == _productionSO.ConnectedResource)
            {
                SetResourceCountText();
            }
        }

        private void SetProductionCountText()
        {
            productionCount.text = GetProductionCount().ToScientificNotationString();
        }

        private BigInteger GetProductionCount()
        {
            return _producer.CurrentStats.ProductionCount *
                   _resourcesInteractor.GetResourceQuantity(_productionSO.ConnectedResource);
        }

        private void SetProductionRateText()
        {
            productionRate.text = _producer.CurrentStats.ProductionRate.ToString();
        }

        private void SetResourceCountText()
        {
            countText.text = _resourcesInteractor
                .GetResourceQuantity(_productionSO.ConnectedResource).ToScientificNotationString();
        }

        private void SetDefaultValues()
        {
            image.sprite = _productionSO.Sprite;
            SetProductionRateText();
            SetProductionCountText();

            SetResourceCountText();
        }

        private void ProducerOnProducerStarted()
        {
            progressBarUI.FillAndReset(_producer.CurrentStats.ProductionRate);
        }

        private void StartProduction()
        {
            _producer.StartProduction();
        }

        public void Dispose()
        {
            _resourcesInteractor.OnResourceQuantityChanged -= ResourcesInteractorOnResourceQuantityChanged;
        }
    }
}