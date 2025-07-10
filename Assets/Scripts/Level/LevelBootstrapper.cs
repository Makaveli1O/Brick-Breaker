namespace Assets.Scripts.Level
{
    using UnityEngine;
    using Assets.Scripts.SharedKernel;
    using UnityEngine.Experimental.AI;

    public class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameObject _levelDesignerPrefab;
        [SerializeField] private GameObject _pausePanelPrefab;
        private IPauseController _pauseController;

        void Awake()
        {
            Instantiate(_levelDesignerPrefab);
            ILevelDesigner designer = _levelDesignerPrefab.GetComponent<ILevelDesigner>();

            Instantiate(_pausePanelPrefab);
            IPauseController pausePanel = _pausePanelPrefab.GetComponent<IPauseController>();

            SimpleServiceLocator.Register<ILevelDesigner>(designer);
            SimpleServiceLocator.Register<IPauseController>(pausePanel);
            SimpleServiceLocator.Register<ILevelCatalog>(new StaticLevelCatalog());
        }
    }
}