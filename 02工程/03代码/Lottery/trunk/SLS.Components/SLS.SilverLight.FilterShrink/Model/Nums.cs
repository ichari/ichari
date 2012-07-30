using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLS.SilverLight.FilterShrink.Model
{
    public class Nums
    {
        string[] num = new string[] { "3", "0", "1", "" };
        int current = 0;
        public int CurrentNum
        {
            get
            {
                return current;
            }
        }

        public string CurrentNumstr
        {
            set
            {
                for (int i = 0; i < num.Length; i++)
                {
                    if (num[i] == value.ToString())
                    {
                        current = i;
                    }
                }
            }

            get
            {
                return current.ToString();
            }
        }


        public string nextNum()
        {
            if (++current == num.Length)
            {
                current = 0;
            }

            return num[current];
        }
    }
}
