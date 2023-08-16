using System;
using _Scripts.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ChooseBuyQuantityUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buyButtonText;
        [SerializeField] private Button buyButton;

        private void Start()
        {
            buyButton.onClick.AddListener(()=>Shop.Instance.ChooseNextShopOption());
            Shop.Instance.OnShopOptionChanged += SetBuyButtonText;
            
            SetBuyButtonText();
        }

        private void SetBuyButtonText()
        {
            buyButtonText.text = Shop.Instance.CurrentShopOption.DisplayText;
        }
    }

    public struct ShopOption
    {
        public string DisplayText { get; private set; }
        public int Value { get; private set; }
        public ShopOptionType Type { get; private set; }

        public ShopOption(string displayText, ShopOptionType type, int value)
        {
            DisplayText = displayText;
            Value = value;
            Type = type;
        }
    }

    public enum ShopOptionType
    {
        DefinedNumber,
        Percent
    }
}