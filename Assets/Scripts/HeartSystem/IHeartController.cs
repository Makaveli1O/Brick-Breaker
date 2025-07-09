using System;

namespace Assets.Scripts.HeartSystem
{
    public interface IHeartController
    {
        public int GetCurrentHeart { get; }
        public int GetMaxHearts { get; }
        public void AddHeart();
        public void RemoveHeart();
        
        event Action<int> OnHeartAdded;
        event Action<int> OnHeartRemoved;
    }
}