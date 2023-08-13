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

        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        [SerializeField] private ProgressBarUI progressBarUI;

        private Production _production;
        private Resource _connectedResource;

        private void Start()
        {
            _production = ProductionsRepository.Instance.GetResourceProducer(productionSO);
            _connectedResource = ResourcesRepository.Instance.GetResource(productionSO.ConnectedResource);

            _production.OnProductionStarted += ProducerOnProductionStarted;
            _connectedResource.OnCountChanged += TargetResourceOnCountChanged;

            progressBarUI.Button.onClick.AddListener(StartProduction);
            buyButton.onClick.AddListener(BuyProducer);

            SetDefaultValues();
        }

        private void BuyProducer()
        {
            _connectedResource.Count += 1;
        }

        private void TargetResourceOnCountChanged()
        {
            countText.text = _connectedResource.Count.ToString();
        }

        private void SetDefaultValues()
        {
            image.sprite = productionSO.Sprite;
            productionRate.text = productionSO.ProductionRate.ToString();
            productionCount.text = productionSO.ProductionCount.ToString();
            buyButtonText.text = $"Buy x1 {_connectedResource.ResourceSO.Name}";

            countText.text = _connectedResource.Count.ToString();
        }

        private void ProducerOnProductionStarted()
        {
            progressBarUI.FillAndReset(productionSO.ProductionRate);
        }

        private void StartProduction()
        {
            _production.StartProduction();
        }
    }
}