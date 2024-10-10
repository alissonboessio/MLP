using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Objects
{
    public class TestReturn
    {
        public int TotalCases { get; set; }
        public int QtyCorrect { get; set; } = 0;
        public int QtyWrong { get; set; } = 0;
        public decimal Accuracy{ get; set; }
        public List<List<double>> calculatedOutput { get; set; } = new List<List<double>>();
        public List<List<double>> classifiedOutput { get; set; } = new List<List<double>>();
    }
}
