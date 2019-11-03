using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensajería {
   public partial class Alerta : UserControl {
      Image imagen = null;
      public int ancho {
         set {
            Width = value;
         }
         get {
            return Width;
         }
      }
      public int alto {
         set {
            Height = value;
         }
         get {
            return Height;
         }
      }
      public Point localizacion {
         set {
            Location = value;
         }
      }
      public Font fuente {
         set {
            label1.Font = value;
         }
      }
      public string urlImagen {
         set {
            System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(value));
            System.Net.HttpWebResponse imagen = (System.Net.HttpWebResponse)solicitud.GetResponse();
            this.imagen= Image.FromStream(imagen.GetResponseStream());
            //this.imagen
            //this.BackgroundImage// Image = Image.FromStream(imagen.GetResponseStream());
         }
      }
      public int tiempo {
         set {
            timer1.Interval = value;
         }
      }
      public void mostrar(string texto="") {
         label1.Text = texto;
         if (!Visible) {
            gif.Image = imagen;
            Visible = true;
            timer1.Enabled = true;
         }
      }
 
      public Alerta() {
         InitializeComponent();
         Visible = false;
      }

      private void timer1_Tick(object sender, EventArgs e) {
         Visible = false;
         timer1.Enabled = false;
      }

      private void panel_Paint(object sender, PaintEventArgs e) {
         
         Brush brocha = new SolidBrush(Color.FromArgb(255, 255, 255));
         e.Graphics.DrawString(label1.Text, label1.Font, brocha, 0, 0);
      }

      private void gif_Paint(object sender, PaintEventArgs e) {
         Brush brocha;
         Font fuente = label1.Font;
         string[] textos = label1.Text.Split(new char[] { '\r' });
         float posY = 0;
         foreach (string texto in textos) {
            SizeF dimensionTexto = e.Graphics.MeasureString(texto, label1.Font);
            brocha = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            e.Graphics.DrawString(texto, label1.Font, brocha, (Width / 2) - (dimensionTexto.Width / 2) + 2, (Height / 2) - (dimensionTexto.Height / 2) + 2+ posY);
            brocha = new SolidBrush(Color.FromArgb(255, 255, 255));
            e.Graphics.DrawString(texto, label1.Font, brocha, (Width / 2) - (dimensionTexto.Width / 2), (Height / 2) - (dimensionTexto.Height / 2)+ posY);
            posY += dimensionTexto.Height;
         }
      }
   }
}
