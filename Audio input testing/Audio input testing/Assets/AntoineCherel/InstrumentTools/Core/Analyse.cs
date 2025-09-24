namespace ACInstrumentTools.Core
{
    /// <summary>
    /// Result of an analyse by the FrequencyAnalyser class
    /// </summary>
    public struct Analyse
    {
        public bool fundamentalFrequencyOutOfBounds { private set; get; }
        public float fundamentalFrequency { private set; get; }
        public Peak[] peaks { private set; get;}
        public float sampleVolume {private set; get;}

        /// <summary>
        /// Result of an analyse by the FrequencyAnalyser class
        /// </summary>
        /// <param name="fundamentalFrequency">the highest peak's frequency, in Hertz</param>
        /// <param name="peaks">the Frequencies in Hz of the found peaks, ordered by height</param>
        /// <param name="sampleVolume">the general volume of the sample (not the peak)</param>
        /// <param name="peakOutOfBounds">is the peaks are all out of bounds (if true the peaks are empty)</param>
        public Analyse(float fundamentalFrequency, Peak[] peaks, float sampleVolume, bool fundamentalFrequencyOutOfBounds = false)
        {
            this.fundamentalFrequency = fundamentalFrequency;
            this.peaks = peaks;
            this.sampleVolume = sampleVolume;
            this.fundamentalFrequencyOutOfBounds = fundamentalFrequencyOutOfBounds;
        }
    }
}