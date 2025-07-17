using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public interface IShrapnelPoolFacade
    {
        public GameObject Get(Vector3 position);
        public void Return(GameObject shrapnel);
        public void ScheduleReturn(GameObject shrapnel);
    }
}