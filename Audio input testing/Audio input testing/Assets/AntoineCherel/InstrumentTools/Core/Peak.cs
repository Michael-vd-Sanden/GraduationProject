using System;

namespace ACInstrumentTools.Core
{
    /// <summary>
    /// a Peak within a sound sample
    /// </summary>
    public struct Peak : IComparable<Peak>
    {
        public double value { private set; get; }
        public float frequency { private set; get; }

        /// <summary>
        /// Peak Contstructor
        /// </summary>
        /// <param name="value">the height of this peak</param>
        /// <param name="frequency">the frequency, in Hz</param>
        public Peak(double value, float frequency)
        {
            this.value = value;
            this.frequency = frequency;
        }

        public int CompareTo(Peak comparePeak)
        {
            return this.value.CompareTo(comparePeak.value);
        }

        public static float[] GetFrequencies(Peak[] peaks)
        {
            float[] frequencies = new float[peaks.Length];

            for (int i = 0; i < peaks.Length; i++)
            {
                frequencies[i] = peaks[i].frequency;
            }

            return frequencies;
        }
    }
}