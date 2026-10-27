using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices.Marshalling;

namespace Cross_Zero
{
    enum ElementsGame
    { 
        Zero,
        Cross
    }
    enum CurrentStep
    { 
        Bot,
        Person
    }
    public partial class Form1 : Form
    {
        //текущий ход
        ElementsGame? current_motion;

        //чей ход - нужен при игре с компьютером
        CurrentStep? currentStep;

        //размеры поля
        const int countHorizontalCell = 3;
        const int countVerticallCell = 3;

        //-2 бледный крест при наведении
        //-1 крест 
        //0 ничего
        //1 нолик
        //2 бледный нолик
        int[,] game_place;

        Computer bot;

        //для отображения прогресса обучения сети на дочерней форме
        public static int currentProgressLearning;
        public Form1()
        {
            InitializeComponent();
            current_motion = null;
            currentStep = null;
            button_reset.Enabled = false;
            radioButton_two_players.Checked = true;
            //игровая матрица
            game_place = new int[3, 3];
            bot = new Computer();
        }
        //рисование игрового поля+сделанные ходы+возможный ход
        private void pictureBox_game_place_Paint(object sender, PaintEventArgs e)
        {
            //создание двойной буферизации
            using (Bitmap map = new Bitmap(pictureBox_game_place.Width, pictureBox_game_place.Height))
            {
                using (Graphics g = Graphics.FromImage(map))
                {
                    //залитие фона
                    g.Clear(Color.White);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //карандаш для рисования сетки
                    using (Pen penForGrid = new Pen(Color.Black, 4.0f))
                    {
                        //рисование сетки
                        //рисование горизонтальных линий
                        for (int i = 0; i < countHorizontalCell - 1; i++)
                        {
                            g.DrawLine(penForGrid, 0, ((float)pictureBox_game_place.Height / countHorizontalCell) * (i + 1),
                                pictureBox_game_place.Width, ((float)pictureBox_game_place.Height / countHorizontalCell) * (i + 1));
                        }
                        //рисование вертикальных линий
                        for (int i = 0; i < countVerticallCell - 1; i++)
                        {
                            g.DrawLine(penForGrid, (float)pictureBox_game_place.Width / countVerticallCell * (i + 1), 0,
                                (float)pictureBox_game_place.Width / countVerticallCell * (i + 1), pictureBox_game_place.Height);
                        }
                    }
                    //отрисовка элементов
                    using (Pen penForMinus2 = new Pen(Color.FromArgb(80, 255, 0, 0), 8.0f))
                    using (Pen penForMinus1 = new Pen(Color.Red, 8.0f))
                    using (Pen penFor1 = new Pen(Color.Blue, 8.0f))
                    using (Pen penFor2 = new Pen(Color.FromArgb(80, 0, 0, 255), 8.0f))
                    {
                        for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
                        {
                            for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                            {
                                int shift = 10;
                                //координаты текущей клетки
                                Point leftTop = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * j),
                                    (int)((float)pictureBox_game_place.Height / countHorizontalCell * i));
                                Point rightTop = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * (j + 1)),
                                    (int)((float)pictureBox_game_place.Height / countHorizontalCell * i));
                                Point leftBottom = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * j),
                                    (int)((float)pictureBox_game_place.Height / countHorizontalCell * (i + 1)));
                                Point rightBottom = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * (j + 1)),
                                    (int)((float)pictureBox_game_place.Height / countHorizontalCell * (i + 1)));
                                switch (game_place[i, j])
                                {
                                    case -2:
                                        {
                                            //рисует бледный крестик
                                            g.DrawLine(penForMinus2, leftTop.X + shift, leftTop.Y + shift, rightBottom.X - shift, rightBottom.Y - shift);
                                            g.DrawLine(penForMinus2, rightTop.X - shift, rightTop.Y + shift, leftBottom.X + shift, leftBottom.Y - shift);
                                        }
                                        break;
                                    case -1:
                                        {
                                            //рисует крестик
                                            g.DrawLine(penForMinus1, leftTop.X + shift, leftTop.Y + shift, rightBottom.X - shift, rightBottom.Y - shift);
                                            g.DrawLine(penForMinus1, rightTop.X - shift, rightTop.Y + shift, leftBottom.X + shift, leftBottom.Y - shift);
                                        }
                                        ; break;
                                    case 1:
                                        {
                                            //рисует нолик
                                            g.DrawEllipse(penFor1, leftTop.X + shift, leftTop.Y + shift, (rightTop.X - leftTop.X - 2 * shift),
                                                (leftBottom.Y - leftTop.Y - 2 * shift));
                                        }
                                        ; break;
                                    case 2:
                                        {
                                            //рисует бледный ноль
                                            g.DrawEllipse(penFor2, leftTop.X + shift, leftTop.Y + shift, (rightTop.X - leftTop.X - 2 * shift),
                                                (leftBottom.Y - leftTop.Y - 2 * shift));
                                        }
                                        ; break;
                                }

                            }
                        }
                    }
                }
                e.Graphics.DrawImage(map, 0, 0);
            }
        }
        //рисование кто сейчас ходит
        private void pictureBox_motion_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap map = new Bitmap(pictureBox_motion.Width, pictureBox_motion.Height))
            using (Graphics g = Graphics.FromImage(map))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (current_motion is not null)
                {
                    int shift = 5;
                    if (current_motion == ElementsGame.Zero)
                    {
                        using (Pen pen = new Pen(Color.Blue, 4.0f))
                        {
                            //рисует нолик
                            g.DrawEllipse(pen, shift, shift, pictureBox_motion.Width - 2 * shift, pictureBox_motion.Height - 2 * shift);
                        }
                    }
                    else
                    {
                        using (Pen pen = new Pen(Color.Red, 4.0f))
                        {
                            //рисует крестик
                            g.DrawLine(pen, shift, shift, pictureBox_motion.Width - 2 * shift, pictureBox_motion.Height - 2 * shift);
                            g.DrawLine(pen, pictureBox_motion.Width - 2 * shift, shift, shift, pictureBox_motion.Height - 2 * shift);
                        }
                    }
                }
                e.Graphics.DrawImage(map, 0, 0);
            }

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            button_reset.Enabled = true;
            button_start.Enabled = false;
            radioButton_playerVsComp.Enabled = false;
            radioButton_two_players.Enabled = false;

            Random random = new Random();

            //массив возможных элементов
            ElementsGame[] elements = { ElementsGame.Zero, ElementsGame.Cross };
            //рандомно генерируем кто первый ходит
            ElementsGame startElement = elements[random.Next(elements.Length)];
            //говорим текущему элементу, кто начинает
            current_motion = ElementsGame.Cross;

            if (radioButton_two_players.Checked)
            {
                if (current_motion == ElementsGame.Zero)
                {
                    MessageBox.Show("Игру начинают нолики!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Игру начинают крестики!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!bot.IsReady)
                {
                    FormForLoadingBot form = new FormForLoadingBot();
                    //здесь запускаю обучение сети в отдельном потоке
                    //создаем поток и говорим, что он должне запустить обучение сети
                    var t = new Thread(bot.LearningNetwork);
                    t.IsBackground = true;//поток фоновый
                    t.Start();//запускаем обучение, после обучения будет готовая сеть
                              //currentProgressLearning - при обучении ее нало обновлять, при отображении результата считывать
                              //в основном потоке обновляю состояние
                    form.ShowDialog();//форма не закроется пока поток не доработает
                    form.Dispose();//уничтожаем(освобождаем объект формы)
                }

                //генерируем, кто ходит первый, человек или бот
                int Whofirst = random.Next(0, 2);//0-бот, 1 - человек
                
                if (Whofirst == 0)
                {
                    currentStep = CurrentStep.Bot;
                    MessageBox.Show("Игру начинает Антон!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentStep = CurrentStep.Person;
                    MessageBox.Show("Вы начинаете игру!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                timer_forBot.Start();//запускуем
            }
            timer_checkWinner.Start();
            //отрисовываем
            pictureBox_motion.Invalidate();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            button_reset.Enabled = false;
            button_start.Enabled = true;
            radioButton_two_players.Enabled = true;
            radioButton_playerVsComp.Enabled = true;
            currentStep = null;
            //сбрасываем матрицу
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                    game_place[i, j] = 0;
            current_motion = null;
            pictureBox_game_place.Invalidate();
            pictureBox_motion.Invalidate();
        }

        //должны сбросить -2/2, возможный ход, когда мышь вышла из зоны
        private void pictureBox_game_place_MouseLeave(object sender, EventArgs e)
        {
            //меняем курсор на стрелку
            pictureBox_game_place.Cursor = Cursors.Default;

            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    if (game_place[i, j] == -2 || game_place[i, j] == 2)
                    {
                        game_place[i, j] = 0;
                        goto Exit;
                    }
                }
            }
        Exit:
            pictureBox_game_place.Invalidate();
        }

        private void pictureBox_game_place_MouseMove(object sender, MouseEventArgs e)
        {
            if (current_motion is null)
                return;
            if (radioButton_playerVsComp.Checked && currentStep is null || radioButton_playerVsComp.Checked && currentStep == CurrentStep.Bot)
                return;
            //определяем в какой клетке сейчас мышь
            int index_i = -1, index_j = -1;
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    //координаты текущей клетки
                    Point leftTop = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * j),
                        (int)((float)pictureBox_game_place.Height / countHorizontalCell * i));
                    Point rightBottom = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * (j + 1)),
                        (int)((float)pictureBox_game_place.Height / countHorizontalCell * (i + 1)));
                    if (e.Location.X >= leftTop.X && e.Location.X <= rightBottom.X)
                        index_j = j;
                    if (e.Location.Y >= leftTop.Y && e.Location.Y <= rightBottom.Y)
                        index_i = i;
                    if (index_i != -1 && index_j != -1)
                        goto Exit;
                }
            }
        Exit:
            //определяем можно ли в нее записать временное значение 
            //если нет, то делаем курсор стрелкой
    
            //проходимся по всей сетке, убираем старое значение
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    //удаляем значение, которое было - оно одно
                    if (game_place[i, j] == 2 || game_place[i, j] == -2)
                    {
                        game_place[i, j] = 0;
                        goto Exit2;
                    }
                }
            }
        Exit2:
            //пробуем записать новое значение
            if (game_place[index_i, index_j] == 0)
            {
                //меняем курсор на руку
                pictureBox_game_place.Cursor = Cursors.Hand;
                //записываем -2/2 в зависимости, чей сейчас ход
                if (current_motion == ElementsGame.Zero)
                    game_place[index_i, index_j] = 2;
                else
                    game_place[index_i, index_j] = -2;
            }
            else
            {
                //делаем курсор стрелкой
                pictureBox_game_place.Cursor = Cursors.Default;
            }
            //отрисовываем
            pictureBox_game_place.Invalidate();

        }




        //установка хода человеком
        private void pictureBox_game_place_MouseClick(object sender, MouseEventArgs e)
        {
            //если нажать нельзя было, то и обрабатывать нечего
            if (pictureBox_game_place.Cursor == Cursors.Default)
                return;
            pictureBox_game_place.Cursor = Cursors.Default;//делаем вид, что произошло нажатие
                                                           //определяем в какой клетке нажали
            int index_i = -1, index_j = -1;
            for (int i = 0; i < game_place.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < game_place.GetUpperBound(1) + 1; j++)
                {
                    //координаты текущей клетки
                    Point leftTop = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * j),
                        (int)((float)pictureBox_game_place.Height / countHorizontalCell * i));
                    Point rightBottom = new Point((int)((float)pictureBox_game_place.Width / countVerticallCell * (j + 1)),
                        (int)((float)pictureBox_game_place.Height / countHorizontalCell * (i + 1)));
                    if (e.Location.X >= leftTop.X && e.Location.X <= rightBottom.X)
                        index_j = j;
                    if (e.Location.Y >= leftTop.Y && e.Location.Y <= rightBottom.Y)
                        index_i = i;
                    if (index_i != -1 && index_j != -1)
                        goto Exit;
                }
            }
        Exit:
            if (current_motion == ElementsGame.Cross)
            {
                game_place[index_i, index_j] = -1;
                current_motion = ElementsGame.Zero;//ходит другой
            }
            else
            {
                game_place[index_i, index_j] = 1;
                current_motion = ElementsGame.Cross;//ходит другой
            }
            if(currentStep is not null)//значит играем с компьютером
                currentStep = CurrentStep.Bot;//следующий хол компьютера
            //отрисовка
            pictureBox_game_place.Invalidate();
            pictureBox_motion.Invalidate();
        }

        

        //Ход компьютера
        private void timer_forBot_Tick(object sender, EventArgs e)
        {
            if (currentStep == CurrentStep.Bot)//делает ход
            {
                bot.NextStepComputer(game_place, current_motion == ElementsGame.Zero ? 1 : -1);
                currentStep = CurrentStep.Person;
                if (current_motion == ElementsGame.Cross)
                {
                    current_motion = ElementsGame.Zero;//ходит другим
                }
                else
                {
                    current_motion = ElementsGame.Cross;//ходит другим
                }
                pictureBox_game_place.Invalidate();
                pictureBox_motion.Invalidate();
            }
        }

        //таймер проверяем завершение игры
        private void timer_checkWinner_Tick(object sender, EventArgs e)
        {
            //проверка победы
            int res = CheckWinner();
            //если конец, завершаем
            if (res != 100)
            {
                timer_forBot.Stop();
                timer_checkWinner.Stop();
                if (res == -1)//победили крестики
                {
                    current_motion = null;
                    MessageBox.Show("Победили крестики!", "Игра завершена!", MessageBoxButtons.OK);
                }
                else if (res == 1)
                {
                    current_motion = null;
                    MessageBox.Show("Победили нолики!", "Игра завершена!", MessageBoxButtons.OK);
                }
                else if (res == 0)
                {
                    current_motion = null;
                    MessageBox.Show("Ничья!", "Игра завершена!", MessageBoxButtons.OK);

                }
            }
        }

        //функция проверки победителя
        // -1 - победили крестики
        // 0 - ничья
        // 1- победили нолики
        // 100 - продолжаем играть
        int CheckWinner()
        {
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
