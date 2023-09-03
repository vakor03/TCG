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
        private InteractorsBase _interactorsBase;
        private ShopOptionManager _shopOptionManager;

        [Inject]
        public void Construct(InteractorsBase interactorsBase,
            ShopOptionManager shopOptionManager)
        {
            _interactorsBase = interactorsBase;
            _shopOptionManager = shopOptionManager;
        }

        public void Init(ResourceSO resourceSO)
        {
            _resourceSO = resourceSO;
        }

        private void Start()
        {
            _resourcesInteractor = _interactorsBase.GetInteractor<ResourcesInteractor>();
          
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
            _currentBuyQuantity = Shop.Instance.CalculateCurrentBuyQuantity(_resourceSO);
            buyButtonText.text =
                $"Buy {_currentBuyQuantity.ToScientificNotationString()} {_resourceSO.Name}";
        }

        private void BuyProducer()
        {
            Shop.Instance.TryBuyResource(_resourceSO, _currentBuyQuantity);
        }
    }
}