using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ACInstrumentTools.Core
{
    /// <summary>
    /// Core class used to analyse frequencies from an AudioClip or the Microphone's input
    /// </summary>
    public class FrequencyAnalyser : MonoBehaviour
    {
    
        [Header("Callbacks")]
        public UnityEvent<Analyse> onSampleAnalysed;

        protected int microphoneIndex = 0;
        public bool initialized {private set; get;}
        public bool recording {private set; get;}
        public float minFreq {private set; get;}
        public float maxFreq {private set; get;}
        public int samplesPerSec {private set; get;}
        public int peakPerAnalye { private set; get; }
        public readonly int samplingRate = 88200;

        private float[] samplesBuffer;
        private int halfedSampleSize;

        public void Init(float minFrequency = 60.0f, float maxFrequency = 1000.0f)
        {
            minFreq = minFrequency;
            maxFreq = maxFrequency;
            initialized = true;
        }

        #region Start & Stop
        public void StartMicrophoneAnalyser(int samplesPerSecond = 4, int maxPeakCount = 10)
        {
            if (!initialized)
            {
                Debug.LogWarning("[ACInstrumentTools.Core] FrequencyAnalyser not initialised");
                return;
            }
            samplesPerSec = samplesPerSecond;
            peakPerAnalye = maxPeakCount;
            halfedSampleSize = (samplingRate / samplesPerSec) / 2;
            samplesBuffer = new float[halfedSampleSize];
            recording = true;
            StartCoroutine(RecordCoroutine());
        }

        public void StopMicrophoneAnalyser()
        {
            StopAllCoroutines();
            recording = false;
        }
        #endregion

        #region Setters & Getters

        public void SetMicrophoneIndex(int micIndex)
        {
            microphoneIndex = micIndex;

            if (recording)
            {
                StopMicrophoneAnalyser();
                StartMicrophoneAnalyser();
            }
        }

        public int GetMicrophoneIndex()
        {
            return microphoneIndex;
        }

        public void SetMaxFrequency(float maxFrequency)
        {
            maxFreq = maxFrequency;
        }

        public void SetMinFrequency(float maxFrequency)
        {
            maxFreq = maxFrequency;
        }


        #endregion
       
    
        #region Analyse

       public IEnumerator RecordCoroutine()
       {
            string deviceChoosen = Microphone.devices[microphoneIndex];

            Debug.Log("[ACInstrumentTools.Core] Recording on " + deviceChoosen);

            //testing
            AudioSource a = GetComponent<AudioSource>();

            while (recording)
            {
                AudioClip c = Microphone.Start(deviceChoosen, false, 1, samplingRate);
                //testing
                a.clip = c;
                a.Play();
                if(a.isPlaying) { Debug.Log("I'm playing!"); }
                for (int i = 0; i < samplesPerSec; i++)
                {
                    yield return new WaitForSecondsRealtime(1.0f/samplesPerSec);

                    int offsetSamples = samplingRate * i / samplesPerSec;

                    Analyse analyse = AnalyseClip(c, offsetSamples, peakPerAnalye); 

                    onSampleAnalysed.Invoke(analyse);
                }
            }

            yield return null;
        }

        /// <summary>
        /// Analyses an AudioClip
        /// </summary>
        /// <param name="clip">the source AudioClip</param>
        /// <param name="offsetSamples">amount of samples that should be offsetted during the analyse (optional)</param>
        /// <returns>an Analyse object with peak and volume data</returns>
        public Analyse AnalyseClip(AudioClip clip, int offsetSamples = 0, int maxPeakCount = 10)
        {
            Peak[] frequencies = GetClipPeakFrequencies(clip, offsetSamples, maxPeakCount);
            float vol = GetClipVolume(clip, offsetSamples);

            if (frequencies.Length == 0)
            {
                return new Analyse(0, new Peak[0], vol, fundamentalFrequencyOutOfBounds: false);
            }
            else
            {
                Peak[] cleanedPeaks = FrequencyUtils.CleanPeaks(frequencies);
                float peakFrequency = FrequencyUtils.FindFundamentalFrequency(cleanedPeaks);

                return new Analyse(peakFrequency, cleanedPeaks, vol, fundamentalFrequencyOutOfBounds: (minFreq > peakFrequency || peakFrequency > maxFreq));
            }
        }

        /// <summary>
        /// GetClipFrequency
        /// </summary>
        /// <param name="clip">the source AudioClip</param>
        /// <param name="offsetSamples">amount of samples that should be offsetted during the analyse (optional)</param>
        /// <returns>returns the peak frequency of an AudioClip</returns>
        public float GetClipFrequency(AudioClip clip, int offsetSamples = 0)
        {
            if (clip == null)
            {
                Debug.LogWarning("[ACInstrumentTools.Core] AudioClip is null");
                return 0.0f;
            }

            int sampleRate = clip.samples * clip.channels;

            float[] samples = new float[sampleRate - offsetSamples];

            clip.GetData(samples, offsetSamples);

            double[] doubleSamples = Array.ConvertAll(samples, x => (double)x);

            float frequency = FrequencyUtils.FindFundamentalFrequency(doubleSamples, sampleRate, minFreq, maxFreq);

            if (frequency > minFreq && frequency < maxFreq)
                return frequency;
            else
                return 0.0f;
        }

        /// <summary>
        /// GetClipPeakFrequencies
        /// </summary>
        /// <param name="clip">the source AudioClip</param>
        /// <param name="offsetSamples">amount of samples that should be offsetted during the analyse (optional)</param>
        /// <returns>returns an ordered list of Peak (containing frequencies) of an AudioClip</returns>
        private Peak[] GetClipPeakFrequencies(AudioClip clip, int offsetSamples = 0, int maxPeakCount = 5)
        {
            if (clip == null)
            {
                Debug.LogWarning("[ACInstrumentTools.Core] AudioClip is null");
                return new Peak[0];
            }

            int sampleRate = clip.samples * clip.channels;
            float[] samples = new float[sampleRate - offsetSamples];

            clip.GetData(samples, offsetSamples);

            double[] doubleSamples = Array.ConvertAll(samples, x => (double)x);

            return FrequencyUtils.FindAndSortPeaks(doubleSamples, sampleRate, minFreq, maxFreq, maxPeakCount);            
        }

        /// <summary>
        /// GetClipVolume
        /// </summary>
        /// <param name="clip">the source AudioClip</param>
        /// <param name="offsetSamples">amount of samples that should be ignored during the analyse (optional)</param>
        /// <returns>a value between 0 and 1000</returns>
        public float GetClipVolume(AudioClip clip, int offsetSamples = 0)
        {
            Array.Clear(samplesBuffer, 0, halfedSampleSize);
            clip.GetData(samplesBuffer, offsetSamples + (halfedSampleSize / 2));

            // Root mean square algorithm for frequencies in Hz, based on Parseval's theorem
            float volume = 0f;
			foreach (var sample in samplesBuffer) {
                float value = Mathf.Abs(sample);
                volume += value * value;
			}
			volume /= (halfedSampleSize);
            volume = Mathf.Sqrt(volume);

            // Hotfix for the issue with GetData sample being 1/3 less complete than when passing an offset
            if (offsetSamples == 0)
            {
                volume *= 1.5f;
            }

            return volume * 1000;
        }

        #endregion
    }
}