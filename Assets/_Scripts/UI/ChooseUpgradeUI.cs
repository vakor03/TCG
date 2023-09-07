using System;
using System.Collections.Generic;
using _Scripts.Core.Productions;
using _Scripts.Core.Productions.Upgrades;
using _Scripts.Repositories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
    public class ChooseUpgradeUI : MonoBehaviour
    {
        [SerializeField] private ProducerDropdownHandler producerDropdownHandler;

        [SerializeField] private Button applyAutomationUpdateButton;
        [SerializeField] private Button applyCountUpdateButton;
        [SerializeField] private Button applyRateUpdateButton;

        private ProductionDatabase _productionDatabase;
        
        private List<IUpgrade> _upgrades = new();

        private void Start()
        {
            producerDropdownHandler.OnItemSelected += ProducerDropdownOnItemSelected;
            
            applyAutomationUpdateButton.onClick.AddListener(()=>ApplyUpgrade(new AutomationUpgrade()));
            applyRateUpdateButton.onClick.AddListener(()=>ApplyUpgrade(new ProductionRateUpgrade(2)));
            applyCountUpdateButton.onClick.AddListener(()=>ApplyUpgrade(new ProductionCountUpgrade(2)));
            applyCountUpdateButton.onClick.AddListener(()=>ApplyUpgrade(new ProductionCountUpgrade(2)));
            
            Initialize();
        }
        
        private List<IHaveLevel> _haveLevels = new();
        private void ApplyUpgrade(IUpgrade upgrade)
        {
            if (!producerDropdownHandler.IsItemSelected)
            {
                return;
            }

            if (upgrade is IHaveLevel haveLevel)
            {
                _haveLevels.Add(haveLevel);
            }
            
            producerDropdownHandler.SelectedItem
                .AddUpgrade(upgrade);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var haveLevel in _haveLevels)
                {
                    haveLevel.ChangeLevel(haveLevel.Level + 1);
                }
            }
        }

        [Inject]
        private void Construct(ProductionDatabase productionDatabase)
        {
            _productionDatabase = productionDatabase;
        }

        private void Initialize()
        {
            foreach (var (_, producer) in _productionDatabase.ProducersMap)
            {
                producerDropdownHandler.AddItem(producer);
            }
            
            producerDropdownHandler.SelectDefault();

            Debug.Log($"{nameof(ChooseUpgradeUI)} Initialized");
        }

        private void ProducerDropdownOnItemSelected(Producer selectedProducer)
        {
            Debug.Log(selectedProducer.Name);
        }
    }
}