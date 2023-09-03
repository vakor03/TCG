namespace _Scripts.Managers
{
    public struct ShopOption
    {
        public string DisplayText { get; private set; }
        public int Value { get; private set; }
        public ShopOptionType Type { get; private set; }

        public ShopOption(string displayText, ShopOptionType type, int value)
        {
            DisplayText = displayText;
            Value = value;
            Type = type;
        }
    }
}