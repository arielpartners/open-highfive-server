using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Server.Models
{
    public class Calculator
    {
        public int FirstNumber { set; private get; }
        public int SecondNumber { set; private get; }

        public int Add()
        {
            return FirstNumber + SecondNumber;
        }
    }
}
