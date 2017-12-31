using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Rider
{
    class Program : MonoBehaviour
    {
        GameObject controller;
        MotionEngine me;
        public bool ready, calc_time, inc;
        Slider slider;
        public int index;
        double delta, default_dT, end_time;

        public void ChangeDelta()
        {
            delta = default_dT * slider.value;
        }

        void OnEnable()
        {
            slider = GameObject.Find("Slider").GetComponent<Slider>();
            me = new MotionEngine();
            default_dT = delta = me.dT;
            controller = GameObject.Find("Controller");
            ready = calc_time = inc = false;
            Controller.changeEvent += SetReady;
        }

        void SetReady()
        {
            ready = true;
            calc_time = true;
            inc = true;
        }

        void Update()
        {
            if (ready)
            {
                me.SetParameters(controller.GetComponent<Controller>().opo.GetParametersOfProblem(index));
                me.Reset();
                ready = false;
            }
            else if (calc_time)
            {
                me.dT = default_dT;
                while (me.Iterate())
                    end_time = me.GetTime();
                //pos = new Vector2((float)me.GetPoint().getX() / 10, (float)me.GetPoint().getY() / 10);
                me.Reset();
                calc_time = false;
            }
            else
            {
                if (me.GetTime() < end_time)
                {
                    me.dT = delta;
                    me.Iterate();
                    GetComponent<Transform>().position = new Vector2((float)me.GetPoint().getX() / 10, (float)me.GetPoint().getY() / 10);
                }
                else if (inc)
                {
                    //GetComponent<Transform>().position = pos;
                    controller.GetComponent<Controller>().ready_lineages++;
                    inc = false;
                    me.dT = default_dT;
                }
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
