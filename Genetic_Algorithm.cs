using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public static class Genetic_Algorithm
    {
        public static Network GA(List<int> scelet, int countIndivid, int countGeneration, ref int currentLearning)
        {
            //генерация начальной популяции
            List<KeyValuePair<Network, int>> Population = new List<KeyValuePair<Network, int>>();
            for (int i = 0; i < countIndivid; i++)
            {
                Population.Add(new KeyValuePair<Network, int>(new Network(scelet), 0));//генерация особей популяции c нулевым рейтингом
            }
            Random random = new Random();
            
            int countGroups = 5;//кол-во групп разбиений

            List<KeyValuePair<Network, int>> elites=new List<KeyValuePair<Network, int>>();//отсюда потом вернем самую элитную особь

            //идем по поколениям
            for (int generation = 0; generation < countGeneration+1; generation++)
            {
                //разбиение на группы
                List<List<KeyValuePair<Network, int>>> groups = new List<List<KeyValuePair<Network, int>>>();
                for (int g = 0; g < countGroups; g++)
                {
                    List<KeyValuePair<Network, int>> group = new List<KeyValuePair<Network, int>>();
                    //по порядку набираем группы
                    for (int i = g * countIndivid / countGroups; i < (g + 1) * countIndivid / countGroups; i++)
                    {
                        group.Add(new KeyValuePair<Network, int>(Population[i].Key, Population[i].Value));
                    }
                    groups.Add(group);
                }

                //проводим соревнования внутри каждой группы - каждый с каждым
                for (int g = 0; g < countGroups; g++)
                {
                    //соревнования внутри группы - две сети играют друг против друга дважды
                    // один раз крестиками, другой ноликами
                    for (int i = 0; i < groups[g].Count(); i++)
                    {
                        for (int j = 0; j < groups[g].Count(); j++)
                        {
                            if (i == j)
                                continue;
                            Network individ1 = groups[g][i].Key;
                            Network individ2 = groups[g][j].Key;
                            //проводим турнир между двумя выбранными особями
                            Network? winner = Competitions(individ1, individ2,random);
                            if (winner is null)//ничья- +1 к рейтингу, победа +2 к рейнигу, поражение ничего
                            {
                                groups[g][i] = new KeyValuePair<Network, int>(individ1, groups[g][i].Value);
                                groups[g][j] = new KeyValuePair<Network, int>(individ2, groups[g][j].Value);
                            }
                            else
                            {
                                if (winner == individ1)
                                {
                                    groups[g][i] = new KeyValuePair<Network, int>(individ1, groups[g][i].Value + 10);
                                    groups[g][j] = new KeyValuePair<Network, int>(individ2, groups[g][j].Value - 100);
                                }
                                else
                                {
                                    groups[g][j] = new KeyValuePair<Network, int>(individ2, groups[g][j].Value + 10);
                                    groups[g][i] = new KeyValuePair<Network, int>(individ1, groups[g][i].Value - 100);
                                }
                            }
                        }
                    }
                }

                //сохраняем по одному самому элитному из каждой группы
                elites = new List<KeyValuePair<Network, int>>();
                //элетизм
                //выбираем по одной самой элитной особи из группы
                for (int g = 0; g < countGroups; g++)
                {
                    int indexElite = 0;
                    for (int i = 1; i < groups[g].Count(); i++)
                    {
                        if (groups[g][indexElite].Value < groups[g][i].Value)
                            indexElite = i;
                    }
                    elites.Add(new KeyValuePair<Network, int>(groups[g][indexElite].Key, groups[g][indexElite].Value));
                }
                if (generation == countGeneration)
                    break;

                //селекция - элитные особи учавствуют в селекции, чтобы потом участвовать в скрещивании
                List<Network> IndividsForIntersection = new List<Network>();
                for (int i = 0; i < countIndivid; i++)
                {
                    int index1 = random.Next(0, countIndivid);
                    int index2 = random.Next(0, countIndivid);
                    //сравниваем у кого из двух особей рейтинг выше, та и проходит дальше
                    //сравнение теперь между всеми, а не в рамках группы
                    int numberGroup1 = index1 / groups[0].Count();//номер группы=индекс/кол-во участников
                    int numberGroup2 = index2 / groups[0].Count();

                    //номер особи в группе
                    int numberElInGroup1 = index1 - numberGroup1 * groups[0].Count();
                    int numberElInGroup2 = index2 - numberGroup2 * groups[0].Count();


                    if (groups[numberGroup1][numberElInGroup1].Value > groups[numberGroup2][numberElInGroup2].Value)
                    {
                        IndividsForIntersection.Add(groups[numberGroup1][numberElInGroup1].Key);
                    }
                    else
                    {
                        IndividsForIntersection.Add(groups[numberGroup2][numberElInGroup2].Key);
                    }
                }

                //скрещивание
                // создаем особей после мутации - сейчас сохраняем их в виде набора хромосом
                List<List<double>> newPopulation = new List<List<double>>();
                double chance = 0.85;
                for (int k = 0; k < IndividsForIntersection.Count(); k += 2)
                {
                    //два рандомных родителя, т.к. селекция была рандомной
                    int index1 = k;
                    int index2 = k + 1;
                    //их наборы хромосом
                    var coll1 = IndividsForIntersection[index1].chromosomes;
                    var coll2 = IndividsForIntersection[index2].chromosomes;
                    if (random.NextDouble() > chance)//клонирование двух родителей
                    {
                        newPopulation.Add(new List<double>(coll1));
                        newPopulation.Add(new List<double>(coll2));
                        continue;
                    }
                    //скрещивание 
                    int del = random.Next(1, coll1.Count());//индекс деления строки
                    List<double> newColl1 = new List<double>();
                    List<double> newColl2 = new List<double>();
                    //первая часть генов
                    for (int j = 0; j < del; j++)
                    {
                        //для первого потомка
                        newColl1.Add(coll1[j]);
                        //для второго 
                        newColl2.Add(coll2[j]);
                    }
                    //вторая часть генов
                    for (int j = del; j < coll1.Count(); j++)
                    {
                        //для первого потомка
                        newColl1.Add(coll2[j]);
                        //для второго 
                        newColl2.Add(coll1[j]);
                    }
                    //формируем потомков
                    newPopulation.Add(new List<double>(newColl1));
                    newPopulation.Add(new List<double>(newColl2));
                }

                //мутации
                double persectageMutations = 0.30;//30% особей мутируют
                chance = 0.1;//10% генов мутируют
                for (int k = 0; k < newPopulation.Count(); k++)
                {
                    if (random.NextDouble() > persectageMutations)//особь не мутирует
                        continue;
                    for (int j = 0; j < newPopulation[k].Count(); j++)
                    {
                        if (random.NextDouble() < chance)//ген мутируем
                        {
                            if (random.Next(0, 2) == 1)//+0.5
                            {
                                newPopulation[k][j] += 0.5;
                            }
                            else//-0.5
                            {
                                newPopulation[k][j] -= 0.5;
                            }
                        }
                    }
                }

                //сохранение новой популяции
                Population.Clear();//очищаем текущую популяцию
                //Замена популяции на потомков
                for (int k = 0; k < newPopulation.Count() - elites.Count(); k++)
                {
                    Population.Add(new KeyValuePair<Network, int>(new Network(scelet, newPopulation[k]), 0));
                }
                //рандомно в популяцию вставляем элитных особей
                for (int i = 0; i < elites.Count(); i++)
                {
                    int indexInsert = random.Next(0, Population.Count());
                    Population.Insert(indexInsert, new KeyValuePair<Network, int>(elites[i].Key,0));
                }

                //изменяем прогресс обучения
                int currentProgress = (int)((double)generation / countGeneration * 100);
                if (currentProgress < 100)
                {
                    currentLearning = currentProgress;
                }
            }
            //находим лучшего из элитных
            int indexBestEliteIndivid = 0;
            for (int i = 1; i < elites.Count(); i++)
            {
                if (elites[indexBestEliteIndivid].Value < elites[i].Value)
                    indexBestEliteIndivid = i;
            }
            currentLearning = 100;
            return elites[indexBestEliteIndivid].Key;
        }

        //турнир между двумя сетями
        static Network? Competitions(Network network1, Network network2,Random random)
        {
            //создаем игральное поле
            int[,] game_place = new int[3, 3];
            int check = 100;
            bool flag = true;
            int whoFirstNetwork = -1;
            int whoSecondNetwork = 1;
            do
            {
                if (flag)
                {
                    List<double> currentCondition = new List<double>();

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
                        currentCondition[i] = whoFirstNetwork;
                        index_and_value[i] = network1.Forward(currentCondition);//прямой ход
                        currentCondition[i] = 0;
                    }

                    //ищем индекс с минимальной оценкой - худший исход для соперника
                    var minKVP = index_and_value.First();
                    foreach (var pair in index_and_value)
                    {
                        if (pair.Value > minKVP.Value)
                            minKVP = pair;
                    }
                    int bestStepIndex = minKVP.Key;

                    //ход первой сети
                    int row, col;
                    row = bestStepIndex / 3;
                    col = bestStepIndex - row * 3;
                    game_place[row, col] = whoFirstNetwork;

                    //указываем, что следующий ход другой сети
                    flag = false;
                }
                else
                {
                    List<double> currentCondition = new List<double>();
                    for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
                        for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                            currentCondition.Add(game_place[i, j]*-1);



                    //словарь: индекс возможного хода - его значение
                    Dictionary<int, double> index_and_value = new Dictionary<int, double>();
                    //перебор ходов второй сети
                    for (int i = 0; i < currentCondition.Count(); i++)
                    {
                        if (currentCondition[i] != 0)
                            continue;
                        currentCondition[i] = whoSecondNetwork;
                        index_and_value[i] = network2.Forward(currentCondition);//прямой ход
                        currentCondition[i] = 0;
                    }
                    //ищем индекс с максимальной оценкой
                    var minKVP = index_and_value.First();
                    foreach (var pair in index_and_value)
                    {
                        if (pair.Value > minKVP.Value)
                            minKVP = pair;
                    }
                    int bestStepIndex = minKVP.Key;
                    //ход второй сети
                    //ход первой сети
                    int row, col;
                    row = bestStepIndex / 3;
                    col = bestStepIndex - row * 3;
                    game_place[row, col] = whoSecondNetwork;

                    //указываем, что следующий ход другой сети
                    flag = true;
                }
                check = CheckWinner(game_place);
            } while (check == 100);
            if (check == 1)
            {
                if (whoFirstNetwork == 1)
                    return network1;
                else
                    return network2;
            }
            else if (check == -1)
            {
                if (whoFirstNetwork == -1)
                    return network1;
                else
                    return network2;
            }
            return null;
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
