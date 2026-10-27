using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public class EPB
    {

        public Network network;
        List<KeyValuePair<int[,], int>> selection;
        public EPB()
        {
            selection = new List<KeyValuePair<int[,], int>>();
            //генерируем начальную выборку 
            int[,] game_place = new int[3, 3];
            GenerateDataset(game_place, -1);//первый ходит крестик 
            //перемешиваем выборку
            Random random = new Random();
            selection = selection.OrderBy(x => random.Next()).ToList();
            int count0 = selection.Count(x => x.Value == 0);
            int count1 = selection.Count(x => x.Value == 1);
            int count_1 = selection.Count(x => x.Value == -1);

            List<int> scelet = new List<int>() { 9,18,1 };//строение МСП
            network = new Network(scelet);
            Train(network,random);
        }

        double Train(Network network, Random random)
        {
            double min_err = 0;
            //цикл по обучающей выборке
            for (int i = 0; i < selection.Count(); i++)
            {
                //берем рандомный пример
                int index_exe = i;
                List<double>input = new List<double>();
                for (int k = 0; k < selection[index_exe].Key.GetLength(0); k++)
                    for (int j = 0; j < selection[index_exe].Key.GetLength(1); j++)
                        input.Add(selection[index_exe].Key[k, j]); 
                network.Learning_with_teacher(input, selection[index_exe].Value, out double curr_err);
                min_err = Math.Min(curr_err, min_err);
            }
            return min_err;
        }

        void GenerateDataset(int[,] game_place, int current_step)
        {
            int[,] game_place_copy = new int[3, 3];
            int res = Teacher(game_place, current_step);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    game_place_copy[i, j] = game_place[i, j];
            selection.Add(new KeyValuePair<int[,], int>(game_place_copy, res));
            if (CheckWinner(game_place) != 100)
            {
                return;
            }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++)
                {
                    if (game_place[i, j] == 0)
                    {
                        game_place[i, j] = current_step;
                        GenerateDataset(game_place, -1 * current_step);
                        game_place[i, j] = 0;
                    }
                }
            }          
        }


        //возвращает 1, если победил тот чей сейчас ход, иначе -1
        private int Teacher(int[,] game_place, int current_step)
        {
            int res = CheckWinner(game_place);
            if (res!= 100)//конец игры
            {
                switch (res)
                {
                    case 0:return 0;
                    default: return res == current_step?1:-1;
                }
            }
            else//продолжаем игру
            {
                List<int> score = new List<int>();
                for (int i = 0; i < game_place.GetLength(0); i++)
                {
                    for (int j = 0; j < game_place.GetLength(1); j++)
                    {
                        if (game_place[i, j] == 0)
                        {
                            game_place[i, j] = current_step;
                            score.Add(-1*Teacher(game_place, -1 * current_step));
                            game_place[i, j] = 0;
                        }
                    }
                }
                int s = score[0];
                foreach (int ss in score)
                { 
                    if(ss>s)
                        s= ss;  
                }
                return s;
            }
        }
        static int CheckWinner(int[,] game_place)
        {
            int countHorizontalCell = 3;
            int countVerticallCell = 3;
            //проверка строк
            int countZero = 0;
            int countCross = 0;
            int countVoid = 0;
            //проверка строк
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                countCross = 0;
                countZero = 0;
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j] == -1)
                        countCross++;
                    else if (game_place[i, j] == 1)
                        countZero++;
                    else
                        countVoid++;
                }
                if (countCross == countHorizontalCell)
                {
                    return -1;
                }
                if (countZero == countHorizontalCell)
                {
                    return 1;
                }
            }

            //проверка столбцов
            for (int i = 0; i < game_place.GetUpperBound(1) + 1; i++)
            {
                countCross = 0;
                countZero = 0;
                for (int j = 0; j < game_place.GetUpperBound(0) + 1; j++)
                {
                    if (game_place[j, i] == -1)
                        countCross++;
                    else if (game_place[j, i] == 1)
                        countZero++;
                }
                if (countCross == countVerticallCell)
                {
                    return -1;
                }
                if (countZero == countVerticallCell)
                {
                    return 1;
                }
            }

            countCross = 0;
            countZero = 0;
            //проверка главной диагонали
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                if (game_place[i, i] == -1)
                    countCross++;
                else if (game_place[i, i] == 1)
                    countZero++;
            }
            if (countCross == countVerticallCell)
            {
                return -1;
            }
            if (countZero == countVerticallCell)
            {
                return 1;
            }

            countCross = 0;
            countZero = 0;
            //проверка побочной диагонали
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                if (game_place[i, game_place.GetUpperBound(0) - i] == -1)
                    countCross++;
                else if (game_place[i, game_place.GetUpperBound(0) - i] == 1)
                    countZero++;
            }
            if (countCross == countVerticallCell)
            {
                return -1;
            }
            if (countZero == countVerticallCell)
            {
                return 1;
            }

            //если не вышли раньше, надо понять, игра закончена в ничью, или еще играем 
            if (countVoid != 0)//не все ячейки заполнены - продолжаем играть
                return 100;
            else//ничья
                return 0;
        }
    }
}