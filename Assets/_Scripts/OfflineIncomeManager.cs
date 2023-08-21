using System;
using System.Globalization;
using UnityEngine;

namespace _Scripts
{
    public class OfflineIncomeManager
    {
        private readonly string _dateTimeFormat = "u";
        private readonly CultureInfo _dateTimeCulture = CultureInfo.InvariantCulture;
        private const string LAST_TIME_ONLINE_KEY = "LAST_TIME_OFFLINE_KEY";
        private const float MAX_SECONDS_OFFLINE_COUNT = float.MaxValue;

        public event Action OnFirstGameEnter;
        public bool TryGetTimeFromLastTimeOnline(out double seconds, out TimeSpan difference)
        {
            if (PlayerPrefs.HasKey(LAST_TIME_ONLINE_KEY))
            {
                var timeNow = DateTime.UtcNow;
                var lastSaveTime = PlayerPrefs.GetString(LAST_TIME_ONLINE_KEY);
                var lastSaveDateTime = DateTime.ParseExact(lastSaveTime, _dateTimeFormat, _dateTimeCulture);
                difference = timeNow - lastSaveDateTime;
                seconds = Mathf.Clamp((float)(difference).TotalSeconds, 0f, MAX_SECONDS_OFFLINE_COUNT);
                return true;
            }
            else
            {
                seconds = 0;
                difference = new TimeSpan();
                OnFirstGameEnter?.Invoke();
                return false;
            }
        }

        public void Save()
        {
            var timeNow = DateTime.UtcNow;
            string timeString = timeNow.ToString(_dateTimeFormat, _dateTimeCulture);
            PlayerPrefs.SetString(LAST_TIME_ONLINE_KEY, timeString);
            PlayerPrefs.Save();
        }
    }
}