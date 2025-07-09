using Assets.Scripts.SharedKernel;
using UnityEngine;
namespace Assets.Scripts.Blocks
{
    public class MoveBehaviour : MonoBehaviour, IUpdateBehaviour, IConfigurableBehaviour<MoveConfig>
    {
        public float Speed = 0.5f;
        public Vector3 EndPoint = Vector3.zero;
        public Vector3 StartPoint = Vector3.zero;
        private float time;
        public float PhaseOffset;

        public void OnUpdateExecute(Block context)
        {
            MoveBackAndForth();
        }

        private void MoveBackAndForth()
        {
            time += Time.deltaTime * Speed;
            float t = Mathf.PingPong(time, 1f);
            transform.position = Vector3.Lerp(StartPoint, EndPoint, t);
        }

        public void Configure(MoveConfig config)
        {
            Speed = config.Speed;
            StartPoint = config.StartPoint;
            EndPoint = config.EndPoint;
            PhaseOffset = config.PhaseOffset;

            time = Mathf.Repeat(PhaseOffset, 1f);

            if (EndPoint.Equals(Vector3.zero))
                EndPoint = Utils2D.GetAxisAlignedVisiblePoint(StartPoint);
        }
    }
}
