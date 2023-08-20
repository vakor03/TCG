﻿using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Interactors;
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
        private ResourceSO _resourceSO;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        private BigInteger _currentBuyQuantity;

        private void Start()
        {
        }

        public void Init(ResourceSO resourceSO)
        {
            _resourceSO = resourceSO;
            buyButton.onClick.AddListener(BuyProducer);

            Shop.Instance.OnShopOptionChanged += RecalculateCurrentBuyQuantity;
            InteractorsHelper.GetInteractor<ResourcesInteractor>()
                .OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;

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