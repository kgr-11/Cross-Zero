using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public class Network
    {
        public int SizeNetwork//кол-во слоев в сети 
        {
            get { return layers.Length; }
        }

        Layer[] layers;
        //хромосомы особи
        public List<double> chromosomes;
        double Eps;//шаг обучения
        public Network(List<int> scelet)
        {
            CreateNetwork(scelet);
            //инициализируем набор хромосом сети
            Network2Chomosome();
            Eps = 0.05;
        }
        public Network(List<int> scelet,List<double> chromosomes)
        {
            this.chromosomes = new List<double>(chromosomes);//копирование хромосом
            //здесь идет инициализация сети по набору хромосом
            Chomosome2Network(scelet);
            Eps = 0.05;
        }

        private void CreateNetwork(List<int> scelet)
        {
            layers = new Layer[scelet.Count() - 1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(scelet[i + 1], scelet[i]);
            }
        }

        public double Forward(List<double> input)
        {
            //подали входный вектор данных, надо выдать выходной вектор данных
            List<double> result = input;
            for (int i = 0; i < layers.Length; i++)
            {
                //передаем слою входные данные для i-слоя, он выдает выходные данные, это входные данные для i+1 слоя
                result = layers[i].ActivateLayer(result);
            }
            return result[0];
        }

        void Network2Chomosome()
        {
            //запись хромосом
            // записываем так= матрица слоя сверху сниз + вектор порогов слоя , начинаем с младшего слоя 
            chromosomes = new List<double>();
            //перебор слоев
            for (int i = 0; i < layers.Length; i++)
            {
                //перебор матрицы i слоя
                var W = layers[i].W;
                for (int j = 0; j < W.GetUpperBound(0) + 1; j++)
                {
                    for (int k = 0; k < W.GetUpperBound(1) + 1; k++)
                        chromosomes.Add(W[j, k]);
                }
                //перебор порогов
                var phi = layers[i].phi;
                for (int j = 0; j < phi.Length; j++)
                    chromosomes.Add(phi[j]);
            }

        }
        void Chomosome2Network(List<int> scelet)
        {
            //восстанавливаем сеть по набору хромосом
            layers = new Layer[scelet.Count() - 1];
            int num = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                //достаем из набора хромосом матрицу весов для слоя и вектор порогов для этого же слоя
                double[,] W = new double[scelet[i + 1], scelet[i]];
                //заполняем матрицу весов
                for (int j = 0; j < W.GetUpperBound(0) + 1; j++)
                    for (int k = 0; k < W.GetUpperBound(1) + 1; k++)
                        W[j, k] = chromosomes[num++];
                //заполняем вектор порогов
                double[] phi = new double[scelet[i + 1]];
                for (int j = 0; j < phi.Length; j++)
                    phi[j] = chromosomes[num++];
                //заполняем слой
                layers[i] = new Layer(scelet[i + 1], scelet[i], W, phi);
            }
        }
        //проход в обратном направлении - часть обучения
        private void Backward(List<double> d)
        {
            //обновление дельт для выходного слоя
            List<double> delta_top_level = layers[layers.Length - 1].SetDeltaOpenLayer(d, out double[,] W_top_level);
            //обновление дельт для остальных слоев
            for (int i = layers.Length - 2; i >= 0; i--)
            {
                delta_top_level = layers[i].SetDeltaCloseLayer(delta_top_level, W_top_level, out W_top_level);
            }
        }

        public void Learning_with_teacher(List<double> input, double t, out double Err)
        {
            //делаем прямой обход
            double outputData = Forward(input);

            //считаем ошибку
            Err = 0;
            Err += (t - outputData) * (t - outputData);
            Err *= 0.5;

            //процесс обучения
            List<double> d = new List<double>();
            d.Add(t);
            //делаем обратный обход
            Backward(d);

            //обновляем веса
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].UpdateWeight(Eps);//обновляем веса у слоев и пороговые уровни у нейронов слоя
            }
        }
    }
}
