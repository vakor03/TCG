using System;
using _Scripts.Repositories;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class DebugHelper : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ProductionsRepository.Instance.GetProduction(
                        ProductionsRepository.Instance.ProductionSOs[0])
                    .ToggleAutoProduction();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ProductionsRepository.Instance.GetProduction(
                        ProductionsRepository.Instance.ProductionSOs[1])
                    .ToggleAutoProduction();
            }
        }
    }
}