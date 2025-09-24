using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ACInstrumentTools.Core
{
    /// <summary>
    /// Helper class for audio sample analysis
    /// </summary>
    public static class FrequencyUtils
    {
        /// <summary>
        /// FindFundamentalFrequency 
        /// </summary>
        /// <param name="samples">the samples to be analyzed</param>
        /// <param name="sampleRate">the sample rate, in Hz</param>
        /// <param name="minFreq">the minimum frequency, in Hz</param>
        /// <param name="maxFreq">the maximum frequency, in Hz</param>
        /// <returns>the fundamental frequency, in Hz, within the samples or 0</returns>
        internal static float FindFundamentalFrequency(double[] samples, int sampleRate, double minFreq, double maxFreq)
        {
            Peak[] peaks = FindAndSortPeaks(samples, sampleRate, minFreq, maxFreq);
            peaks = CleanPeaks(peaks);

            if (peaks.Length > 0)
            {
                return FindFundamentalFrequency(peaks);
            }

            return 0;
        }

        /// <summary>
        /// FindAndSortPeaks 
        /// </summary>
        /// <param name="samples">the samples to be analyzed</param>
        /// <param name="sampleRate">the sample rate, in Hz</param>
        /// <param name="minFreq">the minimum frequency, in Hz</param>
        /// <param name="maxFreq">the maximum frequency, in Hz</param>
        /// <returns>an array of Peaks, ordered by height</returns>
        internal static Peak[] FindAndSortPeaks(double[] samples, int sampleRate, double minFreq, double maxFreq, int peaksCount = 5)
        {
            double[] spectr = FftAlgorithm.Calculate(samples);

            int usefullMinSpectr = Math.Max(0,
                (int)(minFreq * spectr.Length / sampleRate));
            int usefullMaxSpectr = Math.Min(spectr.Length,
                (int)(maxFreq * spectr.Length / sampleRate) + 1);

            // find peaks in the FFT frequency bins 
            int[] peakIndices = FindPeaks(spectr, usefullMinSpectr, usefullMaxSpectr - usefullMinSpectr, peaksCount);

            if (Array.IndexOf(peakIndices, usefullMinSpectr) >= 0)
            {
                // lowest usefull frequency bin shows active
                // looks like is no detectable sound, return an empty array of peaks
                return new Peak[0];
            }

            // select fragment to check peak values: data offset
            const int verifyFragmentOffset = 0;
            int verifyFragmentLength = (int)(sampleRate / minFreq);

            
            List<Peak> peaks = new List<Peak>();

            for (int i = 0; i < peakIndices.Length; i++)
            {
                int index = peakIndices[i];
                int binIntervalStart = spectr.Length / (index + 1);
                int binIntervalEnd = spectr.Length / index;
                int interval;
                double peakValue;
                float peakFreq;
                // scan bins frequencies/intervals
                ScanSignalIntervals(samples, verifyFragmentOffset, verifyFragmentLength,
                    binIntervalStart, binIntervalEnd, out interval, out peakValue);

                peakFreq = (float)sampleRate / interval;

                peaks.Add(new Peak(peakValue, peakFreq));
            }

            peaks.Sort();

            return peaks.ToArray();
        }



        private static void ScanSignalIntervals(double[] samples, int index, int length,
            int intervalMin, int intervalMax, out int optimalInterval, out double optimalValue)
        {
            optimalValue = Double.PositiveInfinity;
            optimalInterval = 0;

            // distance between min and max range value can be big
            // limiting it to the fixed value
            const int MaxAmountOfSteps = 30;
            int steps = intervalMax - intervalMin;
            if (steps > MaxAmountOfSteps)
                steps = MaxAmountOfSteps;
            else if (steps <= 0)
                steps = 1;

            // trying all intervals in the range to find one with
            // smaller difference in signal waves
            for (int i = 0; i < steps; i++)
            {
                int interval = intervalMin + (intervalMax - intervalMin) * i / steps;

                double sum = 0;
                for (int j = 0; j < length; j++)
                {
                    double diff = samples[index + j] - samples[index + j + interval];
                    sum += diff * diff;
                }
                if (optimalValue > sum)
                {
                    optimalValue = sum;
                    optimalInterval = interval;
                }
            }
        }

        private static int[] FindPeaks(double[] values, int index, int length, int peaksCount)
        {
            double[] peakValues = new double[peaksCount];
            int[] peakIndices = new int[peaksCount];

            for (int i = 0; i < peaksCount; i++)
            {
                peakValues[i] = values[peakIndices[i] = i + index];
            }

            // find min peaked value
            double minStoredPeak = peakValues[0];
            int minIndex = 0;
            for (int i = 1; i < peaksCount; i++)
            {
                if (minStoredPeak > peakValues[i]) minStoredPeak = peakValues[minIndex = i];
            }

            for (int i = peaksCount; i < length; i++)
            {
                if (minStoredPeak < values[i + index])
                {
                    // replace the min peaked value with bigger one
                    peakValues[minIndex] = values[peakIndices[minIndex] = i + index];

                    // and find min peaked value again
                    minStoredPeak = peakValues[minIndex = 0];
                    for (int j = 1; j < peaksCount; j++)
                    {
                        if (minStoredPeak > peakValues[j]) minStoredPeak = peakValues[minIndex = j];
                    }
                }
            }

            return peakIndices;
        }

        /// <summary>
        /// Remove duplicate peaks from an array of Peaks
        /// </summary>
        /// <param name="peaks"></param>
        /// <param name="minimumFrequencyGap"></param>
        /// <returns></returns>
        public static Peak[] CleanPeaks(Peak[] peaks, float minimumFrequencyGap = 2)
        {
            // if the "best" peak, has a value above 1, it's probably just noise and no chord is playing
            if (peaks == null || peaks.Length == 0 || peaks[0].value > 1)
            {
                return new Peak[0];
            }

            List<Peak> cleanedPeaks = CleanPeaksTooCloseAndOfBadValue(peaks, minimumFrequencyGap);

            if (cleanedPeaks.Count > 0)
            {
                return cleanedPeaks.ToArray();
            }

            // We couldn't get anything from this sample...
            return new Peak[0];
        }


        internal static List<Peak> CleanPeaksTooCloseAndOfBadValue(Peak[] peaks, float minimumFrequencyGap)
        {
            List<Peak> cleanedUpPeaks = new List<Peak>();
            bool closePeakWasChoosen = false;
            List<Peak> basePeaks = peaks.OrderBy(p => p.frequency).ToList();

            for (int i = 0; i < basePeaks.Count; i++)
            {
                if (i < basePeaks.Count - 1)
                {
                    Peak p1 = basePeaks[i];
                    Peak p2 = basePeaks[i + 1];

                    // if the two peaks are close
                    if (ArePeaksClose(p1, p2, minimumFrequencyGap) && !closePeakWasChoosen)
                    {
                        if (p1.value <= 1)
                        {
                            cleanedUpPeaks.Add(p1);
                            closePeakWasChoosen = true;
                        }
                        closePeakWasChoosen = false;
                    }
                    else
                    {
                        // peaks are far away from each other
                        if (p1.value <= 1)
                        {
                            cleanedUpPeaks.Add(p1);
                        }
                        closePeakWasChoosen = false;
                    }
                }
                else
                {
                    //last peak
                    if (cleanedUpPeaks.Count > 0)
                    {
                        Peak p1 = basePeaks[i];
                        Peak p2 = cleanedUpPeaks.Last();

                        if (ArePeaksClose(p1, p2, minimumFrequencyGap) && !closePeakWasChoosen)
                        {
                            cleanedUpPeaks.Add(p1);
                        }
                    }
                }

            }


            return cleanedUpPeaks;
        }


        private static bool ArePeaksClose(Peak p1, Peak p2, float minimumFrequencyGap)
        {
            if (Mathf.Abs(p1.frequency - p2.frequency) < minimumFrequencyGap)
            {
                return true;
            }
            return false;
        }

        internal static float FindFundamentalFrequency(Peak[] peaks)
        {
            if (peaks.Length == 0)
                return 0.0f;

            if (peaks.Length == 1)
                return peaks[0].frequency;

            float fundamentalFrequency = peaks.OrderBy(i => i.frequency).First().frequency;
            return fundamentalFrequency;
        }
    }
}
