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
   public partial class Espectadores : UserControl {
      public Espectadores() {
         InitializeComponent();
         imagen.Image = Image.FromFile(Configuracion.parametro("imagen_ojo"));
         Random rnd = new Random();
         parpadeo.Interval = rnd.Next(1000, 20000);
         parpadeo.Enabled = true;
      }
      public int espectadores {
         set {
            label1.Text = value.ToString();
         }
      }
      public bool nuevo {
         set {
            if (value) {
               imagen.Image = Image.FromFile(Configuracion.parametro("imagen_giño"));
               reiniciar.Interval = 3000;
               reiniciar.Enabled = true;
            } else {
               imagen.Image = Image.FromFile(Configuracion.parametro("imagen_enfado"));
               reiniciar.Interval = 3000;
               reiniciar.Enabled = true;
            }
         }
      }
      
      private void reiniciar_Tick(object sender, EventArgs e) {
         reiniciar.Enabled = false;
         imagen.Image = Image.FromFile(Configuracion.parametro("imagen_ojo"));
      }

      private void parpadeo_Tick(object sender, EventArgs e) {
         if (!reiniciar.Enabled) {
            imagen.Image = Image.FromFile(Configuracion.parametro("imagen_parpadeo"));
            reiniciar.Interval = 2000;
            reiniciar.Enabled = true;
            Random rnd = new Random();
            parpadeo.Interval = rnd.Next(1000, 20000);
         }
      }
   }
}
