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
            var resource = ResourcesRepository.Instance.GetResource(resourceSO);
            
            resource.OnCountChanged += ResourceOnCountChanged;
            
            UpdateCountText();
        }

        private void ResourceOnCountChanged()
        {
            UpdateCountText();
        }

        private void UpdateCountText()
        {
            var resource = ResourcesRepository.Instance.GetResource(resourceSO);

            countText.text = resource.Count.ToString();
        }
    }
}