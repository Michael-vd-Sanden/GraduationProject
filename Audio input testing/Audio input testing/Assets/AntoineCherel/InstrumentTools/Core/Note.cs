using System;
using System.Collections.Generic;
using System.Linq;

namespace ACInstrumentTools.Core
{
    /// <summary>
    /// a Note
    /// </summary>
    public class Note
    {
        public string name;
        public float frequency;

        /// <summary>
        /// a Note's constructor
        /// </summary>
        /// <param name="name">the note's name</param>
        /// <param name="frequency">the note's frequency</param>
        public Note(float frequency)
        {
            this.name = "";
            this.frequency = frequency;
        }

        /// <summary>
        /// a Note's constructor
        /// </summary>
        /// <param name="name">the note's name</param>
        /// <param name="frequency">the note's frequency</param>
        public Note(string name, float frequency)
        {
            this.name = name;
            this.frequency = frequency;
        }


        public Peak GetClosestPeak(Peak[] peaks)
        {
            return new List<Peak>(peaks).OrderBy(item => Math.Abs(this.frequency - item.frequency)).First();
        }
    }
}