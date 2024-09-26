using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    public interface IFunction
    {
        double Compute(double x);
        double ComputeFirstDerivative(double x);
    }

    internal class SigmoidFunction : IFunction
    {

        private double _alpha = 1;

        internal SigmoidFunction(double alpha)
        {
            _alpha = alpha;
        }

        public double Compute(double x)
        {
            double r = (1 / (1 + Math.Exp(-1 * _alpha * x)));
            //return r == 1f ? 0.9999999f : r;
            return r;
        }

        public double ComputeFirstDerivative(double x)
        {
            return _alpha * this.Compute(x) * (1 - this.Compute(x));
        }
    }
}
