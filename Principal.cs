using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
//using MySql.Data;
using System.Configuration;

namespace Mensajería {
   public partial class Principal : Form {
      const int ESPERA_ENTRE_CONEXIONES =10;

      int numMensajes = 0;
      int ultimoId = 0;
      bool debug = false;
      static public int seguidores = 0;
      static public int espectadores = 0;
      static public int suscriptores = 0;
      static public string mensajeBienvenida = "Bienvenido al canal :D";
      Size tamañoEscritorio;
      chat.IRCTwicth irc;
      Alerta imagenEspectador = null;
      Alerta imagenFugaEspectador =null;
      Alerta imagenNuevoSeguidor = null;
      Alerta imagenNuevoSuscriptor = null;
      bool mostrarEspectadores = true;
      bool mostrarSeguidores = true;
      bool mostrarSuscriptores = true;
      controles.Espectadores controlEspectador;

      bool nuevaConexión = false;
      DateTime ultimaConexión;
      //MySql.Data.MySqlClient.MySqlConnection conexion = null;

      public Principal(bool debug=false) {
         InitializeComponent();
         this.debug = debug;
         this.TransparencyKey = Color.FromArgb(1,1,1);
         this.BackColor = Color.FromArgb(1, 1, 1);

         /*if (conexion == null) {
            conexion = new MySql.Data.MySqlClient.MySqlConnection("Database="+System.Configuration.ConfigurationManager.AppSettings["base_datos"] +";Data Source="+ System.Configuration.ConfigurationManager.AppSettings["servidor"] + ";User Id="+ System.Configuration.ConfigurationManager.AppSettings["usuario"]+ ";Password="+ System.Configuration.ConfigurationManager.AppSettings["clave"]);
            //conexion.Open();
         }/**/
         ultimaConexión = DateTime.Now;
         tamañoEscritorio = Screen.PrimaryScreen.WorkingArea.Size;
         this.Height = tamañoEscritorio.Height - 50;
         this.Top = tamañoEscritorio.Height - this.Height - 10;
         this.Left = 10;
         
         this.TopLevel = true;
         this.TopMost = true;

         //JSON json = new JSON();
         /*
          {
             "primero " :"1",
            "segundo":2,
            "tercero":[
               {
                  "3.1":4
               },
               {
                  "3.2":{
                     "3.2.1":"final"
                  }
                }
            ]
          }

          * */
         //json.parse("{ \"primero \" :\"1\",\"segundo\":2,\"tercero\":[{\"3.1\":4},{\"3.2\":{\"3.2.1\":\"final\"}}]}");
         //json.getJson("http://laravel/prueba.json",new System.Collections.ArrayList());
         //MessageBox.Show(((Entidad)(json["tercero"][1]["3.2"]))["3.2.1"].ToString());

         irc = new chat.IRCTwicth(Configuracion.parametro("oauth"), Configuracion.parametro("canal"));
         if (debug) {
            //MessageBox.Show("En modo depuracion");
         }
         irc.conectar(!debug);/**/
         irc.onNuevaHora += nuevaHora;
         /*mostrarEspectadores = false;
         mostrarSeguidores = false;
         mostrarSuscriptores = false;/**/
         espectadores = irc.espectadores(Configuracion.parametro("id_usuario"));
         seguidores = chat.IRCTwicth.seguidores(Configuracion.parametro("id_usuario"));
         suscriptores = chat.IRCTwicth.suscriptores(Configuracion.parametro("id_usuario"));
         controlEspectador = new controles.Espectadores(irc.segundosEmision);
         Controls.Add(controlEspectador);
         controlEspectador.Visible = true;
         controlEspectador.Top = tamañoEscritorio.Height - controlEspectador.Height;
         controlEspectador.Left = 650;
         controlEspectador.espectadores = espectadores;

         //Cogemos del registro la información de bienvenida
         mensajeBienvenida=(String)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\PrexDirecto\\Mensajeria", "Bienvenida",Principal.mensajeBienvenida);

         horaLimite.Text = DateTime.Now.ToShortDateString()+" "+ DateTime.Now.ToShortTimeString();
         panelHoraLimite.Left = tamañoEscritorio.Width - panelHoraLimite.Width - 20;
         panelHoraLimite.Top = tamañoEscritorio.Height - 325;
         panelHoraLimite.Visible = false;

      }

      private void nuevaHora() {
         if (controlEspectador != null && irc != null) {
            controlEspectador.nuevaHora = irc.segundosEmision;
         }
      }

      ~Principal() {
         //conexion.Close();
      }
      private void CerrarToolStripMenuItem_Click(object sender, EventArgs e) {
         this.Close();
      }

