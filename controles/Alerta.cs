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
      Bitmap imagen = null;
      int _alto = 400;
      int _ancho = 400;
      int _izquierda = 0;
      int _arriba = 0;
      string _texto = "";
      int separacion = 0;
      Brush _pincel;
      Brush _pincelSombra;
      Font _fuenteSombra;
      Font _fuente;
      public int ancho {
         set {
            _ancho = value;
         }
      }
      public int alto {
         set {
            _alto = value;
            
         }
      }
      public Point localizacion {
         set {
            Location = value;
         }
      }
      public Font fuente {
         set {
            Font = value;
         }
      }
      public string urlImagen {
         set {
            System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(value));
            System.Net.HttpWebResponse imagen = (System.Net.HttpWebResponse)solicitud.GetResponse();
            this.imagen = new Bitmap(imagen.GetResponseStream());
            //this.gif.Visible = true;
            //this.imagen
            //this.BackgroundImage// Image = Image.FromStream(imagen.GetResponseStream());
            ImageAnimator.Animate(this.imagen, dibujarFrame);
            
         }
      }
      private void dibujarFrame(object sender, EventArgs e) {
         this.Invalidate();
         //this.Refresh();
      }
      public Size sizeImagen {
         get {
            if (this.imagen != null) {
               return this.imagen.Size;
            }
            return new Size(0, 0);
         }
      }
      public int tiempo {
         set {
            timer1.Interval = value;
         }
      }
      public void mostrar(string texto="") {
         _texto = texto;
         if (!Visible) {
            //gif.Image = imagen;
            /*gif.Width = _ancho;
            gif.Height = _alto;*/
            Visible = true;
            timer1.Enabled = true;
         }

         string[] textos = _texto.Split(new char[] { '\n' });
         float posY = this.Padding.Top;

         int Ancho = 0;
         int Alto = 0;
         //Font fuente = this.Font;
         Graphics g=Graphics.FromImage(this.imagen);
         foreach (string text in textos) {
            SizeF dimensionTexto = g.MeasureString(text, _fuente);
            if (text[0] != '#') {
               
               if (dimensionTexto.Width > Ancho) {
                  Ancho = (int)dimensionTexto.Width;
               }

               posY += (int)dimensionTexto.Height + separacion;
            } else {
               posY+=procesarParámetro(text,dimensionTexto.Height);
               
               
            }
         }
         if (posY > Alto) {
            Alto = (int)posY;
         }

         //this.Top = this.Top + (this.Height - ((int)posY + this.Padding.Bottom));
         //         this.Height = (int)posY + this.Padding.Bottom;
         //Refresh();
         int iW = (int)Math.Round(((double)this.imagen.Width / (double)this.imagen.Height) * (double)_alto);
         int iH = (int)Math.Round(((double)this.imagen.Height / (double)this.imagen.Width) * (double)_ancho);
         if (iW < _ancho) {
            _ancho = iW;
            _alto= (int)Math.Round((this.imagen.Height / this.imagen.Width) * (double)_ancho);
         };
         if(iH<_alto) {
            _alto = iH;
            _ancho = (int)Math.Round(((double)this.imagen.Width / (double)this.imagen.Height) * (double)_alto);
         }
         if (Ancho < _ancho) {
            Ancho = _ancho;
         }
         if (Alto < _alto) {
            Alto = _alto;
         }
         this.Left = this.Left + (this.Width - (Ancho + this.Padding.Right));
         this.Top = this.Top + (this.Height - (Alto + this.Padding.Bottom));
         this.Width = Ancho;
         this.Height = Alto;
         _izquierda = (int)Math.Round((Width / 2) - ((double)_ancho/2)) - this.Padding.Right;
         if (_izquierda < 0) {
            _izquierda = 0;
         }
         _arriba = 0;// +this.Padding.Top;// (int)Math.Round((Height / 2) - ((double)_alto / 2)) - this.Padding.Bottom;
         /*if (_arriba < 0) {
            _arriba = 0;
         }/**/
      }
 
      public Alerta() {
         InitializeComponent();
         Visible = false;
         this.DoubleBuffered = true;
         _pincel = new SolidBrush(this.ForeColor);
         _pincelSombra = new TextureBrush(Image.FromFile("recursos/fondo ojos.png"));
         _fuenteSombra = new Font(this.Font.FontFamily, (float)(this.Font.Size));
         _fuente = this.Font;
      }

      private void timer1_Tick(object sender, EventArgs e) {
         Visible = false;
         //timer1.Enabled = false;
      }

      private void panel_Paint(object sender, PaintEventArgs e) {
         
         
         
         //Brush brocha = new SolidBrush(Color.FromArgb(255, 255, 255));
         //e.Graphics.DrawString(label1.Text, label1.Font, brocha, 0, 0);
      }

      private void gif_Paint(object sender, PaintEventArgs e) {
         
      }

      private void Alerta_Paint(object sender, PaintEventArgs e) {
         //Brush pincel = new SolidBrush(this.ForeColor);
         //Brush pincelSombra = new TextureBrush(Image.FromFile("recursos/fondo ojos.png"));
         //Font fuenteSombra = new Font(this.Font.FontFamily, (float)(this.Font.Size));
         ImageAnimator.UpdateFrames(this.imagen);
         //Font fuente = this.Font;
         //SizeF tamañoTexto = new SizeF(Width, Height); 
         string[] textos = _texto.Split(new char[] { '\n' });
         float posY = this.Padding.Top;
         float proporcionDpi =e.Graphics.DpiY/72;
        // using (Bitmap bmp = new Bitmap(this.imagen.Width, this.imagen.Height)) {
        //    using (Graphics gr = Graphics.FromImage(bmp)) {
        //       gr.DrawImage(this.imagen, _izquierda, _arriba, _ancho, _alto);
        //    }
        //    bmp.MakeTransparent(Color.Black);
         e.Graphics.DrawImage(this.imagen, _izquierda, _arriba, _ancho, _alto);
        // }
         foreach (string texto in textos) {
            SizeF dimensionTexto = e.Graphics.MeasureString(texto, _fuente);
            StringFormat sf = new StringFormat();
            if (texto[0] != '#') {
               //SizeF dimensionTexto = e.Graphics.MeasureString(texto, _fuente);
               int x = (int)((Width / 2) - (dimensionTexto.Width / 2));
               int y = (int)posY;

               e.Graphics.DrawString(texto, _fuenteSombra, _pincelSombra, x + this.Padding.Left - 1, y - 1);
               e.Graphics.DrawString(texto, _fuenteSombra, _pincelSombra, x + this.Padding.Left + 1, y + 1);
               System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
               Pen borde = new Pen(Color.FromArgb(0, 0, 0), 3);
               borde.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
               
               gp.AddString(texto,_fuente.FontFamily,(int)_fuente.Style,_fuente.Size*proporcionDpi, new Point(x + this.Padding.Left, y), sf);
               //e.Graphics.ScaleTransform(1.3f, 1.35f);
               e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
               
               e.Graphics.DrawPath(borde, gp);
               e.Graphics.FillPath(_pincel, gp);
               //e.Graphics.DrawString(texto, _fuente, _pincel, x + this.Padding.Left, y);
               posY += dimensionTexto.Height + separacion;
            } else {
               posY += procesarParámetro(texto,dimensionTexto.Height);
               
            }
         }
         
      }
      private float procesarParámetro(string texto,float alto) {
         float aumenta = 0;
         string[] parametros = texto.Split(new char[] { ' ' }, 2);
         switch (parametros[0]) {
            case "#F":
               string[] partes = parametros[1].Split(':');
               _fuente = new Font(partes[0], float.Parse(partes[1]));
               _pincel = new SolidBrush(Color.FromArgb(int.Parse(partes[2]), int.Parse(partes[3]), int.Parse(partes[4])));
               _fuenteSombra = new Font(partes[0], float.Parse(partes[1]));
               break;
            case "#ES":
               aumenta= (float) (alto + separacion);
               break;
         }
         return aumenta;

      }

      
   }
}
