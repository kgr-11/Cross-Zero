namespace Cross_Zero
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            pictureBox_game_place = new PictureBox();
            groupBox2 = new GroupBox();
            groupBox4 = new GroupBox();
            radioButton_playerVsComp = new RadioButton();
            radioButton_two_players = new RadioButton();
            groupBox3 = new GroupBox();
            label1 = new Label();
            pictureBox_motion = new PictureBox();
            button_reset = new Button();
            button_start = new Button();
            timer_forBot = new System.Windows.Forms.Timer(components);
            timer_checkWinner = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_game_place).BeginInit();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_motion).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(pictureBox_game_place);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(613, 638);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Игровое поле";
            // 
            // pictureBox_game_place
            // 
            pictureBox_game_place.Location = new Point(6, 26);
            pictureBox_game_place.Name = "pictureBox_game_place";
            pictureBox_game_place.Size = new Size(600, 600);
            pictureBox_game_place.TabIndex = 0;
            pictureBox_game_place.TabStop = false;
            pictureBox_game_place.Paint += pictureBox_game_place_Paint;
            pictureBox_game_place.MouseClick += pictureBox_game_place_MouseClick;
            pictureBox_game_place.MouseLeave += pictureBox_game_place_MouseLeave;
            pictureBox_game_place.MouseMove += pictureBox_game_place_MouseMove;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(button_reset);
            groupBox2.Controls.Add(button_start);
            groupBox2.Location = new Point(631, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(250, 488);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Панель управления";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(radioButton_playerVsComp);
            groupBox4.Controls.Add(radioButton_two_players);
            groupBox4.Location = new Point(6, 127);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(238, 132);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Режим игры";
            // 
            // radioButton_playerVsComp
            // 
            radioButton_playerVsComp.AutoSize = true;
            radioButton_playerVsComp.Font = new Font("Times New Roman", 13.8F);
            radioButton_playerVsComp.Location = new Point(6, 82);
            radioButton_playerVsComp.Name = "radioButton_playerVsComp";
            radioButton_playerVsComp.Size = new Size(229, 30);
            radioButton_playerVsComp.TabIndex = 1;
            radioButton_playerVsComp.TabStop = true;
            radioButton_playerVsComp.Text = "Против компьютера";
            radioButton_playerVsComp.UseVisualStyleBackColor = true;
            // 
            // radioButton_two_players
            // 
            radioButton_two_players.AutoSize = true;
            radioButton_two_players.Font = new Font("Times New Roman", 13.8F);
            radioButton_two_players.Location = new Point(6, 36);
            radioButton_two_players.Name = "radioButton_two_players";
            radioButton_two_players.Size = new Size(116, 30);
            radioButton_two_players.TabIndex = 0;
            radioButton_two_players.TabStop = true;
            radioButton_two_players.Text = "2 игрока";
            radioButton_two_players.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(pictureBox_motion);
            groupBox3.Location = new Point(6, 26);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(238, 95);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Текущий ход";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 13.8F);
            label1.Location = new Point(19, 44);
            label1.Name = "label1";
            label1.Size = new Size(62, 26);
            label1.TabIndex = 0;
            label1.Text = "Ход: ";
            // 
            // pictureBox_motion
            // 
            pictureBox_motion.Location = new Point(87, 26);
            pictureBox_motion.Name = "pictureBox_motion";
            pictureBox_motion.Size = new Size(60, 60);
            pictureBox_motion.TabIndex = 1;
            pictureBox_motion.TabStop = false;
            pictureBox_motion.Paint += pictureBox_motion_Paint;
            // 
            // button_reset
            // 
            button_reset.Font = new Font("Times New Roman", 13.8F);
            button_reset.Location = new Point(6, 376);
            button_reset.Name = "button_reset";
            button_reset.Size = new Size(238, 101);
            button_reset.TabIndex = 3;
            button_reset.Text = "Сброс";
            button_reset.UseVisualStyleBackColor = true;
            button_reset.Click += button_reset_Click;
            // 
            // button_start
            // 
            button_start.Font = new Font("Times New Roman", 13.8F);
            button_start.Location = new Point(6, 265);
            button_start.Name = "button_start";
            button_start.Size = new Size(238, 105);
            button_start.TabIndex = 2;
            button_start.Text = "Начать игру";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // timer_forBot
            // 
            timer_forBot.Tick += timer_forBot_Tick;
            // 
            // timer_checkWinner
            // 
            timer_checkWinner.Interval = 16;
            timer_checkWinner.Tick += timer_checkWinner_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(894, 659);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Игра \"Крестики-Нолики\"";
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox_game_place).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_motion).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private PictureBox pictureBox_game_place;
        private GroupBox groupBox2;
        private Button button_reset;
        private Button button_start;
        private PictureBox pictureBox_motion;
        private Label label1;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private RadioButton radioButton_playerVsComp;
        private RadioButton radioButton_two_players;
        private System.Windows.Forms.Timer timer_forBot;
        private System.Windows.Forms.Timer timer_checkWinner;
    }
}
