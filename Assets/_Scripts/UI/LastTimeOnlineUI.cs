using System;
using _Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class LastTimeOnlineUI : StaticInstance<LastTimeOnlineUI>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button closeButton;


        protected override void Awake()
        {
            base.Awake();
            
            closeButton.onClick.AddListener(Hide);
            Hide();
        }

        public void Setup(TimeSpan timeElapsed)
        {
            text.text = FormatTimeSpan(timeElapsed);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            string formattedTime = $"{timeSpan.Minutes} min {timeSpan.Seconds} sec";
            return formattedTime;
        }
    }
}