using UnityEngine;

namespace _Scripts.UI.Tabs
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private TabData[] tabs;

        private void Start()
        {
            foreach (var tab in tabs)
            {
                tab.tabButton.Button.onClick.AddListener(() => ChangeActiveTab(tab));
            }

            ChangeActiveTab(tabs[0]);
        }

        private void ChangeActiveTab(TabData tabData)
        {
            foreach (var tab in tabs)
            {
                tab.tab.gameObject.SetActive(tab.Equals(tabData));
            }
        }
    }
}