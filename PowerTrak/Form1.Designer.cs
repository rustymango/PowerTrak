namespace PowerTrak
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.getVideo = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.TempoTimer = new System.Windows.Forms.Label();
            this.PauseTimer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(21, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(751, 347);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // getVideo
            // 
            this.getVideo.Location = new System.Drawing.Point(21, 369);
            this.getVideo.Name = "getVideo";
            this.getVideo.Size = new System.Drawing.Size(140, 69);
            this.getVideo.TabIndex = 1;
            this.getVideo.Text = "Get_Video";
            this.getVideo.UseVisualStyleBackColor = true;
            this.getVideo.Click += new System.EventHandler(this.getVideo_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(632, 369);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(140, 69);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // TempoTimer
            // 
            this.TempoTimer.AutoSize = true;
            this.TempoTimer.Location = new System.Drawing.Point(676, 32);
            this.TempoTimer.Name = "TempoTimer";
            this.TempoTimer.Size = new System.Drawing.Size(66, 13);
            this.TempoTimer.TabIndex = 3;
            this.TempoTimer.Text = "TempoTimer";
            this.TempoTimer.Click += new System.EventHandler(this.TempoTimer_Click);
            // 
            // PauseTimer
            // 
            this.PauseTimer.AutoSize = true;
            this.PauseTimer.Location = new System.Drawing.Point(676, 61);
            this.PauseTimer.Name = "PauseTimer";
            this.PauseTimer.Size = new System.Drawing.Size(63, 13);
            this.PauseTimer.TabIndex = 4;
            this.PauseTimer.Text = "PauseTimer";
            this.PauseTimer.Click += new System.EventHandler(this.PauseTimer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PauseTimer);
            this.Controls.Add(this.TempoTimer);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.getVideo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button getVideo;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label TempoTimer;
        private System.Windows.Forms.Label PauseTimer;
    }
}

