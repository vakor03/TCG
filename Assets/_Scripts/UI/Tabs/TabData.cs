using System;

namespace _Scripts.UI.Tabs
{
    [Serializable]
    public struct TabData
    {
        public Tab tab;
        public TabButton tabButton;

        public bool Equals(TabData other)
        {
            return Equals(tab, other.tab) && Equals(tabButton, other.tabButton);
        }

        public override bool Equals(object obj)
        {
            return obj is TabData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(tab, tabButton);
        }
    }
}