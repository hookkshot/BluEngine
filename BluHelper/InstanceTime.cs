// InstanceTime
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

        private long last;

        private long frameTime = 33;
        private int target = 30;

        public event EventHandler Frame;

        private Thread timer;

        /// <summary>
        /// Returns the elapsed amount of time since Elapsed was used.
        /// </summary>
        public long Elapsed
        {
            get
            {
                long elapsed = time - last;
                last = time;
                return elapsed;
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

        public InstanceTime()
        {
            time = DateTime.Now.Ticks;
            frameTime = 1000 / 30;
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
                else
                {
                    Thread.Sleep((int)(frameTime - (time - elapsedLast)));
                }
            }
        }

        public override string ToString()
        {
            return time.ToString();
        }

        
    }
}
