using Assets.Scripts.GameHandler;
using TMPro;
using UnityEngine;

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
    [SerializeField] private Sprite _wallSpriteHorizontal;
    [SerializeField] private Sprite _wallSpriteVertical;

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

        CreateWall(
            new Vector2((_left + _right) / _TWO,
            _top + _wallThickness / _TWO - hudOffsetInUnits),
            new Vector2(_width, _wallThickness),
            WallScreenPosition.Top
        );
        CreateWall(
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
    }

    private void CreateWall(Vector2 position, Vector2 size, WallScreenPosition wallPosition)
    {
        GameObject wall;
        switch (wallPosition)
        {
            case WallScreenPosition.Left:
                wall = new GameObject("LeftWall");
                wall.AddComponent<GameOverTrigger>();
                AssignVisualsToWallGO(wall, _wallSpriteVertical);
                break;
            case WallScreenPosition.Right:
                wall = new GameObject("RightWall");
                AssignVisualsToWallGO(wall, _wallSpriteVertical);
                break;
            case WallScreenPosition.Bottom:
                wall = new GameObject("BottomWall");
                AssignVisualsToWallGO(wall, _wallSpriteHorizontal);
                break;
            case WallScreenPosition.Top:
                wall = new GameObject("TopWall");
                AssignVisualsToWallGO(wall, _wallSpriteHorizontal);
                break;
            default:
                wall = new GameObject("Wall");
                break;
        }
        wall.transform.position = position;

        var collider = wall.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;

        wall.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private SpriteRenderer AssignVisualsToWallGO(GameObject wall, Sprite sprite)
    {
        SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return sr;
    }
}
