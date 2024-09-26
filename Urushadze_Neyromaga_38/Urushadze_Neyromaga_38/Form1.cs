using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Urushadze_Neyromaga_38.NetWorkModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace Urushadze_Neyromaga_38
{
    public partial class Form1 : Form
    {
        double[] inputData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        string PathFileTest;
        string textLine = "";
        NetWork netWork;

        public double[] NetOutput
        {
            set
            {
                label_output.Text = value.ToList().IndexOf(value.Max()).ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();
            netWork = new NetWork(NetworkMode.Rec);
        }

        private void ChangeColor_Data(int i, Button b)
        {
            if (b.BackColor == Color.White)
            {
                inputData[i - 1] = 1;
                b.BackColor = Color.Black;
            }
            else
            {
                inputData[i - 1] = 0;
                b.BackColor = Color.White;
            }
        }

        private void ResetButton(int i, Button b)
        {


            b.BackColor = Color.White;
            inputData[i - 1] = 0;

        }
        private void button_reset_Click(object sender, EventArgs e)
        {
            ResetButton(1, button1);
            ResetButton(2, button2);
            ResetButton(3, button3);
            ResetButton(4, button4);
            ResetButton(5, button5);
            ResetButton(6, button6);
            ResetButton(7, button7);
            ResetButton(8, button8);
            ResetButton(9, button9);
            ResetButton(10, button10);
            ResetButton(11, button11);
            ResetButton(12, button12);
            ResetButton(13, button13);
            ResetButton(14, button14);
            ResetButton(15, button15);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(1, button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(2, button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(3, button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(4, button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(5, button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(6, button6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(7, button7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(8, button8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(9, button9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(10, button10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(11, button11);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(12, button12);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(13, button13);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(14, button14);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ChangeColor_Data(15, button15);
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            PathFileTest = AppDomain.CurrentDomain.BaseDirectory + "Train.txt";
            textLine = numericUpDown1.Value.ToString() + " ";
            for (int i = 0; i < inputData.Length; i++)
            {
                if (i < inputData.Length - 1)
                    textLine += inputData[i].ToString() + " ";
                else
                    textLine += inputData[i].ToString();
            }
            textLine += "\n";

            if (!File.Exists(PathFileTest))
            {
                MessageBox.Show("Нет такого файла");
                File.WriteAllText(PathFileTest, textLine);
            }
            else
            {
                File.AppendAllText(PathFileTest, textLine);
            }

        }

        private void Recogn_Click(object sender, EventArgs e)
        {
            netWork.ForwardPass(netWork, inputData);
            label_output.Text = Array.IndexOf(netWork.fact, netWork.fact.Max()).ToString();
        }


        private void TrainButton_Click(object sender, EventArgs e)
        {
            netWork.Train(netWork);
            chart1.Series[0].Points.Clear();
            double[] Y = netWork.E_error_avr;
            for (int i = 0; i < Y.Length; i++)
            {
                chart1.Series[0].Points.AddXY(i + 1, Y[i]);
            }
        }
    }
}
