using System.Linq;
using Assets.Scripts.GameHandler;
using Assets.Scripts.Level;
using Assets.Scripts.SharedKernel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _container;
    private float _buttonHeight;
    private ILevelCatalog _levelCatalog;
    private ISceneLoader _sceneLoader;

    void Awake()
    {
        _levelCatalog = SimpleServiceLocator.Resolve<ILevelCatalog>();
        _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
        _buttonHeight = _buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }

    void Start()
    {
        var levelList = _levelCatalog.GetAvailableLevels().ToList();
        for (int i = 0;  i < levelList.Count; i++)
        {
            GameObject button = Instantiate(_buttonPrefab, _container);
            button.GetComponentInChildren<TextMeshProUGUI>().text = levelList[i].DisplayName;

            button.transform.position = new Vector2(
                button.transform.position.x,
                button.transform.position.y - (_buttonHeight * i)
            );

            var id = levelList[i].Id;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                LoadLevel(id);
            });
        }
    }

    private void LoadLevel(int levelId)
    {
        Debug.Log("Loading level: " + levelId);
        _sceneLoader.LoadScene(SceneNames.Level0, levelId);
    }
}
