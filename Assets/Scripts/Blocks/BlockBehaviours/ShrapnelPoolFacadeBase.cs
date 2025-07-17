using Assets.Scripts.Blocks;
using Assets.Scripts.SharedKernel;
using UnityEngine;

public abstract class ShrapnelPoolFacadeBase : MonoBehaviour, IShrapnelPoolFacade
{
    protected ObjectPool<GameObject> Pool;

    public abstract GameObject Get(Vector3 position);
    public abstract void Return(GameObject shrapnel);
    public abstract void ScheduleReturn(GameObject shrapnel);
}
