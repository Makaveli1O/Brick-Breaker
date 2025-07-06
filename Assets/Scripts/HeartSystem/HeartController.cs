
using System;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.HeartSystem
{
    public class HeartController : IHeartController
    {
        private int _maxHearts;
        private int _heartCount;

        public int GetMaxHearts => _maxHearts;
        public int GetCurrentHeart => _heartCount;

        public event Action<int> OnHeartAdded;
        public event Action<int> OnHeartRemoved;

        public HeartController(int maxHearts)
        {
            _maxHearts = maxHearts;
            _heartCount = _maxHearts;
        }

        public void AddHeart()
        {
            if (_heartCount < _maxHearts)
            {
                _heartCount++;
                OnHeartAdded?.Invoke(_heartCount);
            }
        }

        public void RemoveHeart()
        {
            if (_heartCount > 0)
            {
                _heartCount--;
                OnHeartRemoved?.Invoke(_heartCount);
            }
        }
    }
}