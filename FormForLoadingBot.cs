using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cross_Zero
{
    public partial class FormForLoadingBot : Form
    {
        public FormForLoadingBot()
        {
            InitializeComponent();
            button_play.Enabled = false;
            timer1.Start();
            timer1.Interval = 16;
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //считываем с родительской формы значение прогресса обучения
            int currentProgress = Form1.currentProgressLearning;
            //отображем текущий прогресс
            progressBar_learningNetwork.Value = currentProgress;
            label1.Text=currentProgress.ToString()+"%";
            if (currentProgress == progressBar_learningNetwork.Maximum)
            {
                timer1.Stop();
                MessageBox.Show("Антон обучен и готов играть", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button_play.Enabled = true;
            }
        }
    }
}
