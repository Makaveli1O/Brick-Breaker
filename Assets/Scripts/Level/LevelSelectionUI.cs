using Assets.Scripts.Level;
using Assets.Scripts.SharedKernel;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _container;

    private ILevelCatalog _levelCatalog;
    private ILevelDesigner _levelDesigner;

    void Start()
    {
        _levelCatalog = SimpleServiceLocator.Resolve<ILevelCatalog>();
        _levelDesigner = SimpleServiceLocator.Resolve<ILevelDesigner>();

        foreach (var level in _levelCatalog.GetAvailableLevels())
        {
            var button = Instantiate(_buttonPrefab, _container);
            button.GetComponentInChildren<Text>().text = level.DisplayName;

            var id = level.Id;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                LoadLevel(id);
            });
        }
    }

    private void LoadLevel(int levelId)
    {
        Debug.Log("Loading level: " + levelId);

        _levelDesigner.LoadLevel(levelId);
    }
}
