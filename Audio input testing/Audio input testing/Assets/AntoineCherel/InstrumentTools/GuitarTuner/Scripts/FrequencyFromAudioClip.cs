using System.Collections;
using System.Collections.Generic;
using ACInstrumentTools.Core;
using UnityEngine;
using TMPro;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Easily get the peak frequency of an audio file, with GetClipFrequency
    /// </summary>
    public class FrequencyFromAudioClip : MonoBehaviour
    {
        [Header("Frequency Analyser")]
        [SerializeField] private FrequencyAnalyser analyser;

        [Header("Settings")]
        [SerializeField] private AudioClip audioClip;

        [SerializeField] private TMP_Text fileText;
        [SerializeField] private TMP_Text frequencyText;

        
        void Start()
        {
            analyser.Init();
            Analyse analyse = analyser.AnalyseClip(audioClip);
            fileText.text = "File " + audioClip.name + " has a fundamental frequency of";
            frequencyText.text = analyse.fundamentalFrequency.ToString("F2") + " Hz";
        }

        
    }
}