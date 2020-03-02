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
      //private System.Collections.ArrayList nombres;
      public ExtensionTwitch() {
         InitializeComponent();
         
      }
      private void consultaSolicitudes_Tick(object sender, EventArgs e) {
         try {
            JSON json = new JSON();
            System.Collections.ArrayList cabeceras = new System.Collections.ArrayList();
            json.cargarJson(Configuracion.parametro("url_extension_twitch")+"solicitudes",cabeceras);
            List<Dictionary<string, Entidad>> peticiones = (List<Dictionary<string, Entidad>>)((Entidad)json["datos"]["peticiones"]).valor;
            Controls.Clear();
            //nombres = new System.Collections.ArrayList();
            for(int i = 0; i < peticiones.Count; i++) {
               Label nombre = new Label();
               nombre.Text = peticiones[i]["idUsuario"].ToString();
               nombre.Width = 200;
               nombre.Height = 20;
               nombre.Top = Padding.Top + (i * (nombre.Height + nombre.Margin.Bottom+nombre.Margin.Top));
               nombre.Left = Padding.Left;
               nombre.Name = "L" + i;
               Controls.Add(nombre);

               Button aceptar = new Button();
               aceptar.Text = "Aceptar";
               aceptar.Width = 60;
               aceptar.Height = 20;
               aceptar.Top = Padding.Top + (i * (nombre.Height + nombre.Margin.Bottom + nombre.Margin.Top));
               aceptar.Left = Padding.Left+aceptar.Margin.Left+nombre.Width+nombre.Margin.Right;
               aceptar.FlatStyle = FlatStyle.Flat;
               aceptar.Click += aceptarSolicitud;
               aceptar.Name = nombre.Text;
               Controls.Add(aceptar);
               //nombres.Add();
            }
            Invalidate();
         } catch(Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);
         }
      }
      private void aceptarSolicitud(object sender, EventArgs e) {
         JSON json = new JSON();
         System.Collections.ArrayList cabeceras = new System.Collections.ArrayList();
         json.cargarJson(Configuracion.parametro("url_extension_twitch") + "aceptar/"+ ((Button)sender).Name, cabeceras);
      }

      private void ExtensionTwitch_Paint(object sender, PaintEventArgs e) {
         /*Brush pincel = new SolidBrush(ForeColor);
         if (nombres != null) {
            for (int i = 0; i < nombres.Count; i++) {
               e.Graphics.DrawString((string)nombres[i], Font, pincel, 0, i * 30);
            }
         }*/
      }

      public delegate void eventoRaton(double x, double y);
      public event eventoRaton onRaton;
      private void control_raton_Tick(object sender, EventArgs e) {
         JSON json = new JSON();
         System.Collections.ArrayList cabeceras = new System.Collections.ArrayList();
         json.cargarJson(Configuracion.parametro("url_extension_twitch") + "posicion_raton", cabeceras);
         if (json.hayClave("datos") && json["datos"].hayClave("posicion")) {
            onRaton?.Invoke((double)(json["datos"].cogerValor("posicion.x")), (double)(json["datos"].cogerValor("posicion.y")));
         }

      }
   }
}
