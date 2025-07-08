using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using UnityEngine;

//TODO refactor?
public class LevelBounds : MonoBehaviour
{
    public enum WallScreenPosition
    {
        Left,
        Right,
        Bottom,
        Top
    }

    public static float GetHudOffsetInUnits() => _OFFSET * Camera.main.orthographicSize * _TWO / Screen.height;
    private const float _TWO = 2.0f;
    private const float _OFFSET = 50f;
    private float _wallThickness = 0f;
    private float _left = 0f;
    private float _right = 0f;
    private float _top = 0f;
    private float _bottom = 0f;
    private float _width = 0f;
    private float _height = 0f;
    [SerializeField] private GameObject _wallBlockHorizontalPrefab;
    [SerializeField] private GameObject _wallBlockVerticalPrefab;
    private Transform _horizontalTransform;
    private Transform _verticalTransform;

    void Start()
    {
        CreateBounds();
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

    public void CreateBounds()
    {
        InitializeLevelBounds();

        float hudOffsetInUnits = GetHudOffsetInUnits();

        GameObject topWall = CreateWall(
            new Vector2((_left + _right) / _TWO,
            _top + _wallThickness / _TWO - hudOffsetInUnits),
            new Vector2(_width, _wallThickness),
            WallScreenPosition.Top
        );
        GameObject bottomWall = CreateWall(
            new Vector2((_left + _right) / _TWO,
            _bottom - _wallThickness / _TWO),
            new Vector2(_width, _wallThickness),
            WallScreenPosition.Bottom
        );
        CreateWall(
            new Vector2(_left - _wallThickness / _TWO,
            (_top + _bottom) / _TWO),
            new Vector2(_wallThickness, _height),
            WallScreenPosition.Left
        );
        CreateWall(
            new Vector2(_right + _wallThickness / _TWO,
            (_top + _bottom) / _TWO),
            new Vector2(_wallThickness, _height),
            WallScreenPosition.Right
        );

        FixHorizontalOverlap(topWall);
        FixHorizontalOverlap(bottomWall);
    }

    // Returns wall GO as a whole containing blocks as children
    private GameObject CreateWall(Vector2 position, Vector2 size, WallScreenPosition wallPosition)
    {
        GameObject wallParent = new GameObject(wallPosition + "Wall");

        bool isHorizontal = wallPosition == WallScreenPosition.Top || wallPosition == WallScreenPosition.Bottom;
        GameObject blockPrefab = isHorizontal ? _wallBlockHorizontalPrefab : _wallBlockVerticalPrefab;

        SpriteRenderer blockRenderer = blockPrefab.GetComponent<SpriteRenderer>();
        if (blockRenderer == null)
        {
            Debug.LogError("Wall block prefab is missing a SpriteRenderer.");
            return null;
        }

        float blockSize = isHorizontal
            ? blockRenderer.bounds.size.x
            : blockRenderer.bounds.size.y;

        int count = Mathf.CeilToInt(isHorizontal ? size.x / blockSize : size.y / blockSize);

        Vector2 adjustedPosition = position;

        if (wallPosition == WallScreenPosition.Top)
            adjustedPosition.y -= blockRenderer.bounds.extents.y;
        else if (wallPosition == WallScreenPosition.Bottom)
            adjustedPosition.y += blockRenderer.bounds.extents.y;
        else if (wallPosition == WallScreenPosition.Left)
            adjustedPosition.x += blockRenderer.bounds.extents.x;
        else if (wallPosition == WallScreenPosition.Right)
            adjustedPosition.x -= blockRenderer.bounds.extents.x;

        Vector2 startPos = isHorizontal
            ? new Vector2(adjustedPosition.x - (count - 1) * blockSize / 2f, adjustedPosition.y)
            : new Vector2(adjustedPosition.x, adjustedPosition.y - (count - 1) * blockSize / 2f);

        for (int i = 0; i < count; i++)
        {
            Vector2 blockPos = isHorizontal
                ? new Vector2(startPos.x + i * blockSize, startPos.y)
                : new Vector2(startPos.x, startPos.y + i * blockSize);

            var block = Instantiate(blockPrefab, blockPos, Quaternion.identity, wallParent.transform);
            if (wallPosition == WallScreenPosition.Left)
                block.AddComponent<GameOverTrigger>();
        }

        // Adjust wallParent scale to fit exactly without overlapping
        if (isHorizontal)
        {
            float totalLength = count * blockRenderer.bounds.size.x;
            float scaleX = size.x / totalLength;
            wallParent.transform.localScale = new Vector3(scaleX, 1f, 1f);
            _horizontalTransform = wallParent.transform;
        }
        else
        {
            float totalLength = count * blockRenderer.bounds.size.y;
            float scaleY = size.y / totalLength;
            wallParent.transform.localScale = new Vector3(1f, scaleY, 1f);
            _verticalTransform = wallParent.transform;
        }

        return wallParent;
    }

    private void FixHorizontalOverlap(GameObject horizontalWall)
    {
        // Get full horizontal wall width in world units
        Bounds bounds = Utils2D.GetWorldBounds(horizontalWall);

        // Amount to subtract in world units (e.g. total vertical wall thickness)
        float overlapWorld = GetVerticalWallThickness();

        // Current total width of the horizontal wall (before scaling)
        float currentWidth = bounds.size.x;

        // Calculate target width after subtracting overlap
        float targetWidth = currentWidth - overlapWorld;

        // Compute required X scale
        float scaleX = targetWidth / currentWidth;

        horizontalWall.transform.localScale = new Vector3(scaleX, 1f, 1f);
    }

    private float GetVerticalWallThickness()
    {
        SpriteRenderer renderer = _wallBlockVerticalPrefab.GetComponent<SpriteRenderer>();
        float width = renderer.bounds.size.x * _wallBlockVerticalPrefab.transform.localScale.x;
        return width * 2f; // left and right
    }
}
