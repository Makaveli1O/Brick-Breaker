using Assets.Scripts.SharedKernel;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class HudScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform _innerCanvas;
        [SerializeField] private RectTransform _parentCanvasRect;


        public void AlignToTopWall(GameObject topWall)
        {
            float wallTopWorldY = Utils2D.GetWorldBounds(topWall).max.y;
            float cameraTopWorldY = Camera.main.transform.position.y + Camera.main.orthographicSize;

            Vector2 wallCanvasLocalY, cameraTopCanvasLocalY;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvasRect,
                Camera.main.WorldToScreenPoint(new Vector3(0, wallTopWorldY, 0)),
                Camera.main,
                out wallCanvasLocalY
            );

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvasRect,
                Camera.main.WorldToScreenPoint(new Vector3(0, cameraTopWorldY, 0)),
                Camera.main,
                out cameraTopCanvasLocalY
            );

            float height = cameraTopCanvasLocalY.y - wallCanvasLocalY.y;
            float middleY = (cameraTopCanvasLocalY.y + wallCanvasLocalY.y) / 2f;

            _innerCanvas.anchoredPosition = new Vector2(
                _innerCanvas.anchoredPosition.x,
                middleY
            );

            _innerCanvas.sizeDelta = new Vector2(
                _innerCanvas.sizeDelta.x,
                height
            );
        }

    }
}
