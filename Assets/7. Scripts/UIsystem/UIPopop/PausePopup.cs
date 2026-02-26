using EasyTransition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace JS
{
    public class PausePopup : UIBase
    {
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private float startDelay = 0f;

        [Header("Slider")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TransitionSettings transitionSettings;


        private IUIService uiService;
        private IAudioService _audio;

        [Inject]
        public void Construct(IUIService uiService,IAudioService audioService)
        {
            this.uiService = uiService;
            this._audio = audioService;
        }

        protected override void Awake()
        {
            base.Awake();

            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinue);

            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetry);

            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenu);

            if (musicSlider != null)
                musicSlider.value = _audio.GetMusicVolume();

            if (sfxSlider != null)
                sfxSlider.value = _audio.GetSfxVolume();

            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(OnMusicChanged);

            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        }

        private void OnMusicChanged(float value)
        {
            _audio.SetMusicVolume(value);
        }

        private void OnSfxChanged(float value)
        {
            _audio.SetSfxVolume(value);
        }

        private void OnContinue()
        {
            if (uiService.IsVisible<SelectCardPopup>())
            {
                uiService.Hide<PausePopup>();
            }
            else
            {
                Time.timeScale = 1f;
                uiService.Hide<PausePopup>();
            }

        }

        private void OnRetry()
        {
            Time.timeScale = 1f;
            SceneLoader.RetryGameplay(transitionSettings, startDelay);
            uiService.Hide<PausePopup>();

        }

        private void OnMenu()
        {
            SceneLoader.LoadSelectMap(transitionSettings, startDelay);
            Time.timeScale = 1f;
        }
    }
}
