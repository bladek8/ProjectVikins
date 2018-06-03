using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    public class CountDown
    {
        public double Rate { get; set; }
        public double CoolDown { get; set; }

        public CountDown()
        {
            CoolDown = 0;
            Rate = 1;
        }

        public CountDown(double rate)
        {
            Rate = rate;
            CoolDown = 0;
        }

        public static void DecreaseTime(CountDown model)
        {
            if (model.CoolDown > 0)
                model.CoolDown -= Time.deltaTime;
        }
    }
}
