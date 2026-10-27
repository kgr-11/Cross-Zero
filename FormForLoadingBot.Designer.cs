namespace Cross_Zero
{
    partial class FormForLoadingBot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            label1 = new Label();
            progressBar_learningNetwork = new ProgressBar();
            button_play = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(progressBar_learningNetwork);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 118);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Насколько процентов обучена сеть";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 24F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(638, 45);
            label1.Name = "label1";
            label1.Size = new Size(113, 46);
            label1.TabIndex = 1;
            label1.Text = "100%";
            // 
            // progressBar_learningNetwork
            // 
            progressBar_learningNetwork.Location = new Point(6, 39);
            progressBar_learningNetwork.Name = "progressBar_learningNetwork";
            progressBar_learningNetwork.Size = new Size(614, 55);
            progressBar_learningNetwork.Step = 1;
            progressBar_learningNetwork.TabIndex = 0;
            // 
            // button_play
            // 
            button_play.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button_play.Location = new Point(172, 136);
            button_play.Name = "button_play";
            button_play.Size = new Size(447, 75);
            button_play.TabIndex = 1;
            button_play.Text = "Играть";
            button_play.UseVisualStyleBackColor = true;
            button_play.Click += button_play_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // FormForLoadingBot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 221);
            Controls.Add(button_play);
            Controls.Add(groupBox1);
            Name = "FormForLoadingBot";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Обучение сети";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private ProgressBar progressBar_learningNetwork;
        private Button button_play;
        private System.Windows.Forms.Timer timer1;
    }
}