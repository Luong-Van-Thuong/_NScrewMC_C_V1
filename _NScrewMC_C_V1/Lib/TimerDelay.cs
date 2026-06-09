using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansolHandScrew
{
 public class TimerDelay
    {
        protected DateTime TimeSart;
        public void SetTimer() {
            TimeSart = DateTime.Now;
        }
        public float GetTime {
            get {
                TimeSpan result = DateTime.Now - TimeSart;
                return (float)(result.TotalMilliseconds / 1000);
            }
        }
    }
}
