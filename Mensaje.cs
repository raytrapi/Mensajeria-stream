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
      private double _opacidad = 0.5;
      private double _opacidadAnterior = -1;
      public double opacidad {
         get {
            return _opacidad;
         }
         set {
            if (value < 0) {
               _opacidad = 0;
            } else if (value > 1) {
               _opacidad = 1;
            } else {
               _opacidad = value;
            }
            usuario.ForeColor = Color.FromArgb((int)(255 * _opacidad), usuario.ForeColor.R, usuario.ForeColor.G, usuario.ForeColor.B);
            this.avatar.Image = opacidadImagen(_avatar, _opacidad);
            _pincelFondo = new SolidBrush(Color.FromArgb((int)(255 * (_opacidad / _divisorFondo)), _colorFondo.R, _colorFondo.G, _colorFondo.B));
            Invalidate();
         }
      }
      private bool _borrado = false;
      public bool borrado {
         get {
            return _borrado;
         }
      }

      public Mensaje() {
         InitializeComponent();
      }
      int altoMinimo = 200;
      string _mensaje;
      string _usuario;
      Bitmap _avatar;
      Color _colorFondo = Color.FromArgb(0, 0, 10);
      double _divisorFondo = 1;
      Brush _pincelFondo;
      string[] palabras;
      int altoVentanaAnterior = -1;
      int anchoVentanaAnterior = -1;
      int _altoReducido = 20;
      int _anchoReducido = 20;
      public void mensaje(string mensaje, string avatar, string usuario, int temporizar = 10000) {
         altoMinimo = this.Height;
         //this.texto.Text = mensaje;
         _mensaje = mensaje;
         texto.Text = "";
         _usuario = usuario;
         if (avatar.Length == 0) {

         } else {
            //byte[] imageData = System.Net.DownloadData(avatar); //DownloadData function from here
            //MemoryStream stream = new MemoryStream(imageData);
            System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(avatar));
            ;
            System.Net.HttpWebResponse imagen = (System.Net.HttpWebResponse)solicitud.GetResponse();
            _avatar = (Bitmap)Bitmap.FromStream(imagen.GetResponseStream());
            this.avatar.Image = opacidadImagen(_avatar, _opacidad);

         }
         //stream.Close();
         this.usuario.Text = "";

         //var size = TextRenderer.MeasureText(text, font, new Size(width, height), TextFormatFlags.WordBreak);
         Size tamañoTexto = TextRenderer.MeasureText(this.texto.Text, this.texto.Font, new Size(this.texto.Width, 1000), TextFormatFlags.WordBreak);
         this.texto.Height = tamañoTexto.Height;
         this.Height = this.texto.Height + this.Margin.Top + this.Margin.Bottom + this.usuario.Height;
         if (this.Height < altoMinimo) {
            this.Height = altoMinimo;
         }
         _pincelFondo = new SolidBrush(Color.FromArgb((int)(255 * (_opacidad / _divisorFondo)), _colorFondo.R, _colorFondo.G, _colorFondo.B));
         //this.usuario.Font.

         //destructor.Interval = temporizar;

         palabras = _mensaje.Split(' ');
         //300 palabras por minuto
         destructor.Interval = ((int)(((double)palabras.Length / 200) * 60) * 1000) + 2000;
         this.DoubleBuffered = true;
         calcularAlto();
         controlarRaton(this);
         
      }
      private void controlarRaton(Control control) {
         if (control.Controls!=null) {
            for (int i = 0; i < control.Controls.Count; i++) {
               control.Controls[i].MouseMove += mouseMove;
               control.Controls[i].MouseLeave += mouseLeave;
               controlarRaton(control.Controls[i]);

            }
         }
      }
      public void colorFondo(Color color) {
         this.BackColor = color;
      }

      public delegate void eventoBorrar(Mensaje control);
      public event eventoBorrar onBorrar;

      private void Destructor_Tick(object sender, EventArgs e) {
         if (_opacidad > 0.08) {
            opacidad = _opacidad - 0.01;
            destructor.Interval = 10;
         } else {/**/
            //altoVentanaAnterior = Height;
            this.Height = _altoReducido;
            this.Width = _anchoReducido;
            //Left = (-Width) + 20;
            //Left = 50;
            _borrado = true;
            destructor.Enabled = false;
            Invalidate();
            //Refresh();
            /*eventoBorrar borrar = onBorrar;
            borrar?.Invoke(this);
            destructor.Interval = 1000;*/
         }

      }
      bool encima = false;
      bool saliendo = true;
      private void mouseMove(object sender, MouseEventArgs e) {
         bool pintar = false;
         if (!encima) {
            if (_opacidadAnterior == -1) {
               _opacidadAnterior = _opacidad;
               opacidad = 1;
               pintar = true;
            }
            if (_borrado && Height == _altoReducido) {
               /*Height = altoVentanaAnterior+Height;
               altoVentanaAnterior = Height-altoVentanaAnterior;
               Height += altoVentanaAnterior;*/
               Height = altoVentanaAnterior;
               Width = anchoVentanaAnterior;
               pintar = true;
            }
            if (pintar) {
               Invalidate();
            }

            this.OnMouseMove(e);
            encima = true;
         }
         saliendo = false;
      }

      private void mouseLeave(object sender, EventArgs e) {
         Timer t = new Timer();
         t.Interval = 1500;
         t.Enabled = true;
         t.Tick += (object _sender, EventArgs _e) => {
            if (saliendo) {
               bool pintar = false;
               if (_opacidadAnterior != -1) {
                  opacidad = _opacidadAnterior;
                  _opacidadAnterior = -1;
                  pintar = true;
               }
               if (_borrado) {
                  Height = _altoReducido;
                  Width = _anchoReducido;
                  pintar = true;
               }
               if (pintar) {
                  Invalidate();
               }
               
               this.OnMouseLeave(e);
            }
            t.Dispose();
         };
         saliendo = true;
         encima = false;

      }

      public void detenerReloj() {
         if (!_borrado) {
            destructor.Enabled = false;
            //_noBorrado = true;
         }

      }
      public void reanudarReloj() {
         if (!_borrado) {
            destructor.Enabled = true;
            if (destructor.Interval > 1) {
               //destructor.Interval /= 2;
            }
         }
      }
      private void calcularAlto() {
         int ancho = texto.Width - texto.Padding.Left - texto.Padding.Right;
         string fraseActual = "";
         float tamañoFrase = 0;
         Graphics g = this.CreateGraphics();
         float tamañoEspacio = g.MeasureString(" ", texto.Font).Width;
         float tamañoLinea = g.MeasureString(" ", texto.Font).Height;
         Font fuente = texto.Font;
         int frases = 0;
         foreach (string palabra in palabras) {
            float tamañoPalabra = g.MeasureString(palabra, texto.Font).Width;
            if ((tamañoFrase + tamañoEspacio + tamañoPalabra) < ancho) {
               fraseActual += (tamañoFrase > 0 ? " " : "") + palabra;
               tamañoFrase += (tamañoFrase > 0 ? tamañoEspacio : 0) + tamañoPalabra;
            } else {
               frases++;
               fraseActual = palabra;
               tamañoFrase = tamañoPalabra;
            }
         }
        // if (fraseActual.Length > 0) {
            frases+=2;
         //}
         int tamañoVentana = (int)((frases) * tamañoLinea) + this.Padding.Bottom;
         if (tamañoVentana > this.Height) {
            this.Height = tamañoVentana;
         }
         Height += this.Margin.Top + this.Margin.Bottom;
         altoVentanaAnterior = Height;
         anchoVentanaAnterior = Width;
      }
      private void texto_Paint(object sender, PaintEventArgs e) {

         int ancho = texto.Width - texto.Padding.Left - texto.Padding.Right;
         List<string> frases = new List<string>();
         string fraseActual = "";
         float tamañoFrase = 0;
         float tamañoEspacio = e.Graphics.MeasureString(" ", texto.Font).Width;
         Brush pincel = new SolidBrush(Color.FromArgb((int)(255 * _opacidad), texto.ForeColor.R, texto.ForeColor.G, texto.ForeColor.B));
         Font fuente = texto.Font;
         int y = 0;
         int x = 0;
         float tamañoLinea = e.Graphics.MeasureString(" ", fuente).Height;
         foreach (string palabra in palabras) {
            bool soyPalabra = true;
            Image emoticono = null;
            if (palabra[0] == '%' && palabra[palabra.Length - 1] == '%') {
               string consulta = "select * from emoticonos where id="+palabra.Substring(1, palabra.Length - 2) + "";
               Datos datos = BD.consulta(consulta);
               if (datos.Length > 0) {
                  try {
                     System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(datos[0]["url"].ToString()));
                     ;
                     System.Net.HttpWebResponse imagen = (System.Net.HttpWebResponse)solicitud.GetResponse();
                     emoticono = Image.FromStream(imagen.GetResponseStream());

                  } catch { }
               }
               soyPalabra = false;
            }/**/

            float tamañoPalabra = soyPalabra ? e.Graphics.MeasureString(palabra, texto.Font).Width : 28 ;
            if ((x + tamañoEspacio + tamañoPalabra) < ancho) {
               if (soyPalabra) {
                  e.Graphics.DrawString((tamañoFrase > 0 ? " " : "") + palabra, fuente, pincel, x, y);
               } else {
                  if (emoticono != null) {
                     e.Graphics.DrawImage(emoticono, x, y);
                  }
               }
               x += (int)((tamañoFrase > 0 ? tamañoEspacio : 0) + tamañoPalabra);
               
            } else {
               x = 0;
               y += (int)tamañoLinea;
               if (soyPalabra) {
                  e.Graphics.DrawString((tamañoFrase > 0 ? " " : "") + palabra, fuente, pincel, x, y);
               } else {
                  if (emoticono != null) {
                     e.Graphics.DrawImage(emoticono, x, y);
                  }
               }
               x += (int)((tamañoFrase > 0 ? tamañoEspacio : 0) + tamañoPalabra);
            }
         }
         
         
         /**/
      }

      private void Mensaje_Paint(object sender, PaintEventArgs e) {
         RectangleF area = ClientRectangle;
         if (_borrado) {
            //return;
         }
         
         area.X += this.Margin.Left;
         area.Width = (_borrado?Width:area.Width)- this.Margin.Left - this.Margin.Right;
         area.Y += this.Margin.Top;
         area.Height -= this.Margin.Top - this.Margin.Bottom;
         e.Graphics.FillRectangle(_pincelFondo, area);
         //System.Console.WriteLine("jj");
         /**/
      }

      private void usuario_Paint(object sender, PaintEventArgs e) {
         Brush pincel = new SolidBrush(Color.FromArgb((int)(255 * _opacidad), usuario.ForeColor.R, usuario.ForeColor.G, usuario.ForeColor.B));
         e.Graphics.DrawString(_usuario, usuario.Font, pincel, usuario.Padding.Left, usuario.Padding.Top);
         /**/
      }

      /*FUNCIÓN COGIDA DE INTERNET Y MODIFICADA : https://stackoverflow.com/questions/44749869/picturebox-slider-control-transparency*/
      Bitmap opacidadImagen(Bitmap imagen, double opacidad) {
         try {
            Bitmap imagenTemporal = new Bitmap(imagen.Width, imagen.Height);
            Rectangle r = new Rectangle(0, 0, imagen.Width, imagen.Height);

            float[][] matrixItems = {
              new float[] {1, 0, 0, 0, 0},
              new float[] {0, 1, 0, 0, 0},
              new float[] {0, 0, 1, 0, 0},
              new float[] {0, 0, 0, (float)opacidad, 0},
              new float[] {0, 0, 0, 0, 1}};

            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(matrixItems);

            System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(imagenTemporal))
               g.DrawImage(imagen, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return imagenTemporal;
         }catch{
            return imagen;
         }
      }

   }   
}
