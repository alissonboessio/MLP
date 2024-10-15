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
        public decimal Accuracy { get; set; }
        public Dictionary<string, decimal> Precision { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, TotalByClass> TotalByClass { get; set; } = new Dictionary<string, TotalByClass>();
        public List<List<double>> calculatedOutput { get; set; } = new List<List<double>>();
        public List<List<double>> classifiedOutput { get; set; } = new List<List<double>>();
    }

    public class TotalByClass
    {
        public int total { get; set; } = 0;
        public int totalCorrect { get; set; } = 0;
        public int totalWrong { get; set; } = 0;
    }
}
