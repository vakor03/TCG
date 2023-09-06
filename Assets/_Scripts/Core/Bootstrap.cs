using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Core
{
    public class Bootstrap : MonoBehaviour
    {
        private void Update()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}