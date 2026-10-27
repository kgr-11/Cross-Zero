using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public class Layer
    {
        Neuron[] neurons;//нейроны слоя 
        public double[] phi;//пороги этих нейронов
        public double[] delta;//дельта для каждого нейрона слоя
        public double[,] W;// размеры: кол-во нейронов на текущем слое*кол-во нейронов на прошлом слое
        double[] lastInput;//выходы предудущего слоя- входы текущего слоя - нужно для обучения
        public Layer(int N_curr, int N_prev)
        {
            //заполняем слой нейронами
            neurons = new Neuron[N_curr];
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = new Neuron();

            //создание матрицы весов + её инициализация 
            W = new double[N_curr, N_prev];
            for (int i = 0; i < W.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < W.GetUpperBound(1) + 1; j++)
                {
                    W[i, j] = (Random.Shared.NextDouble() * 2 - 1) /10;
                }
            }
            //генерация порогов для нейронов
            phi = new double[N_curr];
            for (int i = 0; i < phi.Length; i++)
                phi[i] = (Random.Shared.NextDouble() * 2 - 1) /10;

            delta= new double[N_curr];
        }
        public Layer(int N_curr, int N_prev, double[,] W, double[] phi)
        {
            //заполняем слой нейронами
            neurons = new Neuron[N_curr];
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = new Neuron();
            this.W = W;
            this.phi = phi;
        }
        public List<double> ActivateLayer(List<double> InputData)
        {
            // копирование входных данных
            lastInput = new double[InputData.Count()];
            for (int i = 0; i < lastInput.Length; i++)
                lastInput[i] = InputData[i];
            //активация нейронов слоя 
            List<double> outputData = new List<double>();
            for (int i = 0; i < W.GetUpperBound(0) + 1; i++)
            {
                double S = 0;
                for (int j = 0; j < InputData.Count(); j++)
                {
                    S += W[i, j] * InputData[j];
                }
                neurons[i].Activate(S - phi[i]);//активация нейрона
                outputData.Add(neurons[i].Y);
            }
            return outputData;
        }
        public List<double> SetDeltaOpenLayer(List<double> d, out double[,] W_curr_level)
        {
            List<double> delta_curr_level = new List<double>();
            for (int i = 0; i < neurons.Length; i++)
            {
                delta[i] = neurons[i].DerivateY() * (d[i] - neurons[i].Y);
                delta_curr_level.Add(delta[i]);
            }
            W_curr_level = W;
            return delta_curr_level;
        }
        public List<double> SetDeltaCloseLayer(List<double> delta_top_level, double[,] W_top_level, out double[,] W_curr_level)
        {
            List<double> delta_curr_level = new List<double>();
            for (int i = 0; i < neurons.Length; i++)
            {
                delta[i] = neurons[i].DerivateY();
                double kf = 0;
                for (int k = 0; k < delta_top_level.Count(); k++)
                {
                    kf += delta_top_level[k] * W_top_level[k, i];
                }
                delta[i] *= kf;
                delta_curr_level.Add(delta[i]);
            }
            W_curr_level = W;
            return delta_curr_level;
        }

        public void UpdateWeight(double eps)
        {
            //изменение весов
            for (int i = 0; i < W.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < W.GetUpperBound(1) + 1; j++)
                {
                    W[i, j] += eps * delta[i] * lastInput[j];//изменяем веса всех j входов для i нейрона
                }
            }

            //обновляем пороговые уровни нейронов
            for (int i = 0; i < neurons.Length; i++)
            {
                phi[i]-= eps * delta[i];
            }
        }
    }
}
