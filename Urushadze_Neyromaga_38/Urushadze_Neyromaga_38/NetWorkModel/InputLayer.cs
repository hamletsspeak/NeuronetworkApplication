using System;
using System.IO;

namespace Urushadze_Neyromaga_38.NetWorkModel
{
    class InputLayer
    {
        private Random random = new Random();

        //Поля
        private (double[], int)[] trainset = new (double[], int)[100];

        //Свойства
        public (double[], int)[] Trainset { get => trainset; }

        public InputLayer(NetworkMode nm)
        {
            string path_file;
            switch (nm)
            {
                case NetworkMode.Train:
                    // организовать считываеание и формирование trainset из обучающего множества примеров
                    path_file = AppDomain.CurrentDomain.BaseDirectory + "\\Train.txt";
                    string[] all_trains_examples = File.ReadAllLines(path_file);
                    for (int i = 0; i < all_trains_examples.Length; i++)
                    {
                        string[] tmp_train_example = all_trains_examples[i].Split(' ');
                        double[] tmp_w = new double[tmp_train_example.Length - 1];

                        for (int j = 1; j < tmp_train_example.Length; j++)
                        {
                            tmp_w[j - 1] = double.Parse(tmp_train_example[j]);
                        }

                        trainset[i].Item1 = tmp_w;
                        trainset[i].Item2 = int.Parse(tmp_train_example[0]);
                    }
                    break;
                case NetworkMode.Test:
                    break;
                case NetworkMode.Rec:
                    break;
            }
        }
    }
}
