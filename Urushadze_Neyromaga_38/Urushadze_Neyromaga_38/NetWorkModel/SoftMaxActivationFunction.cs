using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    internal class SoftMaxActivationFunction : IFunction
    {

        private ILayer _layer = null;
        private int _ownPosition = 0;

        internal SoftMaxActivationFunction(ILayer layer, int ownPosition)
        {
            _layer = layer;
            _ownPosition = ownPosition;
        }

        public double Compute(double x)
        {
            double numerator = Math.Exp(x);
            double denominator = numerator;
            for (int i = 0; i < _layer.Neurons.Length; i++)
            {
                if (i == _ownPosition)
                {
                    continue;
                }
                denominator += Math.Exp(_layer.Neurons[i].LastNET);
            }
            return numerator / denominator;
        }

        public double ComputeFirstDerivative(double x)
        {
            double y = Compute(x);
            return y * (1 - y);
        }

        public double ComputeSecondDerivative(double x)
        {
            throw new NotImplementedException();
        }
    }
}
