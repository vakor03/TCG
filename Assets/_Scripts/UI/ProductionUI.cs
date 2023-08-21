#region

using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void Init(ProductionSO productionSO)
        {
            buyResourceButtonUI.Init(productionSO.ConnectedResource);
            _productionSO = productionSO;
            _production = RepositoriesHelper.GetRepository<ProductionsRepository>().GetProduction(productionSO);

            _production.OnProductionStarted += ProducerOnProductionStarted;
            _production.OnProductionCountChanged += ProductionOnProductionCountChanged;
            _production.OnProductionRateChanged += ProductionOnProductionRateChanged;
            InteractorsHelper.GetInteractor<ResourcesInteractor>().OnResourceQuantityChanged +=
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
            countText.text = GameManager.Instance.InteractorsBase.GetInteractor<ResourcesInteractor>()
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
            progressBarUI.FillAndReset(RepositoriesHelper.GetRepository<ProductionsRepository>()
                .GetProduction(_productionSO).ProductionRate);
        }

        private void StartProduction()
        {
            _production.StartProduction();
        }
    }
}