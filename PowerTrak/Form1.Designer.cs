using System.Drawing;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.getVideo = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.TempoTimer = new System.Windows.Forms.Label();
            this.PauseTimer = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Logo = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(474, 368);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // getVideo
            // 
            this.getVideo.BackColor = System.Drawing.Color.White;
            this.getVideo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.getVideo.Image = ((System.Drawing.Image)(resources.GetObject("getVideo.Image")));
            this.getVideo.Location = new System.Drawing.Point(492, 327);
            this.getVideo.Name = "getVideo";
            this.getVideo.Size = new System.Drawing.Size(104, 53);
            this.getVideo.TabIndex = 1;
            this.getVideo.Text = "Upload Video";
            this.getVideo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.getVideo.UseVisualStyleBackColor = false;
            this.getVideo.Click += new System.EventHandler(this.getVideo_Click);
            // 
            // cancel
            // 
            this.cancel.BackColor = System.Drawing.Color.White;
            this.cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancel.Image = ((System.Drawing.Image)(resources.GetObject("cancel.Image")));
            this.cancel.Location = new System.Drawing.Point(602, 327);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(108, 53);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cancel.UseVisualStyleBackColor = false;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // TempoTimer
            // 
            this.TempoTimer.AutoSize = true;
            this.TempoTimer.Location = new System.Drawing.Point(492, 185);
            this.TempoTimer.Name = "TempoTimer";
            this.TempoTimer.Size = new System.Drawing.Size(69, 13);
            this.TempoTimer.TabIndex = 3;
            this.TempoTimer.Text = "Tempo Timer";
            this.TempoTimer.Click += new System.EventHandler(this.TempoTimer_Click);
            // 
            // PauseTimer
            // 
            this.PauseTimer.AutoSize = true;
            this.PauseTimer.Location = new System.Drawing.Point(492, 215);
            this.PauseTimer.Name = "PauseTimer";
            this.PauseTimer.Size = new System.Drawing.Size(66, 13);
            this.PauseTimer.TabIndex = 4;
            this.PauseTimer.Text = "Pause Timer";
            this.PauseTimer.Click += new System.EventHandler(this.PauseTimer_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Logo
            // 
            this.Logo.Location = new System.Drawing.Point(492, 12);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(300, 108);
            this.Logo.TabIndex = 5;
            this.Logo.TabStop = false;
            this.Logo.Click += new System.EventHandler(this.Logo_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(492, 150);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(489, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Please select the colour of the outermost weight plate:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 392);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.PauseTimer);
            this.Controls.Add(this.TempoTimer);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.getVideo);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button getVideo;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label TempoTimer;
        private System.Windows.Forms.Label PauseTimer;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}

