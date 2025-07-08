using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public enum WallScreenPosition { Left, Right, Bottom, Top }

    private const float _TWO = 2.0f;
    private const float _OFFSET = 50f;

    [SerializeField] private GameObject _wallBlockHorizontalPrefab;
    [SerializeField] private GameObject _wallBlockVerticalPrefab;

    private float _wallThickness;
    private float _left, _right, _top, _bottom, _width, _height;

    void Start()
    {
        CreateBounds();
    }

    public static float GetHudOffsetInUnits() =>
        _OFFSET * Camera.main.orthographicSize * _TWO / Screen.height;

    private void CreateBounds()
    {
        InitializeLevelBounds();

        float hudOffset = GetHudOffsetInUnits();
        _wallThickness = GetWallThickness(_wallBlockVerticalPrefab); // assumed uniform

        GameObject topWall = CreateWall(
            GetWallPosition(WallScreenPosition.Top, hudOffset),
            new Vector2(_width, _wallThickness),
            WallScreenPosition.Top
        );

        GameObject bottomWall = CreateWall(
            GetWallPosition(WallScreenPosition.Bottom),
            new Vector2(_width, _wallThickness),
            WallScreenPosition.Bottom
        );

        GameObject leftWall = CreateWall(
            GetWallPosition(WallScreenPosition.Left),
            new Vector2(_wallThickness, _height),
            WallScreenPosition.Left
        );

        GameObject rightWall = CreateWall(
            GetWallPosition(WallScreenPosition.Right),
            new Vector2(_wallThickness, _height),
            WallScreenPosition.Right
        );

        FixHorizontalOverlap(topWall);
        FixHorizontalOverlap(bottomWall);
    }

    private void InitializeLevelBounds()
    {
        Camera cam = Camera.main;
        _height = _TWO * cam.orthographicSize;
        _width = _height * cam.aspect;

        _left = cam.transform.position.x - _width / _TWO;
        _right = cam.transform.position.x + _width / _TWO;
        _top = cam.transform.position.y + _height / _TWO;
        _bottom = cam.transform.position.y - _height / _TWO;
    }

    private Vector2 GetWallPosition(WallScreenPosition pos, float hudOffset = 0f)
    {
        return pos switch
        {
            WallScreenPosition.Top => new Vector2((_left + _right) / _TWO, _top - hudOffset),
            WallScreenPosition.Bottom => new Vector2((_left + _right) / _TWO, _bottom),
            WallScreenPosition.Left => new Vector2(_left, (_top + _bottom) / _TWO),
            WallScreenPosition.Right => new Vector2(_right, (_top + _bottom) / _TWO),
            _ => Vector2.zero
        };
    }

    private GameObject CreateWall(Vector2 position, Vector2 size, WallScreenPosition wallPosition)
    {
        GameObject wallParent = new GameObject(wallPosition + "Wall");

        bool isHorizontal = wallPosition is WallScreenPosition.Top or WallScreenPosition.Bottom;
        GameObject prefab = isHorizontal ? _wallBlockHorizontalPrefab : _wallBlockVerticalPrefab;
        SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();

        if (renderer == null)
        {
            Debug.LogError("Wall block prefab is missing a SpriteRenderer.");
            return null;
        }

        float blockSize = isHorizontal ? renderer.bounds.size.x : renderer.bounds.size.y;
        int count = Mathf.CeilToInt((isHorizontal ? size.x : size.y) / blockSize);

        Vector2 adjustedPos = AdjustWallPosition(position, wallPosition, renderer);
        Vector2 startPos = isHorizontal
            ? new Vector2(adjustedPos.x - (count - 1) * blockSize / 2f, adjustedPos.y)
            : new Vector2(adjustedPos.x, adjustedPos.y - (count - 1) * blockSize / 2f);

        CreateWallBlocks(wallParent.transform, prefab, blockSize, count, startPos, isHorizontal, wallPosition);
        ApplyWallScaling(wallParent, count, renderer, size, isHorizontal);

        return wallParent;
    }

    private Vector2 AdjustWallPosition(Vector2 pos, WallScreenPosition position, SpriteRenderer renderer)
    {
        float extent = renderer.bounds.extents.x;
        return position switch
        {
            WallScreenPosition.Top => pos - new Vector2(0f, renderer.bounds.extents.y),
            WallScreenPosition.Bottom => pos + new Vector2(0f, renderer.bounds.extents.y),
            WallScreenPosition.Left => pos + new Vector2(extent, 0f),
            WallScreenPosition.Right => pos - new Vector2(extent, 0f),
            _ => pos
        };
    }

    private void CreateWallBlocks(Transform parent, GameObject prefab, float blockSize, int count, Vector2 startPos, bool isHorizontal, WallScreenPosition wallPosition)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 blockPos = isHorizontal
                ? new Vector2(startPos.x + i * blockSize, startPos.y)
                : new Vector2(startPos.x, startPos.y + i * blockSize);

            var block = Instantiate(prefab, blockPos, Quaternion.identity, parent);
            if (wallPosition == WallScreenPosition.Left)
                block.AddComponent<GameOverTrigger>();
        }
    }

    private void ApplyWallScaling(GameObject wallParent, int count, SpriteRenderer renderer, Vector2 size, bool isHorizontal)
    {
        float totalLength = count * (isHorizontal ? renderer.bounds.size.x : renderer.bounds.size.y);
        float scale = (isHorizontal ? size.x : size.y) / totalLength;
        wallParent.transform.localScale = isHorizontal
            ? new Vector3(scale, 1f, 1f)
            : new Vector3(1f, scale, 1f);
    }

    private void FixHorizontalOverlap(GameObject horizontalWall)
    {
        float overlapWorld = GetWallThickness(_wallBlockVerticalPrefab) * 2f;
        Bounds bounds = Utils2D.GetWorldBounds(horizontalWall);
        float currentWidth = bounds.size.x;
        float targetWidth = currentWidth - overlapWorld;
        float scaleX = targetWidth / currentWidth;
        horizontalWall.transform.localScale = new Vector3(scaleX, 1f, 1f);
    }

    private float GetWallThickness(GameObject prefab)
    {
        SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
        return renderer.bounds.size.x * prefab.transform.localScale.x;
    }
}
