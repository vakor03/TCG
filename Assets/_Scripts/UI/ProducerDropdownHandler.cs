using System;
using System.Collections.Generic;
using _Scripts.Core.Productions;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ProducerDropdownHandler : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;
        private List<Producer> _producers = new();

        public event Action<Producer> OnItemSelected;
        public bool IsItemSelected { get; private set; }
        public Producer SelectedItem { get; private set; }

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.onValueChanged.AddListener(SelectItem);
            
            _dropdown.ClearOptions();
        }

        private void SelectItem(int itemIndex)
        {
            SelectedItem = _producers[itemIndex];
            IsItemSelected = true;
            OnItemSelected?.Invoke(_producers[itemIndex]);
        }

        public void AddItems(List<Producer> producers)
        {
            foreach (var producer in producers)
            {
                AddItem(producer);
            }
        }

        public void SelectDefault()
        {
            SelectItem(0);
            _dropdown.RefreshShownValue();
        }

        public void AddItem(Producer producer)
        {
            _producers.Add(producer);
            _dropdown.options.Add(
                new TMP_Dropdown.OptionData(producer.Name));

        }
    }
}