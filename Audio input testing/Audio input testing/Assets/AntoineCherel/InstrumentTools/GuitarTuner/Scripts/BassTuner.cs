using ACInstrumentTools.Core;

namespace ACInstrumentTools.GuitarTuner
{
    /// <summary>
    /// Tuner implementation for Bass that uses a list of last found frequencies, to smooth the result
    /// </summary>
    public class BassTuner : Tuner
    {
        readonly Note[] bassStandardTuning = {
            new Note("E", 41.203f),
            new Note("A", 55.0f),
            new Note("D", 73.416f),
            new Note("G", 97.999f ),
        };


        void Start()
        {
            standardTuning = bassStandardTuning;
            InitTuner();
        }
    }
    
}
