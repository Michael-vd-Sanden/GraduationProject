using ACInstrumentTools.Core;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Tuner implementation for Guitar that uses a list of last found frequencies, to smooth the result
    /// </summary>
    public class GuitarTuner : Tuner
    {
        readonly Note[] guitarStandardTuning = {
            new Note("E", 82.41f),
            new Note("A", 110.0f),
            new Note("D", 146.8f),
            new Note("G", 196.0f),
            new Note("B", 246.9f),
            new Note("e",329.6f)
        };

        void Start()
        {
            standardTuning = guitarStandardTuning;
            InitTuner();
        }
    }
}