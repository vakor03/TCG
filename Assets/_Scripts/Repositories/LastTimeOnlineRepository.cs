using System;
using System.Globalization;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class LastTimeOnlineRepository : IRepository
    {
        private const string LAST_TIME_ONLINE_KEY = "LAST_TIME_OFFLINE_KEY";
        private readonly CultureInfo _dateTimeCulture = CultureInfo.InvariantCulture;
        private readonly string _dateTimeFormat = "u";

        public DateTime LastTimeOnline { get; private set; }
        public DateTime TimeGameStarted { get; private set; }
        public bool IsFirstGameEnter { get; private set; }

        public void Initialize()
        {
            TimeGameStarted = DateTime.UtcNow;
            if (PlayerPrefs.HasKey(LAST_TIME_ONLINE_KEY))
            {
                var lastTimeOnlineString = PlayerPrefs.GetString(LAST_TIME_ONLINE_KEY);
                LastTimeOnline = DateTime.ParseExact(lastTimeOnlineString, _dateTimeFormat, _dateTimeCulture);
                IsFirstGameEnter = false;
            }
            else
            {
                IsFirstGameEnter = true;
                LastTimeOnline = DateTime.UtcNow;
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