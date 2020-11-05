namespace COSI2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.originalPicture = new System.Windows.Forms.PictureBox();
            this.imagePathBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GrayScale = new System.Windows.Forms.PictureBox();
            this.BinaryScale = new System.Windows.Forms.PictureBox();
            this.matrix = new System.Windows.Forms.Label();
            this.claster = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.originalPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrayScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinaryScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.claster)).BeginInit();
            this.SuspendLayout();
            // 
            // originalPicture
            // 
            this.originalPicture.Location = new System.Drawing.Point(12, 41);
            this.originalPicture.Name = "originalPicture";
            this.originalPicture.Size = new System.Drawing.Size(640, 360);
            this.originalPicture.TabIndex = 0;
            this.originalPicture.TabStop = false;
            this.originalPicture.Click += new System.EventHandler(this.button1_click);
            // 
            // imagePathBox
            // 
            this.imagePathBox.Location = new System.Drawing.Point(12, 12);
            this.imagePathBox.Name = "imagePathBox";
            this.imagePathBox.Size = new System.Drawing.Size(207, 20);
            this.imagePathBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(239, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(330, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_click);
            // 
            // GrayScale
            // 
            this.GrayScale.Location = new System.Drawing.Point(12, 413);
            this.GrayScale.Name = "GrayScale";
            this.GrayScale.Size = new System.Drawing.Size(640, 360);
            this.GrayScale.TabIndex = 4;
            this.GrayScale.TabStop = false;
            // 
            // BinaryScale
            // 
            this.BinaryScale.Location = new System.Drawing.Point(12, 779);
            this.BinaryScale.Name = "BinaryScale";
            this.BinaryScale.Size = new System.Drawing.Size(640, 360);
            this.BinaryScale.TabIndex = 5;
            this.BinaryScale.TabStop = false;
            // 
            // matrix
            // 
            this.matrix.AutoSize = true;
            this.matrix.Location = new System.Drawing.Point(678, 41);
            this.matrix.Name = "matrix";
            this.matrix.Size = new System.Drawing.Size(42, 26);
            this.matrix.TabIndex = 7;
            this.matrix.Text = "label1\r\nadsada\r\n";
            this.matrix.Click += new System.EventHandler(this.label1_Click);
            // 
            // claster
            // 
            this.claster.Location = new System.Drawing.Point(681, 413);
            this.claster.Name = "claster";
            this.claster.Size = new System.Drawing.Size(640, 360);
            this.claster.TabIndex = 8;
            this.claster.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1393, 629);
            this.Controls.Add(this.claster);
            this.Controls.Add(this.matrix);
            this.Controls.Add(this.BinaryScale);
            this.Controls.Add(this.GrayScale);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imagePathBox);
            this.Controls.Add(this.originalPicture);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.originalPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrayScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinaryScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.claster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox originalPicture;
        private System.Windows.Forms.TextBox imagePathBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox GrayScale;
        private System.Windows.Forms.PictureBox BinaryScale;
        private System.Windows.Forms.Label matrix;
        private System.Windows.Forms.PictureBox claster;
    }
}

