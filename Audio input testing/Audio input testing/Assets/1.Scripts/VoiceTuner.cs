using ACInstrumentTools.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Tuner implementation for voice that uses a list of last found frequencies, to smooth the result
    /// </summary>
    public class VoiceTuner : Tuner
    {
        [Header("Sharp Flat Control")]
        [SerializeField] private Toggle sharpToggle;
        [SerializeField] private Toggle flatToggle;

        #region sharpNotes
        readonly Note[] voiceStandardTuningSharp = {
            //Octave 0
            new Note("C0", 16.35f),
            new Note("C#0", 17.32f),
            new Note("D0", 18.35f),
            new Note("D#0", 19.45f),
            new Note("E0", 20.60f),
            new Note("F0", 21.93f),
            new Note("F#0", 23.12f),
            new Note("G0", 24.5f),
            new Note("G#0", 25.96f),
            new Note("A0", 27.50f),
            new Note("A#0", 29.14f),
            new Note("B0", 30.87f),

           //Octave 1
            new Note("C1", 32.70f),
            new Note("C#1", 34.65f),
            new Note("D1", 36.71f),
            new Note("D#1", 38.89f),
            new Note("E1", 41.20f),
            new Note("F1", 43.65f),
            new Note("F#1", 46.25f),
            new Note("G1", 49f),
            new Note("G#1", 51.91f),
            new Note("A1", 55f),
            new Note("A#1", 58.27f),
            new Note("B1", 61.74f),

            //Octave 2
            new Note("C2", 65.41f),
            new Note("C#2", 69.3f),
            new Note("D2", 73.42f),
            new Note("D#2", 77.78f),
            new Note("E2", 82.41f),
            new Note("F2", 87.31f),
            new Note("F#2", 92.50f),
            new Note("G2", 98f),
            new Note("G#2", 103.83f),
            new Note("A2", 110f),
            new Note("A#2", 116.54f),
            new Note("B2", 123.47f),

            //Octave 3
            new Note("C3", 130.81f),
            new Note("C#3", 138.59f),
            new Note("D3", 146.83f),
            new Note("D#3", 155.56f),
            new Note("E3", 164.81f),
            new Note("F3", 174.61f),
            new Note("F#3", 185f),
            new Note("G3", 196f),
            new Note("G#3", 207.65f),
            new Note("A3", 220f),
            new Note("A#3", 233.08f),
            new Note("B3", 246.94f),

            //Octave 4
            new Note("C4", 264.63f),
            new Note("C#4", 277.18f),
            new Note("D4", 293.66f),
            new Note("D#4", 311.13f),
            new Note("E4", 329.63f),
            new Note("F4", 349.23f),
            new Note("F#4", 369.99f),
            new Note("G4", 392f),
            new Note("G#4", 415.3f),
            new Note("A4", 440f),
            new Note("A#4", 466.16f),
            new Note("B4", 493.88f),

            //Octave 5
            new Note("C5", 523.25f),
            new Note("C#5", 554.37f),
            new Note("D5", 587.33f),
            new Note("D#5", 622.25f),
            new Note("E5", 659.25f),
            new Note("F5", 698.46f),
            new Note("F#5", 739.99f),
            new Note("G5", 783.99f),
            new Note("G#5", 830.61f),
            new Note("A5", 880f),
            new Note("A#5", 932.33f),
            new Note("B5", 987.77f),

            //Octave 6
            new Note("C6", 1046.5f),
            new Note("C#6", 1108.73f),
            new Note("D6", 1174.66f),
            new Note("D#6", 1244.51f),
            new Note("E6", 1318.51f),
            new Note("F6", 1396.91f),
            new Note("F#6", 1479.98f),
            new Note("G6", 1567.98f),
            new Note("G#6", 1661.22f),
            new Note("A6", 1760f),
            new Note("A#6", 1864.66f),
            new Note("B6", 1975.53f),

            //Octave 7
            new Note("C7", 2093f),
            new Note("C#7", 2217.46f),
            new Note("D7", 2349.32f),
            new Note("D#7", 2489.02f),
            new Note("E7", 2637.02f),
            new Note("F7", 2793.83f),
            new Note("F#7", 2959.96f),
            new Note("G7", 3135.96f),
            new Note("G#7", 3322.44f),
            new Note("A7", 3520f),
            new Note("A#7", 3729.31f),
            new Note("B7", 3951.07f),

            //Octave 8
            new Note("C8", 4186.01f),
            new Note("C#8", 4434.92f),
            new Note("D8", 4698.63f),
            new Note("D#8", 4978.03f),
            new Note("E8", 5274.04f),
            new Note("F8", 5587.65f),
            new Note("F#8", 5919.91f),
            new Note("G8", 6271.93f),
            new Note("G#8", 6644.88f),
            new Note("A8", 7040f),
            new Note("A#8", 7458.62f),
            new Note("B8", 7902.13f),

        };
        #endregion

        #region flatNotes
        readonly Note[] voiceStandardTuningFlat = {
            //Octave 0
            new Note("C0", 16.35f),
            new Note("Db0", 17.32f),
            new Note("D0", 18.35f),
            new Note("Eb0", 19.45f),
            new Note("E0", 20.60f),
            new Note("F0", 21.93f),
            new Note("Gb0", 23.12f),
            new Note("G0", 24.5f),
            new Note("Ab0", 25.96f),
            new Note("A0", 27.50f),
            new Note("Bb0", 29.14f),
            new Note("B0", 30.87f),

           //Octave 1
            new Note("C1", 32.70f),
            new Note("Db1", 34.65f),
            new Note("D1", 36.71f),
            new Note("Eb1", 38.89f),
            new Note("E1", 41.20f),
            new Note("F1", 43.65f),
            new Note("Gb1", 46.25f),
            new Note("G1", 49f),
            new Note("Ab1", 51.91f),
            new Note("A1", 55f),
            new Note("Bb1", 58.27f),
            new Note("B1", 61.74f),

            //Octave 2
            new Note("C2", 65.41f),
            new Note("Db2", 69.3f),
            new Note("D2", 73.42f),
            new Note("Eb2", 77.78f),
            new Note("E2", 82.41f),
            new Note("F2", 87.31f),
            new Note("Gb2", 92.50f),
            new Note("G2", 98f),
            new Note("Ab2", 103.83f),
            new Note("A2", 110f),
            new Note("Bb2", 116.54f),
            new Note("B2", 123.47f),

            //Octave 3
            new Note("C3", 130.81f),
            new Note("Db3", 138.59f),
            new Note("D3", 146.83f),
            new Note("Eb3", 155.56f),
            new Note("E3", 164.81f),
            new Note("F3", 174.61f),
            new Note("Gb3", 185f),
            new Note("G3", 196f),
            new Note("Ab3", 207.65f),
            new Note("A3", 220f),
            new Note("Bb3", 233.08f),
            new Note("B3", 246.94f),

            //Octave 4
            new Note("C4", 264.63f),
            new Note("Db4", 277.18f),
            new Note("D4", 293.66f),
            new Note("Eb4", 311.13f),
            new Note("E4", 329.63f),
            new Note("F4", 349.23f),
            new Note("Gb4", 369.99f),
            new Note("G4", 392f),
            new Note("Ab4", 415.3f),
            new Note("A4", 440f),
            new Note("Bb4", 466.16f),
            new Note("B4", 493.88f),

            //Octave 5
            new Note("C5", 523.25f),
            new Note("Db5", 554.37f),
            new Note("D5", 587.33f),
            new Note("Eb5", 622.25f),
            new Note("E5", 659.25f),
            new Note("F5", 698.46f),
            new Note("Gb5", 739.99f),
            new Note("G5", 783.99f),
            new Note("Ab5", 830.61f),
            new Note("A5", 880f),
            new Note("Bb5", 932.33f),
            new Note("B5", 987.77f),

            //Octave 6
            new Note("C6", 1046.5f),
            new Note("Db6", 1108.73f),
            new Note("D6", 1174.66f),
            new Note("Eb6", 1244.51f),
            new Note("E6", 1318.51f),
            new Note("F6", 1396.91f),
            new Note("Gb6", 1479.98f),
            new Note("G6", 1567.98f),
            new Note("Ab6", 1661.22f),
            new Note("A6", 1760f),
            new Note("Bb6", 1864.66f),
            new Note("B6", 1975.53f),

            //Octave 7
            new Note("C7", 2093f),
            new Note("Db7", 2217.46f),
            new Note("D7", 2349.32f),
            new Note("Eb7", 2489.02f),
            new Note("E7", 2637.02f),
            new Note("F7", 2793.83f),
            new Note("Gb7", 2959.96f),
            new Note("G7", 3135.96f),
            new Note("Ab7", 3322.44f),
            new Note("A7", 3520f),
            new Note("Bb7", 3729.31f),
            new Note("B7", 3951.07f),

            //Octave 8
            new Note("C8", 4186.01f),
            new Note("Db8", 4434.92f),
            new Note("D8", 4698.63f),
            new Note("Eb8", 4978.03f),
            new Note("E8", 5274.04f),
            new Note("F8", 5587.65f),
            new Note("Gb8", 5919.91f),
            new Note("G8", 6271.93f),
            new Note("Ab8", 6644.88f),
            new Note("A8", 7040f),
            new Note("Bb8", 7458.62f),
            new Note("B8", 7902.13f),

        };
        #endregion

        void Start()
        {
            if (sharpToggle.isOn)
            { standardTuning = voiceStandardTuningSharp; }
            else if (flatToggle.isOn)
            { standardTuning = voiceStandardTuningFlat; }
            InitTuner();
        }

        private void Update()
        {
            if(micIsSwitchedOn)
            {
                

                micIsSwitchedOn = false;
            }
        }

        public void changetuning()
        {
            if (sharpToggle.isOn)
            { standardTuning = voiceStandardTuningSharp; }
            else if (flatToggle.isOn)
            { standardTuning = voiceStandardTuningFlat; }

            
        }
    }
}