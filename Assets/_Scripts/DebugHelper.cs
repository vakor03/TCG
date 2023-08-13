using UnityEngine;

namespace _Scripts
{
    public class DebugHelper : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Money.Instance.IncreaseValue(100);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Money.Instance.DecreaseValue(10);
            }
        }
    }
}