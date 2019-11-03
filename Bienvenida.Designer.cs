namespace Mensajería {
   partial class Bienvenida {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.mensaje = new System.Windows.Forms.TextBox();
         this.bCambiar = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // mensaje
         // 
         this.mensaje.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mensaje.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.mensaje.Location = new System.Drawing.Point(0, 0);
         this.mensaje.Multiline = true;
         this.mensaje.Name = "mensaje";
         this.mensaje.Size = new System.Drawing.Size(612, 116);
         this.mensaje.TabIndex = 0;
         // 
         // bCambiar
         // 
         this.bCambiar.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.bCambiar.Location = new System.Drawing.Point(0, 116);
         this.bCambiar.Name = "bCambiar";
         this.bCambiar.Size = new System.Drawing.Size(612, 45);
         this.bCambiar.TabIndex = 1;
         this.bCambiar.Text = "Cambiar";
         this.bCambiar.UseVisualStyleBackColor = true;
         this.bCambiar.Click += new System.EventHandler(this.bCambiar_Click);
         // 
         // Bienvenida
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(612, 161);
         this.Controls.Add(this.mensaje);
         this.Controls.Add(this.bCambiar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "Bienvenida";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Bienvenida";
         this.TopMost = true;
         this.Enter += new System.EventHandler(this.Bienvenida_Enter);
         this.Leave += new System.EventHandler(this.Bienvenida_Leave);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox mensaje;
      private System.Windows.Forms.Button bCambiar;
   }
}