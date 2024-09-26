using System;
using static System.Math;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    class Neuron
    {
        private TypeNeyron type; // тип нейрона
        private double[] weight;// веса
        private double[] input;// вход
        private double output;// выход
        private double proizv;// производная функции

        // Свойства
        public double[] Weights { get => weight; set => weight = value; }
        public double[] Inputs { get => input; set => input = value; }
        public double Output { get => output; }
        public double Proizv { get => proizv; }

        public Neuron(double[] w, TypeNeyron typ)
        {
            type = typ;
            weight = w;
        }
        public void Activator(double[] inpt, double[] wght)
        {
            double sum = wght[0];
            for (int m = 0; m < inpt.Length; m++)
                sum += inpt[m] * wght[m + 1];
            switch (type)
            {
                case TypeNeyron.H_Ney:// для нейронов скрытого слоя
                    output = Logistics(sum);
                    break;
                case TypeNeyron.O_Ney:// для нейронов выходного слоя
                    output = Math.Exp(sum);
                    break;
            }
        }
        private double Logistics(double sum) => 1 / (1 + Math.Pow(Math.E, -sum));
        private double Logistics_Derivativator(double sum) => Logistics(sum) * (1 - Logistics(sum));
    }
}
