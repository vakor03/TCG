using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Core.Productions
{
    public class ProductionUIContainerMB : MonoBehaviour
    {
        [field: SerializeField] public ProductionUI ProductionUITemplate { get; private set; }
    }
}