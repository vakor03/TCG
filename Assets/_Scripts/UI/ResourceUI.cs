#region

using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using Zenject;

#endregion

namespace _Scripts.UI
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private ResourceSO resourceSO;

        private ResourcesInteractor _resourcesInteractor;

        [Inject]
        public void Construct(ResourcesInteractor resourcesInteractor)
        {
            _resourcesInteractor = resourcesInteractor;
        }

        private void Start()
        {
            _resourcesInteractor.OnResourceQuantityChanged +=
                ResourcesRepositoryOnResourceQuantityChanged;

            UpdateCountText();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == resourceSO)
            {
                UpdateCountText();
            }
        }

        private void UpdateCountText()
        {
            countText.text = _resourcesInteractor.GetResourceQuantity(resourceSO)
                .ToScientificNotationString();
        }
    }
}