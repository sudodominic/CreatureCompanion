namespace KoboldKompanion
{
    partial class petHeart
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
            this.imgHeart = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgHeart)).BeginInit();
            this.SuspendLayout();
            // 
            // imgHeart
            // 
            this.imgHeart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgHeart.Image = global::KoboldKompanion.Properties.Resources.heart1;
            this.imgHeart.Location = new System.Drawing.Point(0, 0);
            this.imgHeart.Name = "imgHeart";
            this.imgHeart.Size = new System.Drawing.Size(32, 32);
            this.imgHeart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgHeart.TabIndex = 0;
            this.imgHeart.TabStop = false;
            // 
            // petHeart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(32, 32);
            this.Controls.Add(this.imgHeart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "petHeart";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "petHeart";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.petHeart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgHeart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgHeart;
    }
}