using System.Collections.Generic;
using ACInstrumentTools.Core;
using TMPro;
using UnityEngine;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Basic frequency display without any smoothing
    /// </summary>
    public class FrequencyDisplay : MonoBehaviour
    {
        [Header("Frequency Analyser")]
        [SerializeField] private FrequencyAnalyser analyser;

        [Header("Settings")]
        [SerializeField] private bool autoStart = false;
        [Tooltip("Higher the samples, the more frequent the update, but with experience, 2 or 3 give the best results for tuning a guitar")]
        [Range(2, 6)]
        [SerializeField] private int samplesPerSecond = 3;
        [SerializeField] private float minFreq = 60.0f;
        [SerializeField] private float maxFreq = 1000.0f;
        [SerializeField] private float minVolume = 0.5f;

        [Header("Outputs")]
        [SerializeField] private TMP_Text textFrequency;


        [Header("Inputs")]
        [SerializeField] private TMP_Dropdown microphoneDropdown;

        void Start()
        {
            analyser.Init(minFreq, maxFreq);

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
            textFrequency.enabled = true;
            analyser.StartMicrophoneAnalyser(samplesPerSecond);
        }

        public virtual void StopTuner()
        {
            analyser.StopMicrophoneAnalyser();
            textFrequency.enabled = false;
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
            if (analyse.sampleVolume < minVolume)
            {
                textFrequency.text = "";
            }
            else if (!analyse.fundamentalFrequencyOutOfBounds)
            {
                textFrequency.text = analyse.fundamentalFrequency.ToString("F2") + " Hz";
            }
            else
            {
                
            }
        }
    }
}
