using _Scripts.UI;
using UnityEngine;

namespace _Scripts
{
    public class ProductionUIContainerMB : MonoBehaviour
    {
        [field: SerializeField] public ProductionUI ProductionUITemplate { get; private set; }
    }
}