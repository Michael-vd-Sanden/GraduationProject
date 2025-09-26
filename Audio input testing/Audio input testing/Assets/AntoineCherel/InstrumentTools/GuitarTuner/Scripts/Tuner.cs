using System.Collections.Generic;
using ACInstrumentTools.Core;
using TMPro;
using UnityEngine;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Base class for multiple tuners
    /// This acts as an interface for Tuners. It has the necessary component's references, like a FrequencyAnalyser, the frequency range
    ///
    /// Note that if this interface doesn't suit your needs, just create a new Script that references a FrequencyAnalyser, initializes it, and subscribes to onSampleAnalyzed
    /// </summary>
    public class Tuner : MonoBehaviour
    {

        [Header("Frequency Analyser")]
        [SerializeField] protected FrequencyAnalyser analyser;

        [Header("Settings")]
        [SerializeField] protected bool autoStart = false;
        [Tooltip("Higher the samples, the more frequent the update, but with experience, 2 or 3 give the best results for tuning a guitar")]
        [Range(2, 6)]
        [SerializeField] protected int samplesPerSecond = 3; 
        [SerializeField] protected float minFreq = 60.0f;
        [SerializeField] protected float maxFreq = 1000.0f;
        [SerializeField] protected float minVolume = 1.5f;
        
        [Header("Outputs")]
        [SerializeField] protected TMP_Text textFrequency;
        [SerializeField] protected TMP_Text textString;
        [SerializeField] protected TMP_Text textVolume;
        [SerializeField] protected TMP_Text textMinVolume;


        [Header("Inputs")]
        [SerializeField] protected TMP_Dropdown microphoneDropdown;

        // Filled by derived classes
        public Note[] standardTuning {get; protected set;}
        protected List<float> latestFreq = new List<float>();
        protected Note currentNote = null;

        public bool micIsSwitchedOn { get; protected set; }

        protected void InitTuner()
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

            if(autoStart)
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
            micIsSwitchedOn = true;
            textFrequency.enabled = true;
            textString.enabled = true;
            textVolume.enabled = true;
            textMinVolume.enabled = true;
            analyser.StartMicrophoneAnalyser(samplesPerSecond);
        }

        public virtual void StopTuner()
        {
            analyser.StopMicrophoneAnalyser();
            textFrequency.enabled = false;
            textString.enabled = false;
            textVolume.enabled = false;
            textMinVolume.enabled = false;
        }

        public void OnToggle()
        {
            if (analyser.recording)
                StopTuner();
            else
                StartTuner();
        }

        private void ResetCurrentNote()
        {
            textString.enabled = false;
            latestFreq = new List<float>();
            currentNote = null;
        }

        public void changeMinVolume(float vol)
        {
            minVolume = vol;
            textMinVolume.text = minVolume.ToString("#.00");
        }


        /// <summary>
        /// callback that recieves an event when an analyse is done. Will be called every (1 / samplesPerSecond) seconds.
        /// it will average the frequency with the last recieved, in an effort to smooth results
        /// </summary>
        /// <param name="analyse">the computed Analyse</param>
        protected virtual void OnSampleAnalysed(Analyse analyse)
        {
            if (analyse.sampleVolume < minVolume || analyse.fundamentalFrequencyOutOfBounds)
            {
                ResetCurrentNote();
                return;
            }

            textVolume.text = analyse.sampleVolume.ToString("#.00") + " db";

            float freq = analyse.fundamentalFrequency;

            if (latestFreq.Count == 0)
            {
                // Fetch the note
                currentNote = GetClosestNote(freq);

            }

            latestFreq.Add(freq);

            // We average the frequency from the last second
            if (latestFreq.Count == samplesPerSecond)
                latestFreq.RemoveAt(0);

            float mean = 0.0f;

            for (int i = 0; i < latestFreq.Count; i++)
            {
                mean += latestFreq[i];
            }

            float averagedFreq = mean / latestFreq.Count;


            // display the raw Freqency
            textFrequency.text = averagedFreq.ToString("F2") + " Hz";

            float dist = analyser.maxFreq;
            bool positive = true;

            if (currentNote != null)
            {
                dist = Mathf.Abs(currentNote.frequency - averagedFreq);
                positive = currentNote.frequency - freq < 0.0f;
            }

            textString.color = (dist < 1f) ? Color.green : Color.white;

            // If the distance is greater than 7 Hz, the user is probably stringing another string
            if (dist > 7.0f)
            {
                ResetCurrentNote();

                return;
            }

            string sign = (positive) ? "+" : "-";
            textString.enabled = true;
            textString.text = currentNote.name + " " + sign + dist.ToString("F1");
        }

        protected Note GetClosestNote(float freq)
        {
            Note closestNote = null;
            float minDistance = analyser.maxFreq;
            

            for (int i = 0; i < standardTuning.Length; i++)
            {

                float dist = Mathf.Abs(standardTuning[i].frequency - freq);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestNote = standardTuning[i];
                }
            }


            return closestNote;
        }
    }
}
