using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Tabs
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        public Button Button { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
        }
    }
}