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
         this.gif = new System.Windows.Forms.PictureBox();
         ((System.ComponentModel.ISupportInitialize)(this.gif)).BeginInit();
         this.SuspendLayout();
         // 
         // timer1
         // 
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // gif
         // 
         this.gif.Location = new System.Drawing.Point(0, 0);
         this.gif.Name = "gif";
         this.gif.Size = new System.Drawing.Size(112, 122);
         this.gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.gif.TabIndex = 3;
         this.gif.TabStop = false;
         this.gif.Visible = false;
         this.gif.Paint += new System.Windows.Forms.PaintEventHandler(this.gif_Paint);
         // 
         // Alerta
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.BackColor = System.Drawing.Color.Transparent;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.Controls.Add(this.gif);
         this.Font = new System.Drawing.Font("Roboto", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ForeColor = System.Drawing.Color.White;
         this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 250);
         this.Name = "Alerta";
         this.Padding = new System.Windows.Forms.Padding(5, 100, 5, 5);
         this.Size = new System.Drawing.Size(246, 168);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Alerta_Paint);
         ((System.ComponentModel.ISupportInitialize)(this.gif)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.PictureBox gif;
   }
}
