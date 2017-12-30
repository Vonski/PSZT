using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rider
{
    class Program : MonoBehaviour
    {
        MotionEngine me;
        OnePlusOne opo;
        bool ready;

        void Start()
        {
            ready = true;
            me = new MotionEngine();
            me.SetParameters(new double[8] { 0, 115, 0, -1.5, -1, 0, -0.5, -0.27 });
            opo = new OnePlusOne(10, 0.82, 1.2, 1.0);
            opo.SetProblem(me);
        }

        public void Reset()
        {
            Debug.Log("no1");
            ready = true;
            me.SetParameters(new double[8] { 0, 115, 0, -1.5, -1, 0, -0.5, -0.27 });
            me.Reset();
            opo = new OnePlusOne(10, 0.82, 1.2, 1.0);
            opo.SetProblem(me);
        }

        void Update()
        {
            if (ready)
            {
                me.SetParameters(opo.Iterate(100));
                me.Reset();
                ready = false;
            }
            else
            {
                if (me.Iterate())
                    GetComponent<Transform>().position = new Vector2((float)me.GetPoint().getX() / 10, (float)me.GetPoint().getY() / 10);
                else
                    ready = true;
            }
        }

        //static void Main(string[] args)
        //{
        //    IProblem me = new MotionEngine();
        //    double[] p = { 0, 115, 0, -1.5, -1, 0, -0.5, -0.27 };
        //    me.SetParameters(p);

        //    IAlgorithm opo = new OnePlusOne(10, 0.82, 1.2, 1.0);
        //    opo.SetProblem(me);
        //    opo.Optimize();

        //}
    }
}
