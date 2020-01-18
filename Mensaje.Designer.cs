namespace Mensajería {
   partial class Mensaje {
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
         this.destructor = new System.Windows.Forms.Timer(this.components);
         this.usuario = new System.Windows.Forms.Label();
         this.avatar = new System.Windows.Forms.PictureBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.texto = new System.Windows.Forms.Label();
         this.panel2 = new System.Windows.Forms.Panel();
         ((System.ComponentModel.ISupportInitialize)(this.avatar)).BeginInit();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // destructor
         // 
         this.destructor.Enabled = true;
         this.destructor.Interval = 10000;
         this.destructor.Tick += new System.EventHandler(this.Destructor_Tick);
         // 
         // usuario
         // 
         this.usuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.usuario.Dock = System.Windows.Forms.DockStyle.Top;
         this.usuario.Font = new System.Drawing.Font("Roboto", 11.89565F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.usuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.usuario.Location = new System.Drawing.Point(0, 0);
         this.usuario.Name = "usuario";
         this.usuario.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
         this.usuario.Size = new System.Drawing.Size(553, 25);
         this.usuario.TabIndex = 3;
         this.usuario.Text = "Mensaje";
         this.usuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.usuario.Paint += new System.Windows.Forms.PaintEventHandler(this.usuario_Paint);
         // 
         // avatar
         // 
         this.avatar.BackColor = System.Drawing.Color.Transparent;
         this.avatar.Location = new System.Drawing.Point(0, 0);
         this.avatar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.avatar.Name = "avatar";
         this.avatar.Size = new System.Drawing.Size(88, 92);
         this.avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.avatar.TabIndex = 1;
         this.avatar.TabStop = false;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.avatar);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
         this.panel1.Location = new System.Drawing.Point(3, 2);
         this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(88, 98);
         this.panel1.TabIndex = 4;
         // 
         // texto
         // 
         this.texto.BackColor = System.Drawing.Color.Transparent;
         this.texto.Dock = System.Windows.Forms.DockStyle.Fill;
         this.texto.Font = new System.Drawing.Font("Roboto", 11.89565F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.texto.ForeColor = System.Drawing.Color.White;
         this.texto.Location = new System.Drawing.Point(0, 25);
         this.texto.Margin = new System.Windows.Forms.Padding(5);
         this.texto.Name = "texto";
         this.texto.Size = new System.Drawing.Size(553, 73);
         this.texto.TabIndex = 0;
         this.texto.Text = "Mensaje";
         this.texto.Paint += new System.Windows.Forms.PaintEventHandler(this.texto_Paint);
         // 
         // panel2
         // 
         this.panel2.AutoSize = true;
         this.panel2.Controls.Add(this.texto);
         this.panel2.Controls.Add(this.usuario);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(91, 2);
         this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(553, 98);
         this.panel2.TabIndex = 5;
         // 
         // Mensaje
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Margin = new System.Windows.Forms.Padding(2);
         this.Name = "Mensaje";
         this.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.Size = new System.Drawing.Size(647, 102);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Mensaje_Paint);
         ((System.ComponentModel.ISupportInitialize)(this.avatar)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Timer destructor;
      private System.Windows.Forms.Label usuario;
      private System.Windows.Forms.PictureBox avatar;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label texto;
      private System.Windows.Forms.Panel panel2;
   }
}
