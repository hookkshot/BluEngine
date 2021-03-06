﻿// InstanceTime
// Author: Keirron Stach
// Date: 22/09/2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BluHelper
{
    /// <summary>
    /// Keeps track of system time and can target clock rates based on values set.
    /// </summary>
    public class InstanceTime
    {
        private long time;
        private long elapsedLast;
        private long elapsedTime;

        private long last;

        private long frameTime = 33;
        private int target = 30;

        public event EventHandler Frame;

        private Thread timer;

        /// <summary>
        /// Returns the elapsed amount of time since Elapsed was used (In Milliseconds).
        /// </summary>
        public long Elapsed
        {
            get
            {
                return elapsedTime;
            }
        }

        /// <summary>
        /// The target clock rate of the timer.
        /// </summary>
        public int Target
        {
            set
            {
                target = value;
                frameTime = 1000 / target;
            }
            get { return target; }
        }

        /// <summary>
        /// Get the total time this program has been running.
        /// </summary>
        public long TotalTime
        {
            get { return time/10000; }
        }

        /// <summary>
        /// Returnt he total number of ticks.
        /// </summary>
        public long Ticks
        {
            get { return time; }
        }

        public InstanceTime()
        {
            time = DateTime.Now.Ticks;
            frameTime = InstanceTime.FromSeconds(1) / 30;
        }

        /// <summary>
        /// Starts the timer which will trigger the Frame even as often as Target is set too per second.
        /// </summary>
        public void Run()
        {
            if (Frame != null)
            {
                timer = new Thread(new ThreadStart(RunThread));
                timer.IsBackground = true;
                timer.Start();
            }
        }

        private void RunThread()
        {
            while (true)
            {
                time = DateTime.Now.Ticks;

                if (time - elapsedLast > frameTime)
                {
                    Frame(this, new EventArgs());
                    elapsedLast = time;
                }
            }
        }

        public override string ToString()
        {
            return  this.TotalTime.ToString();
        }

        public void Update()
        {
            long elapsed = TotalTime - last;
            last = TotalTime;
        }

        #region AutoFormats

        public static long FromSeconds(int v)
        {
            return v * 10000000 ;
        }

        public static long FromMinutes(int v)
        {
            return v * FromSeconds(60);
        }

        public static long FromHours(int v)
        {
            return v * FromMinutes(60);
        }

        public static long FromDays(int v)
        {
            return v * FromHours(24);
        }

        public static long Zero
        {
            get { return 0; }
        }

        public static string ShowTime()
        {
            return "";
        }

        #endregion
    }
}
