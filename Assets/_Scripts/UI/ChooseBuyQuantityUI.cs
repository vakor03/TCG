using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
    public class ChooseBuyQuantityUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buyButtonText;
        [SerializeField] private Button buyButton;

        private ShopOptionManager _shopOptionManager;

        private void Start()
        {
            buyButton.onClick.AddListener(() => _shopOptionManager.ChooseNextShopOption());
            _shopOptionManager.OnShopOptionChanged += SetBuyButtonText;

            SetBuyButtonText();
        }

        [Inject]
        private void Construct(ShopOptionManager shopOptionManager)
        {
            _shopOptionManager = shopOptionManager;
        }

        private void SetBuyButtonText()
        {
            buyButtonText.text = _shopOptionManager.CurrentShopOption.DisplayText;
        }
    }
}