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
        private InteractorsBase _interactorsBase;

        [Inject]
        public void Construct(InteractorsBase interactorsBase)
        {
            _interactorsBase = interactorsBase;
        }

        private void Start()
        {
            _resourcesInteractor = _interactorsBase.GetInteractor<ResourcesInteractor>();

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