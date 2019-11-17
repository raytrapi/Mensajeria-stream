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
      int horas=0;
      int minutos=0;
      int segundos=0;
      int horasFin = 0;
      int minutosFin = 0;
      int segundosFin = 0;
      public Espectadores(double _segundos=0) {
         InitializeComponent();
         imagen.Image = Image.FromFile(Configuracion.parametro("imagen_ojo"));
         Random rnd = new Random();
         parpadeo.Interval = rnd.Next(1000, 20000);
         parpadeo.Enabled = true;
         if (_segundos > 0) {
            minutos = (int)(_segundos / 60);
            segundos = (int)(_segundos - (minutos * 60));
            if (minutos > 0) {
               horas = (int)(minutos / 60);
               minutos = (int)(minutos - (horas * 60));
            }
         }
      }
      public int espectadores {
         set {
            label1.Text = value.ToString();
         }
      }
      public double nuevaHora {
         set {
            if (value > 0) {
               minutos = (int)(value / 60);
               segundos = (int)(value - (minutos * 60));
               if (minutos > 0) {
                  horas = (int)(minutos / 60);
                  minutos = (int)(minutos - (horas * 60));
               }
            }
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

      private void segundero_Tick(object sender, EventArgs e) {
         segundos++;
         if (segundos > 59) {
            minutos++;
            segundos = 0;
            if (minutos > 59) {
               horas++;
               minutos = 0;
            }
         }
         tiempoEmision.Text = horas.ToString().PadLeft(2,'0') + ":" + minutos.ToString().PadLeft(2, '0') + ":" + segundos.ToString().PadLeft(2, '0');

         if(horasFin>0 || minutosFin>0 || segundosFin > 0) {
            segundosFin--;
            if (segundosFin <0) {
               minutosFin--;
               segundosFin = 59;
               if (minutosFin < 0) {
                  horasFin--;
                  minutosFin = 59;
               }
            }
            
            tiempoAcabar.Text = horasFin.ToString().PadLeft(2, '0') + ":" + minutosFin.ToString().PadLeft(2, '0') + ":" + segundosFin.ToString().PadLeft(2, '0');
            if (!panelFin.Visible) {
               panelFin.Visible = true;
               this.Width += panelFin.Width + 10;
            }
         } else {
            if (panelFin.Visible) {
               this.Width -= panelFin.Width + 10;
               panelFin.Visible = false;
            }
         }
      }

      public double horaLimite {
         set {
            if (value > 0) {
               minutosFin = (int)(value / 60);
               segundosFin = (int)(value - (minutosFin * 60));
               if (minutosFin > 0) {
                  horasFin = (int)(minutosFin / 60);
                  minutosFin = (int)(minutosFin - (horasFin * 60));
               }
            }
         }
      }
   }
}
