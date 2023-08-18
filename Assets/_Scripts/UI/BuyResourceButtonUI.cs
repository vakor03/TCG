using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Managers;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class BuyResourceButtonUI : MonoBehaviour
    {
        [SerializeField] private ResourceSO resourceSO;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        private BigInteger _currentBuyQuantity;

        private void Start()
        {
            buyButton.onClick.AddListener(BuyProducer);

            Shop.Instance.OnShopOptionChanged += RecalculateCurrentBuyQuantity;
            ResourcesRepository.Instance.OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;
            
            RecalculateCurrentBuyQuantity();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO ignored)
        {
            RecalculateCurrentBuyQuantity();
        }

        private void RecalculateCurrentBuyQuantity()
        {
            _currentBuyQuantity = Shop.Instance.CalculateCurrentBuyQuantity(resourceSO);
            buyButtonText.text =
                $"Buy {_currentBuyQuantity.ToScientificNotationString()} {resourceSO.Name}";
        }

        private void BuyProducer()
        {
            Shop.Instance.TryBuyResource(resourceSO, _currentBuyQuantity);
        }
    }
}