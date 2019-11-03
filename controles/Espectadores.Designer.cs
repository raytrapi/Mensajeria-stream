namespace Mensajería.controles {
   partial class Espectadores {
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
         this.imagen = new System.Windows.Forms.PictureBox();
         this.label1 = new System.Windows.Forms.Label();
         this.reiniciar = new System.Windows.Forms.Timer(this.components);
         this.parpadeo = new System.Windows.Forms.Timer(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.imagen)).BeginInit();
         this.SuspendLayout();
         // 
         // imagen
         // 
         this.imagen.Location = new System.Drawing.Point(3, 0);
         this.imagen.Margin = new System.Windows.Forms.Padding(0);
         this.imagen.Name = "imagen";
         this.imagen.Size = new System.Drawing.Size(48, 48);
         this.imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.imagen.TabIndex = 0;
         this.imagen.TabStop = false;
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Roboto", 11.89565F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
         this.label1.Location = new System.Drawing.Point(51, 14);
         this.label1.Margin = new System.Windows.Forms.Padding(0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 34);
         this.label1.TabIndex = 1;
         this.label1.Text = "10";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // reiniciar
         // 
         this.reiniciar.Tick += new System.EventHandler(this.reiniciar_Tick);
         // 
         // parpadeo
         // 
         this.parpadeo.Tick += new System.EventHandler(this.parpadeo_Tick);
         // 
         // Espectadores
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
         this.Controls.Add(this.label1);
         this.Controls.Add(this.imagen);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "Espectadores";
         this.Size = new System.Drawing.Size(89, 48);
         ((System.ComponentModel.ISupportInitialize)(this.imagen)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.PictureBox imagen;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Timer reiniciar;
      private System.Windows.Forms.Timer parpadeo;
   }
}
