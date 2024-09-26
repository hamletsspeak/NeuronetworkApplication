using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    class NetWork
    {
        //Поля
        private InputLayer input_layer = null;
        private HidenLayer hiden_layer1 = new HidenLayer(73, 15, TypeNeyron.H_Ney, nameof(hiden_layer1));
        private HidenLayer hiden_layer2 = new HidenLayer(34, 73, TypeNeyron.H_Ney, nameof(hiden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 34, TypeNeyron.O_Ney, nameof(OutputLayer));


        public double[] fact = new double[10];

        private double[] e_error_avr;//Среднее значение энергии ошибки эпохи обучения

        public double[] E_error_avr
        {
            get => e_error_avr;
            set => e_error_avr = value;
        }
        // конструктор
        public NetWork(NetworkMode nm)
        {
            input_layer = new InputLayer(nm);
        }
        //прямой проход сети
        public void ForwardPass(NetWork net, double[] netInput)
        {
            net.hiden_layer1.Data = netInput;
            net.hiden_layer1.Recognize(null, net.hiden_layer2);
            net.hiden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }

        public void Train(NetWork net)
        {
            int epoches = 70; // кол-во эпох обучения
            net.input_layer = new InputLayer(NetworkMode.Train);
            double tmpSumError;// временная переменная суммы ошибок
            double[] errors;   // вектор (массив) сигнала ошибки выходного слоя
            double[] temp_gsums1; // вектор градиента 1-го скрытого слоя
            double[] temp_gsums2; // вектор градиента 2-го скрытого слоя
            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++)
            {
                //Console.WriteLine(5);
                e_error_avr[k] = 0;
                for (int i = 0; i < net.input_layer.Trainset.Length; i++)
                {
                    //Console.WriteLine(net.input_layer.Trainset[i].Item1.Length);
                    // прямой проход
                    ForwardPass(net, net.input_layer.Trainset[i].Item1);

                    // вычисления ошибок по итерации
                    tmpSumError = 0; // для каждого тобучающего образца среднее значение суммы ошибок равно 0
                    errors = new double[net.fact.Length];

                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.Trainset[i].Item2)
                        {
                            errors[x] = -(net.fact[x] - 1.0);
                        }
                        else
                        {
                            errors[x] = -(net.fact[x]);
                        }
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; // суммарное значение ошибки

                    // Обратный проход и корректировка весов
                    temp_gsums2 = net.output_layer.BackwardPass(errors);
                    temp_gsums1 = net.hiden_layer2.BackwardPass(temp_gsums2);
                    net.hiden_layer1.BackwardPass(temp_gsums1);
                }

                e_error_avr[k] /= net.input_layer.Trainset.Length; // среднее значение ошибки к-эпохи
                ////
                Console.WriteLine(k + "\t" + e_error_avr[k]);
                /////
                // здаесь написать код отображения средней ошибки эпохим на графике (сделаем на следующем занятии)

            }
            net.input_layer = null; //обнуление (уборка) входного слоя

            // запись скорректированных весов в памятим
            string pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            net.hiden_layer1.WeightInitialize(MemoryMode.SET, nameof(hiden_layer1) + "_memory.csv", hiden_layer1.Get_Weights());
            net.hiden_layer2.WeightInitialize(MemoryMode.SET, nameof(hiden_layer2) + "_memory.csv", hiden_layer2.Get_Weights());
            net.output_layer.WeightInitialize(MemoryMode.SET, nameof(output_layer) + "_memory.csv", output_layer.Get_Weights());
        }
    }
}
