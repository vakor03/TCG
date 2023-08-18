using _Scripts.Managers;
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
}