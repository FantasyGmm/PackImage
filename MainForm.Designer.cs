namespace PackImage
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.showTimer = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.picbox = new System.Windows.Forms.PictureBox();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.pnMain = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.nbxThread = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nbxHeight = new System.Windows.Forms.NumericUpDown();
            this.nbxWidth = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.worker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).BeginInit();
            this.pnMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxThread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(172, 248);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(262, 248);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // picbox
            // 
            this.picbox.Location = new System.Drawing.Point(44, 88);
            this.picbox.Name = "picbox";
            this.picbox.Size = new System.Drawing.Size(223, 70);
            this.picbox.TabIndex = 5;
            this.picbox.TabStop = false;
            // 
            // pbar
            // 
            this.pbar.Location = new System.Drawing.Point(40, 206);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(468, 17);
            this.pbar.TabIndex = 10;
            // 
            // pnMain
            // 
            this.pnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMain.Controls.Add(this.label3);
            this.pnMain.Controls.Add(this.nbxThread);
            this.pnMain.Controls.Add(this.label2);
            this.pnMain.Controls.Add(this.label1);
            this.pnMain.Controls.Add(this.picbox);
            this.pnMain.Controls.Add(this.nbxHeight);
            this.pnMain.Controls.Add(this.nbxWidth);
            this.pnMain.Controls.Add(this.textBox1);
            this.pnMain.Controls.Add(this.button1);
            this.pnMain.Location = new System.Drawing.Point(40, 30);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(468, 170);
            this.pnMain.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "线程";
            // 
            // nbxThread
            // 
            this.nbxThread.Location = new System.Drawing.Point(273, 61);
            this.nbxThread.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbxThread.Name = "nbxThread";
            this.nbxThread.Size = new System.Drawing.Size(75, 21);
            this.nbxThread.TabIndex = 14;
            this.nbxThread.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "高度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "宽度";
            // 
            // nbxHeight
            // 
            this.nbxHeight.Location = new System.Drawing.Point(131, 61);
            this.nbxHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbxHeight.Name = "nbxHeight";
            this.nbxHeight.Size = new System.Drawing.Size(74, 21);
            this.nbxHeight.TabIndex = 11;
            this.nbxHeight.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // nbxWidth
            // 
            this.nbxWidth.Location = new System.Drawing.Point(51, 61);
            this.nbxWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbxWidth.Name = "nbxWidth";
            this.nbxWidth.Size = new System.Drawing.Size(74, 21);
            this.nbxWidth.TabIndex = 10;
            this.nbxWidth.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(52, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(215, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "C:\\Git\\PackImage\\bin\\Debug\\asus";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "选择文件夹";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // worker
            // 
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 315);
            this.Controls.Add(this.pnMain);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pack";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).EndInit();
            this.pnMain.ResumeLayout(false);
            this.pnMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxThread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer showTimer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox picbox;
        private System.Windows.Forms.ProgressBar pbar;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nbxHeight;
        private System.Windows.Forms.NumericUpDown nbxWidth;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nbxThread;
        private System.ComponentModel.BackgroundWorker worker;
    }
}