      private void Agregador_Tick(object sender, EventArgs e) {
         //obtenerDatosAsync();
         obtenerDatosMySQL();
      }
      private void obtenerDatosMySQL() {
         if (numMensajes < 3) {
            string limite = " limit 1";
            string parametros = (ultimoId != 0 ? " and m.id>" + ultimoId : " and m.id >= (select id from mensajes order by id desc limit 1)");
            //string parametros = (ultimoId != 0 ? " and m.id>" + ultimoId : " and m.id >= 681");
            string consulta="select m.id id, m.fecha, m.idEstado,"
               + " e.nombre, u.nombre usuario, m.mensaje mensaje, u.avatar avatar, m.puntuacion puntuacion"
               + " from mensajes m, estados e, usuarios u where e.id=m.idEstado and u.id=idUsuario"
               + " and date_format(m.fecha,'%d/%m/%Y')=date_format(now(),'%d/%m/%Y')"
               + parametros
               + " order by fecha asc"
               + limite;
            Datos datos = BD.consulta(consulta);

            for (int i=0;i<datos.Length;i++) {
               Dictionary<string, object> fila = datos[i];
               //if (ultimoId != 0) {
               nuevoMensaje((string)fila["mensaje"], (double)fila["puntuacion"], fila["avatar"].ToString(), (string)fila["usuario"]);
               //}

               ultimoId = (int)fila["id"];
            }

            //comando.Connection.Close();
         }
      }
      void nuevoMensaje(string texto,double puntuacion, string avatar= "https://static-cdn.jtvnw.net/jtv_user_pictures/b170a410-b459-4154-9d5e-4f0901e58c25-profile_image-300x300.png", string usuario="YO") {
         this.TopLevel = true;
         this.TopMost = true;
         Mensaje mensaje = new Mensaje();
         mensaje.Dock = DockStyle.Bottom;
         //string mensajeTexto = texto;
         //int tiempoMuestreo = 10000 + (texto.Length > 200 ? 5000 : 0);

         if (puntuacion> 0.7) {
            mensaje.mensaje("No puedo reproducir lo que me han dicho", avatar, usuario);
            mensaje.colorFondo(Color.FromArgb(255, 0, 0));
         } else {
            mensaje.mensaje(texto, avatar, usuario);
         }
         mensaje.onBorrar += borrarControl;
         mensaje.MouseMove += Principal_MouseMove;
         mensaje.MouseLeave += Principal_MouseLeave;
         panelMensajes.Controls.Add(mensaje);
         numMensajes++;
      }
      /*private async Task obtenerDatosAsync() {
         HttpClient cliente = new HttpClient();
         //liente.
         HttpResponseMessage info= await cliente.GetAsync("http://localhost/mensajes");
         
         info.Content
         
      }/**/
      void borrarControl(Mensaje control) {
         for(int i = 0; i < panelMensajes.Controls.Count; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               Mensaje m = (Mensaje)panelMensajes.Controls[i];
               if (m == control) {
                 panelMensajes.Controls.Remove(m);
                  numMensajes--;
               }
            }
         }
      }

