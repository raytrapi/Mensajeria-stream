namespace Mensajería.controles {
   partial class ListaTareas {
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
         this.SuspendLayout();
         // 
         // ListaTareas
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 27F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Font = new System.Drawing.Font("Roboto Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ForeColor = System.Drawing.Color.White;
         this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.Name = "ListaTareas";
         this.Size = new System.Drawing.Size(559, 264);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.ListaTareas_Paint);
         this.ResumeLayout(false);

      }

      #endregion
   }
}
