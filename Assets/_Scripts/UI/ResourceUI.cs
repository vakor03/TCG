using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private ResourceSO resourceSO;

        private void Start()
        {
            InteractorsHelper.GetInteractor<ResourcesInteractor>().OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;

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
            countText.text = InteractorsHelper.GetInteractor<ResourcesInteractor>().GetResourceQuantity(resourceSO)
                .ToScientificNotationString();
        }
    }
}