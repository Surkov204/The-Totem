using EasyTransition;
using JS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingPopup : UIBase
{
        [Header("Buttons")]
        [SerializeField] private Button continueButton;

        [Header("Slider")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private IUIService uiService;
        private IAudioService _audio;

        [Inject]
        public void Construct(IUIService uiService, IAudioService audioService)
        {
            this.uiService = uiService;
            this._audio = audioService;
        }

        protected override void Awake()
        {
            base.Awake();

            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinue);

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
                uiService.Hide<SettingPopup>();
        }
}
