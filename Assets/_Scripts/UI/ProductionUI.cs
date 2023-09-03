#region

using _Scripts.Core.Productions;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class ProductionUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI productionRate;
        [SerializeField] private TextMeshProUGUI productionCount;
        [SerializeField] private TextMeshProUGUI countText;

        [SerializeField] private ProgressBarUI progressBarUI;
        [SerializeField] private BuyResourceButtonUI buyResourceButtonUI;

        private Production _production;
        private ProductionSO _productionSO;

        private ProductionDatabase _productionDatabase;
        private ResourcesInteractor _resourcesInteractor;
        
        private InteractorsBase _interactorsBase;

        [Inject]
        public void Construct(InteractorsBase interactorsBase,
            ProductionDatabase productionDatabase)
        {
            _interactorsBase = interactorsBase;
            _productionDatabase = productionDatabase;
        }

        public void Init(ProductionSO productionSO)
        {
            _productionSO = productionSO;
        }

        private void Start()
        {
            _resourcesInteractor = _interactorsBase.GetInteractor<ResourcesInteractor>();
            
            buyResourceButtonUI.Init(_productionSO.ConnectedResource);
            _production = _productionDatabase.GetProduction(_productionSO);

            _production.OnProductionStarted += ProducerOnProductionStarted;
            _production.OnProductionCountChanged += ProductionOnProductionCountChanged;
            _production.OnProductionRateChanged += ProductionOnProductionRateChanged;
            _resourcesInteractor.OnResourceQuantityChanged +=
                ResourcesRepositoryOnResourceQuantityChanged;

            progressBarUI.Button.onClick.AddListener(StartProduction);

            SetDefaultValues();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == _productionSO.ConnectedResource)
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
            countText.text = _resourcesInteractor
                .GetResourceQuantity(_productionSO.ConnectedResource).ToScientificNotationString();
        }

        private void SetDefaultValues()
        {
            image.sprite = _productionSO.Sprite;
            productionRate.text = _production.ProductionRate.ToString();
            SetProductionCountText();

            SetResourceCountText();
        }

        private void ProducerOnProductionStarted()
        {
            progressBarUI.FillAndReset(_productionDatabase
                .GetProduction(_productionSO).ProductionRate);
        }

        private void StartProduction()
        {
            _production.StartProduction();
        }
    }
}