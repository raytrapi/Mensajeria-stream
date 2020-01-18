using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensajería.controles {
   public struct Tarea {
      public bool resuelta;
      public DateTime comienzo;
      public Tarea(bool resultado) {
         this.resuelta = resultado;
         this.comienzo = new DateTime();
         
      }
   }
   public partial class ListaTareas : UserControl {
      System.Collections.Generic.Dictionary<string, Tarea> tareas;
      bool conCambios = false;
      public ListaTareas() {
         InitializeComponent();
         tareas = new Dictionary<string,Tarea>();
      }
      public void añadirTarea(string tarea) {
         conCambios = true;
         tareas.Add(tarea, new Tarea(false));
      }
      public void borrarTarea(string tarea) {
         conCambios = true;
         tareas.Remove(tarea);
      }
      public void realizarTarea(string tarea) {
         Tarea laTarea = tareas[tarea];
         laTarea.resuelta = true;
      }
      private void ListaTareas_Paint(object sender, PaintEventArgs e) {
         Brush picel = new SolidBrush(this.ForeColor);
         Brush picelSombra = new TextureBrush(Image.FromFile("recursos/fondo ojos.png"));
         Font fuenteSombra = new Font(this.Font.FontFamily, (float)(this.Font.Size));
         float y = this.Padding.Top;
         int separacion = 2;
         foreach (string clave in tareas.Keys) {
            e.Graphics.DrawString(clave, fuenteSombra, picelSombra, this.Padding.Left - 1, y - 1);
            e.Graphics.DrawString(clave, fuenteSombra, picelSombra, this.Padding.Left+1, y+1);
            e.Graphics.DrawString(clave, this.Font, picel, this.Padding.Left, y);
            y += e.Graphics.MeasureString(clave, this.Font).Height+separacion;
            //System.Diagnostics.Trace.WriteLine(clave);
         }
         if (conCambios) {
            this.Top = this.Top + (this.Height - ((int)y + this.Padding.Bottom));
            this.Height = (int)y + this.Padding.Bottom;
         }
      }
   }
}
