#region

using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Managers;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class BuyResourceButtonUI : MonoBehaviour
    {
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        private BigInteger _currentBuyQuantity;

        private ResourcesInteractor _resourcesInteractor;
        private ResourceSO _resourceSO;
        
        private ShopOptionManager _shopOptionManager;
        private Shop _shop;

        [Inject]
        public void Construct(ResourcesInteractor resourcesInteractor,
            ShopOptionManager shopOptionManager,
            Shop shop)
        {
            _resourcesInteractor = resourcesInteractor;
            _shopOptionManager = shopOptionManager;
            _shop = shop;
        }

        public void Init(ResourceSO resourceSO)
        {
            _resourceSO = resourceSO;
        }

        private void Start()
        {
            buyButton.onClick.AddListener(BuyProducer);

            _shopOptionManager.OnShopOptionChanged += RecalculateCurrentBuyQuantity;
            _resourcesInteractor.OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;

            RecalculateCurrentBuyQuantity();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO ignored)
        {
            RecalculateCurrentBuyQuantity();
        }

        private void RecalculateCurrentBuyQuantity()
        {
            _currentBuyQuantity = _shop.CalculateCurrentBuyQuantity(_resourceSO);
            buyButtonText.text =
                $"Buy {_currentBuyQuantity.ToScientificNotationString()} {_resourceSO.Name}";
        }

        private void BuyProducer()
        {
            _shop.TryBuyResource(_resourceSO, _currentBuyQuantity);
        }
    }
}