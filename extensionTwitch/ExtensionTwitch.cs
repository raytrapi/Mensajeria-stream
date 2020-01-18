using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensajería.extensionTwitch {
   public partial class ExtensionTwitch : UserControl {
      private System.Collections.ArrayList nombres;
      public ExtensionTwitch() {
         InitializeComponent();
         
      }
      private void consultaSolicitudes_Tick(object sender, EventArgs e) {
         try {
            JSON json = new JSON();
            System.Collections.ArrayList cabeceras = new System.Collections.ArrayList();
            json.cargarJson(Configuracion.parametro("url_solicitudes"),cabeceras);
            List<Dictionary<string, Entidad>> peticiones = (List<Dictionary<string, Entidad>>)((Entidad)json["datos"]["peticiones"]).valor;
            //Controls.Clear();
            nombres = new System.Collections.ArrayList();
            for(int i = 0; i < peticiones.Count; i++) {
               nombres.Add(peticiones[i]["idUsuario"].ToString());
            }
            Invalidate();
         } catch(Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);
         }
      }

      private void ExtensionTwitch_Paint(object sender, PaintEventArgs e) {
         Brush pincel = new SolidBrush(ForeColor);
         if (nombres != null) {
            for (int i = 0; i < nombres.Count; i++) {
               e.Graphics.DrawString((string)nombres[i], Font, pincel, 0, i * 30);
            }
         }
      }
   }
}
