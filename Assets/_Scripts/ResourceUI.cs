using _Scripts.Enums;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI productionRate;
        [SerializeField] private TextMeshProUGUI productionCount;
        [SerializeField] private TextMeshProUGUI countText;
        
        [SerializeField] private Button buyButton;

        [SerializeField] private ProgressBarUI progressBarUI;


        private Resource _targetResource;

        private void Start()
        {
            _targetResource = ResourcesRepository.Instance.GetResource(resourceType);
            
            _targetResource.Producer.OnProductionStarted += ProducerOnProductionStarted;
            _targetResource.OnCountChanged += TargetResourceOnCountChanged;

            progressBarUI.Button.onClick.AddListener(StartProduction);
            buyButton.onClick.AddListener(BuyProducer);
            
            SetDefaultValues();
        }

        private void BuyProducer()
        {
            _targetResource.IncreaseCount(1);
        }

        private void TargetResourceOnCountChanged()
        {
            countText.text = _targetResource.Count.ToString();
        }

        private void SetDefaultValues()
        {
            image.sprite = _targetResource.SO.Sprite;
            productionRate.text = _targetResource.ProductionRate.ToString();
            productionCount.text = _targetResource.ProductionCount.ToString();

            countText.text = _targetResource.Count.ToString();
        }
        
        private void ProducerOnProductionStarted()
        {
            progressBarUI.FillAndReset(_targetResource.ProductionRate);
        }
        
        private void StartProduction()
        {
            _targetResource.Producer.StartProduction();
        }
    }
}