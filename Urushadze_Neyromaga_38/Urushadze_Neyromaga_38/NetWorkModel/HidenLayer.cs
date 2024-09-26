namespace Urushadze_Neyromaga_38.NetWorkModel
{
    class HidenLayer : Layer
    {
        public HidenLayer(int non, int nopn, TypeNeyron nt, string type) : base(non, nopn, nt, type)
        {

        }
        public override void Recognize(NetWork net, Layer nextlayer)
        {
            double[] hidden_out = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                hidden_out[i] = Neurons[i].Output;
            }
            nextlayer.Data = hidden_out;
        }
        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < numofprevneurons; j++)
            { //вычисление локального градиента
                double sum = 0;

                for (int k = 0; k < Neurons.Length; k++)
                {
                    sum += Neurons[k].Weights[j] * Neurons[k].Proizv * gr_sums[k];
                }
                gr_sum[j] = sum;
            }
            for (int i = 0; i < numofneurons; i++)
            { //корректирование весов
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw = 0;

                    if (n == 0)
                    {
                        deltaw = momentum * lastdeltaweights[i, 0] + learningrate * Neurons[i].Proizv * gr_sums[i];
                    }
                    else
                    {
                        deltaw = momentum * lastdeltaweights[i, n] + learningrate * Neurons[i].Inputs[n - 1] * Neurons[i].Proizv * gr_sums[i];
                    }
                    lastdeltaweights[i, n] = deltaw;
                    Neurons[i].Weights[n] += deltaw;//коррекция весов

                }
            }
            return gr_sum;
        }
    }
}
