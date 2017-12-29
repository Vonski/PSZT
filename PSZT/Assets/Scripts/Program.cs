using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Zuzel
{
    public class Program : MonoBehaviour
    {
        
        MotionEngine me = new MotionEngine();
        public bool check = true;

        void Start()
        {
            me.SetParameters(0, 115, 0, -1, -1, 0, -0.5, -0.27);
        }

        void Update()
        {
            if (check)
            {
                check = me.Iterate();
                //Debug.Log("Time: " + Math.Round(me.GetTime(), 3) + "\tPosition: R=" + Math.Round(me.GetPoint().getR(), 2) + "\tDegrees=" + Math.Round(me.GetPoint().getDegrees(), 2));
                GetComponent<Transform>().position = new Vector2((float)me.GetPoint().getX()/10, (float)me.GetPoint().getY()/10);
            }
            //else
                //Debug.Log("Angle: " + me.GetPoint().getDegrees());
        }
    }
}
