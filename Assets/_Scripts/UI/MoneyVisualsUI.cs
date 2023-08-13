using System.Numerics;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class MoneyVisualsUI : MonoBehaviour
    {
        private Money _money;
        [SerializeField] private TextMeshProUGUI moneyText;
        
        private void Start()
        {
            _money = Money.Instance;
            _money.OnAfterValueChanged += MoneyOnAfterValueChanged;
            
            Initialize();
        }

        private void Initialize()
        {
            SetValue(_money.Value);
        }

        private void MoneyOnAfterValueChanged()
        {
            SetValue(_money.Value);
        }

        private void SetValue(BigInteger newValue)
        {
            moneyText.text = newValue.ToString();
        }
    }
}