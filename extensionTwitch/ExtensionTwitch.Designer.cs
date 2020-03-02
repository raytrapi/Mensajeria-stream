namespace Mensajería.extensionTwitch {
   partial class ExtensionTwitch {
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
         this.consultaSolicitudes = new System.Windows.Forms.Timer(this.components);
         this.control_raton = new System.Windows.Forms.Timer(this.components);
         this.SuspendLayout();
         // 
         // consultaSolicitudes
         // 
         this.consultaSolicitudes.Enabled = true;
         this.consultaSolicitudes.Interval = 5000;
         this.consultaSolicitudes.Tick += new System.EventHandler(this.consultaSolicitudes_Tick);
         // 
         // control_raton
         // 
         this.control_raton.Enabled = true;
         this.control_raton.Tick += new System.EventHandler(this.control_raton_Tick);
         // 
         // ExtensionTwitch
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Red;
         this.Font = new System.Drawing.Font("Roboto", 11.26957F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ForeColor = System.Drawing.Color.White;
         this.Margin = new System.Windows.Forms.Padding(4);
         this.Name = "ExtensionTwitch";
         this.Size = new System.Drawing.Size(558, 83);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.ExtensionTwitch_Paint);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Timer consultaSolicitudes;
      private System.Windows.Forms.Timer control_raton;
   }
}
