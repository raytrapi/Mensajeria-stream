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
   public partial class Mensaje : UserControl {
      public Mensaje() {
         InitializeComponent();
      }
      int altoMinimo = 200;
      string _mensaje;
      public void mensaje(string mensaje, string avatar, string usuario, int temporizar=10000) {
         altoMinimo = this.Height;
         //this.texto.Text = mensaje;
         _mensaje = mensaje;
         texto.Text = "";
         if (avatar.Length == 0) {
            
         } else {
            //byte[] imageData = System.Net.DownloadData(avatar); //DownloadData function from here
            //MemoryStream stream = new MemoryStream(imageData);
            System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(avatar));
            ;
            System.Net.HttpWebResponse imagen = (System.Net.HttpWebResponse)solicitud.GetResponse();

            this.avatar.Image = Image.FromStream(imagen.GetResponseStream());

         }
         //stream.Close();
         this.usuario.Text = usuario;

         //var size = TextRenderer.MeasureText(text, font, new Size(width, height), TextFormatFlags.WordBreak);
         Size tamañoTexto=TextRenderer.MeasureText(this.texto.Text, this.texto.Font, new Size(this.texto.Width, 1000), TextFormatFlags.WordBreak);
         this.texto.Height = tamañoTexto.Height;
         this.Height = this.texto.Height + this.Margin.Top + this.Margin.Bottom+this.usuario.Height;
         if (this.Height < altoMinimo) {
            this.Height = altoMinimo;
         }
         //this.usuario.Font.

         destructor.Interval = temporizar;
      }
      public void colorFondo(Color color) {
         this.BackColor = color;
      }

      public delegate void eventoBorrar(Mensaje control);
      public event eventoBorrar onBorrar;
      
      private void Destructor_Tick(object sender, EventArgs e) {
         /*if (this.BackColor.A> 0) {
            this.BackColor = Color.FromArgb(this.BackColor.A - 10, this.BackColor.R, this.BackColor.G, this.BackColor.B);
            destructor.Interval = 500;
         } else {/**/
            eventoBorrar borrar = onBorrar;
            borrar?.Invoke(this);
         //}
         
      }

      private void Texto_MouseMove(object sender, MouseEventArgs e) {
         this.OnMouseMove(e);
      }

      private void Texto_MouseEnter(object sender, EventArgs e) {
         this.OnMouseEnter(e);

      }

      private void Texto_MouseLeave(object sender, EventArgs e) {
         this.OnMouseLeave(e);
      }
      public void detenerReloj() {
         destructor.Enabled = false;
         
      }
      public void reanudarReloj() {
         destructor.Enabled = true;
         destructor.Interval /= 2;
      }

      private void texto_Paint(object sender, PaintEventArgs e) {
         string[] palabras = _mensaje.Split(' ');
         //300 palabras por minuto
         destructor.Interval=((int)(((double)palabras.Length /200 ) * 60)*1000)+2000;
         int ancho = texto.Width;
         List<string> frases = new List<string>();
         string fraseActual = "";
         float tamañoFrase = 0;
         float tamañoEspacio= e.Graphics.MeasureString(" ", texto.Font).Width;
         foreach (string palabra in palabras) {
            float tamañoPalabra=e.Graphics.MeasureString(palabra, texto.Font).Width;
            if ((tamañoFrase + tamañoEspacio + tamañoPalabra) < ancho) {
               fraseActual += (tamañoFrase>0?" ":"")+palabra;
               tamañoFrase+= (tamañoFrase > 0 ?tamañoEspacio : 0) + tamañoPalabra;
            } else {
               frases.Add(fraseActual);
               fraseActual = "";
               tamañoFrase = 0;
            }
         }
         if (fraseActual != "") {
            frases.Add(fraseActual);
         }
         float tamañoLinea= e.Graphics.MeasureString(" ", texto.Font).Height;
         int i = 0;
         foreach (string frase in frases) {
            e.Graphics.DrawString(frase, texto.Font, new SolidBrush(texto.ForeColor), 0, i * tamañoLinea);
            i++;
         }
         int tamañoVentana=(int)((frases.Count+1) * tamañoLinea)+this.Padding.Bottom;
         if (tamañoVentana > this.Height) {
            this.Height = tamañoVentana;
         }
      }
   }
}
