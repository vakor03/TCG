using System;
using System.Numerics;
using _Scripts.Helpers;
using UnityEngine;

namespace _Scripts
{
    public class Money : StaticInstance<Money>
    {
        public event Action OnBeforeValueChanged;
        public event Action OnAfterValueChanged;

        public BigInteger Value { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        private void Initialize()
        {
            var time = Time.time;
            Value = new BigInteger(time);
        }

        public void IncreaseValue(BigInteger value)
        {
            OnBeforeValueChanged?.Invoke();
            Value += value;
            OnAfterValueChanged?.Invoke();
        }

        public void DecreaseValue(BigInteger value)
        {
            OnBeforeValueChanged?.Invoke();
            Value -= value;
            OnAfterValueChanged?.Invoke();
        }
    }
}