      private void Principal_MouseMove(object sender, MouseEventArgs e) {
         this.Opacity = 1;
         for (int i = 0; i < panelMensajes.Controls.Count; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               ((Mensaje)panelMensajes.Controls[i]).detenerReloj();
            }
         }
      }

      private void Principal_MouseLeave(object sender, EventArgs e) {
         this.Opacity = 0.8;
         for (int i = 0; i < panelMensajes.Controls.Count; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               ((Mensaje)panelMensajes.Controls[i]).reanudarReloj();
            }
         }
      }

      private void Principal_MouseUp(object sender, MouseEventArgs e) {
         this.Opacity = 1;

         
         
      }

      private void controlDirecto_Tick(object sender, EventArgs e) {
         try {
            if (!irc.estaConectado && !irc.estaConectando) {
               irc.conectar(!debug);
            }
               
         }catch{

         }
         try {



            int controlEspectadores = irc.espectadores(Configuracion.parametro("id_usuario"));
            int controlSeguidores = chat.IRCTwicth.seguidores(Configuracion.parametro("id_usuario"));
            int controlSuscriptores = chat.IRCTwicth.suscriptores(Configuracion.parametro("id_usuario"));

            if (controlEspectadores != espectadores) {
               if (controlEspectadores > espectadores) {
                  if (mostrarEspectadores) {
                     //nuevoEspectador(controlEspectadores);
                     controlEspectador.nuevo = true;
                  }
                  if (!debug) {
                     nuevaConexión = true;
                  }
               } else {
                  if (mostrarEspectadores) {
                     controlEspectador.nuevo = false;
                     //fugaEspectador(controlEspectadores);
                  }
               }
            }
            espectadores = controlEspectadores;
            controlEspectador.espectadores = espectadores;
            if (controlSeguidores > seguidores && (imagenNuevoSeguidor == null || !imagenNuevoSeguidor.Visible)) {
               if (mostrarSeguidores) {
                  nuevoSeguidor(chat.IRCTwicth.listaSeguidores[chat.IRCTwicth.listaSeguidores.Count - seguidores - 1].ToString());
               }
               seguidores++;
            }else if(controlSeguidores < seguidores) {
               seguidores = controlSeguidores;
            }
            if (controlSuscriptores > suscriptores && (imagenNuevoSuscriptor == null || !imagenNuevoSuscriptor.Visible)) {
               if (mostrarSuscriptores) {
                  nuevoSuscriptor(chat.IRCTwicth.listaSuscriptores[chat.IRCTwicth.listaSuscriptores.Count - suscriptores - 1].ToString());
               }
               suscriptores++;
            } else if (controlSuscriptores < suscriptores) {
               suscriptores = controlSuscriptores;
            }
         } catch {

         }
         if(nuevaConexión && DateTime.Now.Subtract(ultimaConexión).TotalSeconds > ESPERA_ENTRE_CONEXIONES) {
            if (!debug) {
               irc.mensaje = mensajeBienvenida;
            }
            nuevaConexión = false;
         }
      }
      private void nuevoEspectador(int espectadores) {
         try {
            Alerta imagen = imagenEspectador;
            if (imagen == null) {
               imagen = new Alerta();
               imagen.urlImagen = Configuracion.parametro("imagen_nuevo_espectador");
               imagen.ancho = 300;
               imagen.alto = 200;
               imagen.tiempo = 4100;
               //imagen.localizacion = new Point(((Width / 2) - (imagen.Width/2)) - ((imagen.Width / 2) + 20), 0);
               imagen.localizacion = new Point(panelMensajes.Width+10, tamañoEscritorio.Height - imagen.alto - 20);
               Controls.Add(imagen);
            }
            imagen.mostrar("Nuevo espectador ["+espectadores.ToString()+"]");
         }catch(Exception ex){
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
      }
      private void fugaEspectador(int espectadores) {
         try {
            Alerta imagen = imagenFugaEspectador;
            if (imagen == null) {
               imagen = new Alerta();
               imagen.urlImagen = Configuracion.parametro("imagen_fuga_espectador");
               imagen.ancho = 300;
               imagen.alto = 200;
               imagen.tiempo = 4100;
               //imagen.localizacion = new Point(((Width / 2) - (imagen.Width / 2)) + ((imagen.Width / 2)+20), 0);
               imagen.localizacion = new Point(panelMensajes.Width + 10 + imagen.ancho+10, tamañoEscritorio.Height - imagen.alto - 20);
               Controls.Add(imagen);
            }
            imagen.mostrar("VAYA!! ahora somos " + espectadores.ToString() + "");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
      }
      private void nuevoSeguidor(string usuario) {
         try {
            Alerta imagen = imagenNuevoSeguidor;
            if (imagen == null) {
               imagen = new Alerta();
               imagen.urlImagen = Configuracion.parametro("imagen_nuevo_seguidor");
               imagen.ancho = 400;
               imagen.alto = 250;
               imagen.tiempo = 6000;
               imagen.localizacion = new Point(((Width / 2) - (imagen.Width / 2)), tamañoEscritorio.Height - 280 - imagen.alto);
               imagen.fuente = new Font("Roboto", 22);
               Controls.Add(imagen);
            }
            imagen.mostrar(usuario+"\r\n se ha unido. \r\nHOLA");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
         
      }
      private void nuevoSuscriptor(string usuario) {
         try {
            Alerta imagen = imagenNuevoSeguidor;
            if (imagen == null) {
               imagen = new Alerta();
               imagen.urlImagen = Configuracion.parametro("imagen_nuevo_suscriptor");
               imagen.ancho = 400;
               imagen.alto = 250;
               imagen.tiempo = 6000;
               imagen.localizacion = new Point(((Width / 2)  - (imagen.Width / 2))+390, tamañoEscritorio.Height - 280 - imagen.alto);
               imagen.fuente = new Font("Roboto", 22);
               Controls.Add(imagen);
            }
            imagen.mostrar(usuario + "\r\n se ha suscrito al canal");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }

      }

      private void mensajeDeBienvenidaToolStripMenuItem_Click(object sender, EventArgs e) {
         Bienvenida mensaje = new Bienvenida();
         mensaje.ShowDialog();
         //irc.mensaje = mensajeBienvenida;
      }

      private void Principal_FormClosing(object sender, FormClosingEventArgs e) {
         botonNotificacion.Visible = false;
      }

      private void timer1_Tick(object sender, EventArgs e) {
         string mensaje = chat.Gitter.leerMensaje(Configuracion.parametro("gitter_sala"));
         if (mensaje.Length > 0) {
            nuevoMensaje(mensaje, 0);
         }
      }

      private void horaLímiteToolStripMenuItem_Click(object sender, EventArgs e) {
         panelHoraLimite.Visible = true;
         horaLimite.Focus();
      }

      private void horaLimite_Leave(object sender, EventArgs e) {
         panelHoraLimite.Visible = false;
         DateTime fechaLimite;
         if(DateTime.TryParse(horaLimite.Text, out fechaLimite)) {
            controlEspectador.horaLimite = fechaLimite.Subtract(DateTime.Now).TotalSeconds;
         }
      }
   }
}
