using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class ProducerVisualsUI : MonoBehaviour
    {
        [FormerlySerializedAs("producer")] [SerializeField] private ProducerMB producerMb;
        [SerializeField] private Image fillImage;

        private void Update()
        {
            float progress = producerMb.GetProgressNormalized();
            fillImage.fillAmount = progress;
        }
    }
}