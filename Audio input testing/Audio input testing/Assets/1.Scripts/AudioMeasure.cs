using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine.Profiling;

public class AudioMeasure : MonoBehaviour
{
    public float RmsValue;
    public float DbValue;
    public float PitchValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;
    private AudioSource audioSource;

    public TMP_Text calculatedDb;
    public TMP_Text calculatedFreq;

    //microphone
    /*
    private string _device;
    private AudioClip _clipRecord;
    private bool isMicOn;*/

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        AnalyzeSound();
        calculatedDb.text = DbValue.ToString();
        calculatedFreq.text = PitchValue.ToString();

        //microphone
        /*
        if(Input.GetKeyDown(KeyCode.P)) InitializeMic();
        if (Input.GetKeyDown(KeyCode.Q)) StopMic();

        AnalyzeMic();*/
    }

    //microphone
    /*
    private void InitializeMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);

        isMicOn= true;
    }

    private void StopMic()
    {
        Microphone.End(_device);
        isMicOn= false;
    }

    void AnalyzeMic()
    {
        if (isMicOn)
        {
            _clipRecord.GetData(_samples, 0); // fill array with samples
            int i;
            float sum = 0;
            for (i = 0; i < QSamples; i++)
            {
                sum += _samples[i] * _samples[i]; // sum squared samples*</em>
            }
            RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
            DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
            if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                                // get sound spectrum
            
            audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
            float maxV = 0;
            var maxN = 0;
            for (i = 0; i < QSamples; i++)
            { // find max
                if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                    continue;

                maxV = _spectrum[i];
                maxN = i; // maxN is the index of max
            }
            float freqN = maxN; // pass the index to a float variable
            if (maxN > 0 && maxN < QSamples - 1)
            { // interpolate index using neighbours
                var dL = _spectrum[maxN - 1] / _spectrum[maxN];
                var dR = _spectrum[maxN + 1] / _spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }
            PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
        }
    }   */

    //analyse
    void AnalyzeSound()
    {
        audioSource.GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples*</em>
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
        audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
    }

}