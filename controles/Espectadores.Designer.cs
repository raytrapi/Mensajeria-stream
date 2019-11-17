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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Espectadores));
         this.imagen = new System.Windows.Forms.PictureBox();
         this.label1 = new System.Windows.Forms.Label();
         this.reiniciar = new System.Windows.Forms.Timer(this.components);
         this.parpadeo = new System.Windows.Forms.Timer(this.components);
         this.tiempoEmision = new System.Windows.Forms.Label();
         this.segundero = new System.Windows.Forms.Timer(this.components);
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.tiempoAcabar = new System.Windows.Forms.Label();
         this.panelFin = new System.Windows.Forms.Panel();
         this.panelTiempoEmisión = new System.Windows.Forms.Panel();
         ((System.ComponentModel.ISupportInitialize)(this.imagen)).BeginInit();
         this.panelFin.SuspendLayout();
         this.panelTiempoEmisión.SuspendLayout();
         this.SuspendLayout();
         // 
         // imagen
         // 
         this.imagen.Location = new System.Drawing.Point(4, 0);
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
         this.label1.Location = new System.Drawing.Point(52, 6);
         this.label1.Margin = new System.Windows.Forms.Padding(0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(51, 42);
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
         // tiempoEmision
         // 
         this.tiempoEmision.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.tiempoEmision.Font = new System.Drawing.Font("Roboto Thin", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tiempoEmision.ForeColor = System.Drawing.Color.Gainsboro;
         this.tiempoEmision.Location = new System.Drawing.Point(0, 25);
         this.tiempoEmision.Name = "tiempoEmision";
         this.tiempoEmision.Size = new System.Drawing.Size(137, 23);
         this.tiempoEmision.TabIndex = 2;
         this.tiempoEmision.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // segundero
         // 
         this.segundero.Enabled = true;
         this.segundero.Interval = 1000;
         this.segundero.Tick += new System.EventHandler(this.segundero_Tick);
         // 
         // label2
         // 
         this.label2.Dock = System.Windows.Forms.DockStyle.Top;
         this.label2.Font = new System.Drawing.Font("Roboto Thin", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.Gainsboro;
         this.label2.Location = new System.Drawing.Point(0, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(137, 19);
         this.label2.TabIndex = 3;
         this.label2.Text = "Tiempo emisión";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label3
         // 
         this.label3.Dock = System.Windows.Forms.DockStyle.Top;
         this.label3.Font = new System.Drawing.Font("Roboto Thin", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.ForeColor = System.Drawing.Color.Gainsboro;
         this.label3.Location = new System.Drawing.Point(2, 2);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(94, 19);
         this.label3.TabIndex = 5;
         this.label3.Text = "Acaba en ...";
         this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // tiempoAcabar
         // 
         this.tiempoAcabar.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.tiempoAcabar.Font = new System.Drawing.Font("Roboto Thin", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tiempoAcabar.ForeColor = System.Drawing.Color.Gainsboro;
         this.tiempoAcabar.Location = new System.Drawing.Point(2, 23);
         this.tiempoAcabar.Name = "tiempoAcabar";
         this.tiempoAcabar.Size = new System.Drawing.Size(94, 23);
         this.tiempoAcabar.TabIndex = 4;
         this.tiempoAcabar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // panelFin
         // 
         this.panelFin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
         this.panelFin.Controls.Add(this.label3);
         this.panelFin.Controls.Add(this.tiempoAcabar);
         this.panelFin.Location = new System.Drawing.Point(243, 3);
         this.panelFin.Name = "panelFin";
         this.panelFin.Padding = new System.Windows.Forms.Padding(2);
         this.panelFin.Size = new System.Drawing.Size(98, 48);
         this.panelFin.TabIndex = 6;
         this.panelFin.Visible = false;
         // 
         // panelTiempoEmisión
         // 
         this.panelTiempoEmisión.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
         this.panelTiempoEmisión.Controls.Add(this.label2);
         this.panelTiempoEmisión.Controls.Add(this.tiempoEmision);
         this.panelTiempoEmisión.Location = new System.Drawing.Point(106, 3);
         this.panelTiempoEmisión.Name = "panelTiempoEmisión";
         this.panelTiempoEmisión.Size = new System.Drawing.Size(137, 48);
         this.panelTiempoEmisión.TabIndex = 7;
         // 
         // Espectadores
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.Controls.Add(this.panelTiempoEmisión);
         this.Controls.Add(this.panelFin);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.imagen);
         this.DoubleBuffered = true;
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "Espectadores";
         this.Size = new System.Drawing.Size(239, 50);
         ((System.ComponentModel.ISupportInitialize)(this.imagen)).EndInit();
         this.panelFin.ResumeLayout(false);
         this.panelTiempoEmisión.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.PictureBox imagen;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Timer reiniciar;
      private System.Windows.Forms.Timer parpadeo;
      private System.Windows.Forms.Label tiempoEmision;
      private System.Windows.Forms.Timer segundero;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label tiempoAcabar;
      private System.Windows.Forms.Panel panelFin;
      private System.Windows.Forms.Panel panelTiempoEmisión;
   }
}
