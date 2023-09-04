using System;

namespace _Scripts.Managers
{
    public class LastTimeOnlineInteractor
    {
        private readonly LastTimeOnlineRepository _lastTimeOnlineRepository;

        public LastTimeOnlineInteractor(LastTimeOnlineRepository lastTimeOnlineRepository)
        {
            _lastTimeOnlineRepository = lastTimeOnlineRepository;
        }

        public TimeSpan GetTimeFromSinceTimeOnline() => DateTime.UtcNow - _lastTimeOnlineRepository.LastTimeOnline;

        public bool IsFirstTimePlaying() =>
            _lastTimeOnlineRepository.IsFirstGameEnter;
    }
}