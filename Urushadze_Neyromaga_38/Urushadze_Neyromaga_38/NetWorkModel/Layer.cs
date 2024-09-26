using System;
using System.IO;
using System.Windows.Forms;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    abstract class Layer
    {
        // Поля
        protected string name_Layer; // наименование слоя
        string pathDirWeights; // путь к каталогу с весами
        string pathFileWeights; // путь к файлу синаптических весов
        protected int numofneurons; // число нейронов текущего слоя
        protected int numofprevneurons; // число нейронов предыдущего слоя
        protected const double learningrate = 0.01; // скорость обучения
        protected const double momentum = 0.7d; // момент инерции
        protected double[,] lastdeltaweights; // массив последних изменений весов (веса предыдущей итерации обучения)

        Neuron[] neurons; // массив нейронов
        // Свойства
        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data
        {
            set
            {
                for (int i = 0; i < Neurons.Length; ++i)
                {
                    Neurons[i].Inputs = value; //в цикле на каждый нейрон слоя это значение передается
                    Neurons[i].Activator(Neurons[i].Inputs, Neurons[i].Weights);// ссумируется произвеение весов на соответственное значение(пример был на доске)
                }
            }
        }
        public double[,] Get_Weights()
        {
            double[,] layer_weights = new double[numofneurons, numofprevneurons + 1];
            double[] this_neuron;
            for (int i = 0; i < numofneurons; i++)
            {
                this_neuron = Neurons[i].Weights;
                for (int j = 0; j < numofprevneurons + 1; j++)
                {
                    layer_weights[i, j] = this_neuron[j];
                }
            }
            return layer_weights;
        }

        // Конструктор
        protected Layer(int non, int nopn, TypeNeyron nt, string nm_Layer)
        {
            int i, j; // счетчики циклов
            numofneurons = non; // кол-во нейронов текущего слоя
            numofprevneurons = nopn; // кол-во нейронов предыдущего слоя
            Neurons = new Neuron[non]; // определение массива нейронов
            name_Layer = nm_Layer; // наименование слоя
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory//";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; // временный массив синаптических весов текущего слоя

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights, null);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights, null);
            }

            lastdeltaweights = new double[non, nopn + 1];

            for (i = 0; i < non; ++i) // цикл формирования слоя и заполнение
            {
                double[] tmp_weights = new double[nopn + 1];
                for (j = 0; j < nopn + 1; j++)
                    tmp_weights[j] = Weights[i, j];
                Neurons[i] = new Neuron(tmp_weights, nt);
            }
        }

        // Метод работы с массивом синаптических весов слоя
        public double[,] WeightInitialize(MemoryMode mm, string path, double[,] weights_new)
        {
            int i, j;
            string[] tmpStrWeights; //временный массив строк
            string tmpStr; // строка
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
            char[] delim = new char[] { ';', ' ' }; // массив символов разделителей
            string[] mem_lem;
            switch (mm) // memory mod
            {
                case MemoryMode.GET:

                    tmpStrWeights = File.ReadAllLines(path); // массив строк
                    for (i = 0; i < tmpStrWeights.Length; i++)
                    {
                        mem_lem = tmpStrWeights[i].Split(';');
                        for (j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(mem_lem[j]);
                        }
                    }
                    break;
                case MemoryMode.SET:
                    weights = weights_new;
                    tmpStrWeights = new string[numofneurons];
                    //if (!File.Exists(path))
                    //{
                    //    MessageBox.Show("такого файла нет");
                    //}
                    for (i = 0; i < numofneurons; i++)
                    {
                        tmpStr = Neurons[i].Weights[0].ToString();
                        for (j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpStr += delim[0] + Neurons[i].Weights[j].ToString();
                        }
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights); // создает или перезаписывает файл
                    break;

                case MemoryMode.INIT:
                    Random random = new Random();// генератор случайных чисел
                    double expected_value; //мат. ожидание
                    double standard_deviation; //среднеквадратичное отклонени
                    double correction; //смещение
                    tmpStrWeights = new string[numofneurons]; 
                    // инициализация исходных весов
                    double tmpRatio; // коэф диформации
                    double tmpShift; // смещение
                    double[] tmpArr = new double[numofprevneurons + 1]; // предыдущие нейроны + порог
                    tmpStrWeights = new String[numofneurons]; // массив строк

                    for (i = 0; i < numofneurons; i++)
                    {
                        expected_value = 0;
                        standard_deviation = 0;
                        for (j = 1; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = random.NextDouble();
                            expected_value += weights[i, j];
                        }
                        correction = expected_value / numofprevneurons;
                        for (j = 1; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] -= correction;
                            standard_deviation += Math.Sqrt(Math.Pow(weights[i, j], 2));
                        }
                        standard_deviation /= numofprevneurons;
                        for (j = 1; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] /= standard_deviation;
                        }

                        tmpStr = weights[i, 0].ToString();
                        for (j = 1; j < numofprevneurons + 1; j++)
                        {
                            tmpStr += ';' + weights[i, j].ToString();
                        }
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

            }
            return weights;
        }

        abstract public void Recognize(NetWork net, Layer nextLayer); // метод выполнения прямого прохода сигнала
        abstract public double[] BackwardPass(double[] stuff); // метод выполнения обратного распространения ошибки

    }
}
