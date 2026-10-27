using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public class Computer
    {
        Network computer;  //найденная компьютерная сеть для игры

        public bool IsReady;//будет готов, когда обучится
        public Computer()
        {
            IsReady = false;
        }
        
        //функция запуска ГА
        public void LearningNetwork()
        {
            
        }

        public void NextStepComputer(int[,] game_place,int typeElement)
        {
            List<double> currentCondition = new List<double>();//текущее состояние поля
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                    currentCondition.Add(game_place[i, j]);


            //словарь: индекс возможного хода - его значение
            Dictionary<int, double> index_and_value = new Dictionary<int, double>();
            //перебор ходов первой сети
            for (int i = 0; i < currentCondition.Count(); i++)
            {
                if (currentCondition[i] != 0)
                    continue;
                currentCondition[i] = typeElement;
                index_and_value[i] = computer.Forward(currentCondition);//прямой ход
                currentCondition[i] = 0;
            }

            //ищем индекс с максимальной оценкой
            var maxKVP = index_and_value.First();
            foreach (var pair in index_and_value)
            {
                if (pair.Value < maxKVP.Value)
                    maxKVP = pair;
            }
            int bestStepIndex = maxKVP.Key;

            //ход сети
            int row, col;
            row = bestStepIndex / 3;
            col = bestStepIndex - row * 3;
            game_place[row, col] = typeElement;
        }
    }

}
