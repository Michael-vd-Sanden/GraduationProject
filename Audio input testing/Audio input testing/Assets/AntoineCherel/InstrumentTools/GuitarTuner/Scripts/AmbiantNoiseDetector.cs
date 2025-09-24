using System.Collections.Generic;
using ACInstrumentTools.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Basic frequency display without any smoothing
    /// </summary>
    public class AmbiantNoiseDetector : MonoBehaviour
    {
        [Header("Frequency Analyser")]
        [SerializeField] private FrequencyAnalyser analyser;

        [Header("Settings")]
        [SerializeField] private bool autoStart = false;
        
        [Header("Outputs")]
        [SerializeField] private TMP_Text textVolume;
        [SerializeField] private Slider volumeSlider;

        [Header("Inputs")]
        [SerializeField] private TMP_Dropdown microphoneDropdown;

        float maxVolume = 1;

        void Start()
        {
            analyser.Init();

            // Initializes the dropdown
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (string mic in Microphone.devices)
            {
                options.Add(new TMP_Dropdown.OptionData(mic));
            }
            microphoneDropdown.AddOptions(options);

            microphoneDropdown.onValueChanged.AddListener(OnMicrophoneDropdownChanged);

            if (autoStart)
                StartTuner();
            else
                StopTuner();
        }

        private void OnEnable()
        {
            analyser.onSampleAnalysed.AddListener(OnSampleAnalysed);
        }

        private void OnDisable()
        {
            analyser.onSampleAnalysed.RemoveListener(OnSampleAnalysed);
        }


        void OnMicrophoneDropdownChanged(int micId)
        {
            analyser.SetMicrophoneIndex(micId);
        }

        public virtual void StartTuner()
        {
            analyser.StartMicrophoneAnalyser();
        }

        public virtual void StopTuner()
        {
            analyser.StopMicrophoneAnalyser();
        }

        public void OnToggle()
        {
            if (analyser.recording)
                StopTuner();
            else
                StartTuner();
        }

        private void OnSampleAnalysed(Analyse analyse)
        {
            float currentVolume = analyse.sampleVolume;

            textVolume.text = currentVolume.ToString("F1");

            if (analyse.sampleVolume > maxVolume)
            {
                maxVolume = analyse.sampleVolume;
            }

            volumeSlider.SetValueWithoutNotify(currentVolume / maxVolume);
        }
    }
}
