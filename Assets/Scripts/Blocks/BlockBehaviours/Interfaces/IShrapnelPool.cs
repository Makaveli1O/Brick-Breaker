using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public interface IShrapnelPool
    {
        GameObject Get(Vector3 position);
        void Return(GameObject shrapnel);
    }
}