namespace Mensajería {
   partial class Alerta {
      /// <summary> 
      /// Variable del diseñador necesaria.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Limpiar los recursos que se estén usando.
      /// </summary>
      /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Código generado por el Diseñador de componentes

      /// <summary> 
      /// Método necesario para admitir el Diseñador. No se puede modificar
      /// el contenido de este método con el editor de código.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.label1 = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.gif = new System.Windows.Forms.PictureBox();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gif)).BeginInit();
         this.SuspendLayout();
         // 
         // timer1
         // 
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // label1
         // 
         this.label1.BackColor = System.Drawing.Color.Transparent;
         this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.label1.Font = new System.Drawing.Font("Roboto", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.Gainsboro;
         this.label1.Location = new System.Drawing.Point(0, 0);
         this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(112, 122);
         this.label1.TabIndex = 1;
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // panel1
         // 
         this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.panel1.Controls.Add(this.label1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(112, 122);
         this.panel1.TabIndex = 2;
         // 
         // gif
         // 
         this.gif.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gif.Location = new System.Drawing.Point(0, 0);
         this.gif.Name = "gif";
         this.gif.Size = new System.Drawing.Size(112, 122);
         this.gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.gif.TabIndex = 3;
         this.gif.TabStop = false;
         this.gif.Paint += new System.Windows.Forms.PaintEventHandler(this.gif_Paint);
         // 
         // Alerta
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.Controls.Add(this.gif);
         this.Controls.Add(this.panel1);
         this.Margin = new System.Windows.Forms.Padding(2);
         this.Name = "Alerta";
         this.Size = new System.Drawing.Size(112, 122);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gif)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.PictureBox gif;
   }
}
