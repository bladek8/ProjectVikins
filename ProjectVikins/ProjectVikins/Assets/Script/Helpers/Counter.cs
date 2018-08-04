using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Helpers
{
    public class Counter
    {
        public float Time { get; set; }
        public float MaxTime { get; set; }
        public bool Counting { get; set; }

        public Counter()
        {
            MaxTime = 1;
            Time = 0;
        }
        public Counter(float maxTime)
        {
            MaxTime = maxTime;
            Time = 0;
        }

        public void ResetCounter()
        {
            Time = 0;
        }

        public static void Count(Counter data)
        {
            if (data.Time < data.MaxTime)
                data.Time += UnityEngine.Time.deltaTime;
            else if (data.Time > data.MaxTime)
                data.Time = data.MaxTime;
        }

    }
}
