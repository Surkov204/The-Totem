using EasyTransition;
using JS;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace js {
    public class MainMenuPopup : UIBase
    {
        [Header("Transition")]
        [SerializeField] private float startDelay = 0f;
        [SerializeField] private TransitionSettings transiontionSetting;
        [SerializeField] private Button OnPlayButton;
        [SerializeField] private Button OnListButton;
        [SerializeField] private Button OnSettingButton;
        [SerializeField] private Button OnExitButton;
        [SerializeField] private Button OnLootingButton;

        private IUIService uiService;

        [Inject]
        public void Construct(IUIService uiService)
        {
            this.uiService = uiService;
        }

        protected override void Awake()
        {
            base.Awake();
            OnPlayButton?.onClick.AddListener(OnPlayButtonClicked);
            OnListButton?.onClick.AddListener(OnListPress);
            OnSettingButton?.onClick.AddListener(OnSettingPress);
            OnExitButton?.onClick.AddListener(OnExitPress);
            OnLootingButton?.onClick.AddListener(OnLootingPress);
        }

        private void Start()
        {
            uiService.Show<MainMenuPopup>();
        }

        public void OnPlayButtonClicked()
        {
            SceneLoader.LoadSelectMap(transiontionSetting, startDelay);
            uiService.Hide<MainMenuPopup>();
        }

        public void OnListPress()
        {
            uiService.Show<PopupListDefender>();
        }

        public void OnSettingPress()
        {
            uiService.Show<SettingPopup>();
        }

        public void OnLootingPress()
        {
            uiService.Show<LootingPopup>();
        }

        public void OnExitPress()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}
