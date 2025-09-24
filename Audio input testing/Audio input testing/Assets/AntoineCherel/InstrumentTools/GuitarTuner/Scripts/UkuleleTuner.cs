using ACInstrumentTools.Core;


namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Tuner implementation for Ukulele that uses a list of last found frequencies, to smooth the result
    /// </summary>
    public class UkuleleTuner : Tuner
    {
        readonly Note[] ukuleleStandardTuning = {
            new Note("G", 392f),
            new Note("C", 261.63f),
            new Note("E", 329.63f),
            new Note("A", 440.0f ),
        };

        void Start()
        {
            standardTuning = ukuleleStandardTuning;
            InitTuner();
        }
    }
}
