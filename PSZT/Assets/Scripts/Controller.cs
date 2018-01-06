using System;
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
        Slider slider, sliderIter;
        System.Random r;
        int number_of_lineages;
        public int ready_lineages, number_of_iterations;
        public InputField input1, input2, input3, input4, input5;

        public delegate void ChangeEvent();
        public static event ChangeEvent changeEvent;

        void Start()
        {
            number_of_lineages = 10;
            r = new System.Random();
            slider = GameObject.Find("Slider").GetComponent<Slider>();
            sliderIter = GameObject.Find("SliderIter").GetComponent<Slider>();
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

            number_of_iterations = 1;
            sliderIter.value = 0;
            number_of_lineages = input1.text != "" ? Int32.Parse(input1.text) : 10;
            double sigma = input2.text != "" ? Double.Parse(input2.text) : 1.0;
            int m = input3.text!="" ? Int32.Parse(input3.text) : 10;
            double c1 = input4.text != "" ? Double.Parse(input4.text) : 0.82;
            double c2 = input5.text != "" ? Double.Parse(input5.text) : 1.2;
            opo = new OnePlusOneParallel(m, c1, c2, sigma);

            MotionEngine me = new MotionEngine();

            for (int i = 0; i < number_of_lineages; ++i)
            {
                me.SetParameters(new double[8] { (r.NextDouble()-0.5)*100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100, (r.NextDouble() - 0.5) * 100 });
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

        public void ChangeIterationStep()
        {
            number_of_iterations = (int)Math.Pow(10.0, (double)sliderIter.value);
        }
        double[] lastParams = new Double[8];
        double[] paramChanges = new Double[8];
        double[] avgParamChanges = new Double[8];
        int counter = 1;
        void WriteAndAnalyzeParameters(Double[] tab)
        {
            bool flag = false;
            for (int i=0; i<tab.Length; ++i)
            {
                paramChanges[i] += Math.Abs(tab[i] - lastParams[i]);
                if (tab[i]-lastParams[i] != 0)
                {
                    flag = true;
                }
                lastParams[i] = tab[i];
                avgParamChanges[i] = paramChanges[i] / counter;
            }
            if (flag)
            {
                ++counter;
            }
            Debug.Log("Current params = " + String.Join("   ",
             new List<double>(tab)
             .ConvertAll(i => i.ToString())
             .ToArray()));
            Debug.Log("Avg param changes = " + String.Join("   ",
             new List<double>(avgParamChanges)
             .ConvertAll(i => i.ToString())
             .ToArray()));
        }

        void Update()
        {
            if (ready_lineages == number_of_lineages)
            {
                WriteAndAnalyzeParameters(opo.Iterate(number_of_iterations));
                ready_lineages = 0;
                if (changeEvent != null)
                    changeEvent();
            }
        }
    }
}
