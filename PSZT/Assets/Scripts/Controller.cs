﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Rider
{
    class Controller : MonoBehaviour
    {

        public OnePlusOneParallel opo;
        public Transform prefab;
        Slider slider;
        System.Random r;
        int number_of_lineages;
        public int ready_lineages;
        public InputField input1, input2, input3, input4, input5;

        public delegate void ChangeEvent();
        public static event ChangeEvent changeEvent;

        void Start()
        {
            number_of_lineages = 10;
            r = new System.Random();
            slider = GameObject.Find("Slider").GetComponent<Slider>();
            Reset();
        }

        public void Reset()
        {
            slider.onValueChanged.RemoveAllListeners();
            var clones = GameObject.FindGameObjectsWithTag("Clone");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }

            number_of_lineages = input1.text != "" ? Int32.Parse(input1.text) : 10;
            double sigma = input2.text != "" ? Double.Parse(input2.text) : 1.0;
            int m = input3.text!="" ? Int32.Parse(input3.text) : 10;
            double c1 = input4.text != "" ? Double.Parse(input4.text) : 0.82;
            double c2 = input5.text != "" ? Double.Parse(input5.text) : 1.2;
            opo = new OnePlusOneParallel(m, c1, c2, sigma);

            MotionEngine me = new MotionEngine();

            for (int i = 0; i < number_of_lineages; ++i)
            {
                me.SetParameters(new double[8] { 0, r.Next() % 115, 0, -r.Next() % 1.5, -r.Next() % 1, 0, -r.Next() % 1, -r.Next() % 0.5 });
                me.Reset();
                opo.SetProblem(me);
                Transform go = Instantiate(prefab, new Vector3(3.5f, 0, 0), Quaternion.identity);
                go.GetComponent<Program>().index = i;
                go.GetComponent<SpriteRenderer>().color = Color.HSVToRGB((float)r.NextDouble() % 1, (float)r.NextDouble() % 1 / 4 + 0.7f, 1f);
                slider.onValueChanged.AddListener(delegate { go.GetComponent<Program>().ChangeDelta(); } );
            }
            ready_lineages = number_of_lineages;
        }

        public void Restart()
        {
            if (changeEvent != null)
                changeEvent();
        }

        void Update()
        {
            if (ready_lineages == number_of_lineages)
            {
                opo.Iterate(100);
                ready_lineages = 0;
                if (changeEvent != null)
                    changeEvent();
            }
        }
    }
}
