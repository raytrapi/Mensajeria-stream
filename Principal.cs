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
      const int ESPERA_ENTRE_CONEXIONES =60;

      int numMensajes = 0;
      int ultimoId = 0;
      bool debug = false;
      static public int seguidores = 0;
      static public int espectadores = 0;
      static public int suscriptores = 0;
      static public string mensajeBienvenida = "Bienvenido al canal :D";
      Size tamañoEscritorio;

      chat.Twitch twitch;
      Alerta imagenEspectador = null;
      Alerta imagenFugaEspectador =null;
      Alerta imagenNuevoSeguidor = null;
      Alerta imagenNuevoSuscriptor = null;
      Alerta imagenBit = null;
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
         //chat.Discord.autenticacion();
         //chat.Discord.cogerMensajes("678195044817174532");
         //return;

         ultimaConexión = DateTime.Now;
         tamañoEscritorio = Screen.PrimaryScreen.WorkingArea.Size;
         this.Height = tamañoEscritorio.Height - 50;
         this.Top = tamañoEscritorio.Height - this.Height - 10;
         this.Left = 10;
         
         this.TopLevel = true;
         this.TopMost = true;

         /*JSON json = new JSON();
         
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
         /*string textoJSON = System.IO.File.ReadAllText("cheer.txt");
         json.parse(textoJSON);
         JSON json2 = new JSON();
         json2.parse(json["data"]["message"].ToString());
         //dynamic json2 = Newtonsoft.Json.Linq.JObject.Parse(textoJSON);
         //nuevoSeguidor("Soy yo");
         //nuevoSuscriptor("Nombre");
         nuevoBit(json2["data"]["user_name"].ToString(), (double)((Entidad) json2["data"]["bits_used"]).valor);/**/
         /*nuevoMensaje("1 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s.", 0.7);
         nuevoMensaje("2 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard %15940% dummy text ever since the 1500s.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s.", 0.7);
         nuevoMensaje("3 Lorem Ipsum is", 0.7);
         nuevoMensaje("4 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s.", 0.7);
         /**/
         //nuevoMensaje("2 Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard %15940% dummy text ever since the 1500s.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s.", 0.7);




         /*extensionTwitch.ExtensionTwitch extension = new extensionTwitch.ExtensionTwitch();
         Controls.Add(extension); 
         extension.Top = 200;
         extension.Left = 700;

         extension.Width = 600;
         extension.Height = 100;
         extension.Visible = true;
         extension.onRaton += moverRatonExtensionTwitch;

         seguidores = chat.Twitch.seguidores(Configuracion.parametro("id_usuario"));

         return;/**/


         /*Datos usuarios = BD.consulta("select * from usuarios where userID is null");
         for(int i = 0; i < usuarios.Length; i++) {
            JSON user = chat.Twitch.infoUsuario(usuarios[i]["alias"].ToString());
            if(user.hayClave("data") && user["data"].Length > 0) {
               BD.ejecutar("update usuarios set nombre='" + user["data"][0]["display_name"].ToString() + "', userID='" + user["data"][0]["id"].ToString() + "' where id=" + usuarios[i]["id"].ToString());
            }
         }*/


         //JSON tableroTrello = integraciones.Trello.cogerTablero("2Wu1jM2U");
         /*JSON tableroTrello = integraciones.Trello.cogerTableros("programcionextrema");
         tableroTrello = integraciones.Trello.cogerTablero("2Wu1jM2U");
         return;*/








         twitch = new chat.Twitch(Configuracion.parametro("oauth"), Configuracion.parametro("canal"));
         if (debug) {
            //MessageBox.Show("En modo depuracion");
         }
         twitch.onNuevaHora += nuevaHora;
         /*mostrarEspectadores = false;
         mostrarSeguidores = false;
         mostrarSuscriptores = false;/**/
         controlDirecto.Enabled = false;
         controlDirecto2.Enabled = false;
         espectadores = twitch.espectadores(Configuracion.parametro("id_usuario"));
         seguidores = chat.Twitch.seguidores(Configuracion.parametro("id_usuario"));
         suscriptores = chat.Twitch.suscriptores(Configuracion.parametro("id_usuario"));
         controlEspectador = new controles.Espectadores(twitch.segundosEmision);
         controlDirecto.Enabled = true;
         controlDirecto2.Enabled = true;
         Controls.Add(controlEspectador);
         controlEspectador.Visible = true;
         controlEspectador.Top = tamañoEscritorio.Height - controlEspectador.Height;
         controlEspectador.Left = 1200;//650;
         controlEspectador.espectadores = espectadores;
         controlEspectador.MouseMove +=sobreControlEspectador;
         controlEspectador.MouseLeave +=fueraControlEspectador;
         controlEspectador.MouseDown += pulsadoControlEspectador;
         controlEspectador.MouseUp += soltadoControlEspectador;
         chat.Twitch.infoUsuario("prex_directo");
         chat.Twitch.infoCanal();
         twitch.conectar(!debug);
         twitch.conectarTopicos(!debug);/**/

         //Cogemos del registro la información de bienvenida
         mensajeBienvenida = (String)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\PrexDirecto\\Mensajeria", "Bienvenida",Principal.mensajeBienvenida);

         horaLimite.Text = DateTime.Now.ToShortDateString()+" "+ DateTime.Now.ToShortTimeString();
         panelHoraLimite.Left = tamañoEscritorio.Width - panelHoraLimite.Width - 20;
         panelHoraLimite.Top = tamañoEscritorio.Height - 325;
         panelHoraLimite.Visible = false;
         //nuevoEspectador();

         //nuevoSeguidor("dd");

         //chat.Twitch.infoBits();
         //chat.Twitch.infoExtensiones();

         /*controles.ListaTareas listaTareas = new controles.ListaTareas();
         this.Controls.Add(listaTareas);
         listaTareas.añadirTarea("Instalar Mono");
         listaTareas.añadirTarea("Plantear un problema con Ionic");
         listaTareas.Left = 1000;
         listaTareas.Top = tamañoEscritorio.Height - listaTareas.Height;
         listaTareas.Visible = true;/**/
         /*listaTareas.Resize = () => {
            this.
         }*/
      }

      private void nuevaHora() {
         if (controlEspectador != null && twitch != null) {
            controlEspectador.nuevaHora = twitch.segundosEmision;
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
         if (numMensajes <= 3) {
            string limite = " limit 1";
            string parametros = (ultimoId != 0 ? " and m.id>" + ultimoId : " and m.id >= (select id from mensajes order by id desc limit 1)");

            parametros = " and idEstado<>5";

            //string parametros = (ultimoId != 0 ? " and m.id>" + ultimoId : " and m.id >= 681");
            string consulta="select m.id id, m.fecha, m.idEstado,"
               + " e.nombre, u.alias usuario, m.mensaje mensaje, u.avatar avatar, m.puntuacion puntuacion"
               + " from mensajes m, estados e, usuarios u where e.id=m.idEstado and u.id=idUsuario"
               + " and date_format(m.fecha,'%d/%m/%Y')=date_format(now(),'%d/%m/%Y')"
               + parametros
               + " order by fecha asc"
               + limite;
            Datos datos = BD.consulta(consulta);

            for (int i=0;i<datos.Length;i++) {
               Dictionary<string, object> fila = datos[i];
               if (ultimoId != 0) {
                  BD.ejecutar("update mensajes set idEstado=5 where id=" + fila["id"]);
                  nuevoMensaje((string)fila["mensaje"], (double)fila["puntuacion"], fila["avatar"].ToString(), (string)fila["usuario"]);
               }

               ultimoId = (int)fila["id"];
            }

            //comando.Connection.Close();
         }
      }
      void nuevoMensaje(string texto,double puntuacion, string avatar= "https://static-cdn.jtvnw.net/jtv_user_pictures/b170a410-b459-4154-9d5e-4f0901e58c25-profile_image-300x300.png", string usuario="YO") {
         if (numMensajes > 2) {
            Timer t = new Timer();
            t.Interval = 200;
            t.Enabled = true;
            t.Tick += (object sender, EventArgs e) => {
               if (numMensajes < 3) {
                  nuevoMensaje(texto, puntuacion, avatar, usuario);
                  t.Dispose();
               }
               borrarMensajes();
            };
            return;
         } 
         this.TopLevel = true;
         this.TopMost = true;
         Mensaje mensaje = new Mensaje();
         //mensaje.Dock = DockStyle.Bottom;
         //string mensajeTexto = texto;
         //int tiempoMuestreo = 10000 + (texto.Length > 200 ? 5000 : 0);

         if (puntuacion > 0.7) {
            mensaje.mensaje("No puedo reproducir lo que me han dicho", avatar, usuario);
            mensaje.colorFondo(Color.FromArgb(255, 0, 0));
         } else {
            mensaje.mensaje(texto, avatar, usuario);
         }
         foreach(Control control in panelMensajes.Controls) {
            control.Top -= mensaje.Height;
         }
         mensaje.Top = tamañoEscritorio.Height-mensaje.Height;
         mensaje.opacidad = 0.7;
         mensaje.onBorrar += borrarControl;
         mensaje.MouseMove += Principal_MouseMove;
         mensaje.MouseLeave += Principal_MouseLeave;
         mensaje.Resize += cambiarTamañoMensaje;
         panelMensajes.Controls.Add(mensaje);
         numMensajes++;
         
      }
      private void cambiarTamañoMensaje(object sender, EventArgs e) {
         //bool noEncontrado = true;
         int top = tamañoEscritorio.Height;
         for (int i= panelMensajes.Controls.Count-1; i>=0 ;i--) {
            Control control = panelMensajes.Controls[i];
            control.Top = top-control.Height;
            top = control.Top;
         }
      }
      /*private async Task obtenerDatosAsync() {
         HttpClient cliente = new HttpClient();
         //liente.
         HttpResponseMessage info= await cliente.GetAsync("http://localhost/mensajes");
         
         info.Content
         
      }/**/
      void borrarMensajes() {
         bool noEncontrado = true;
         for (int i = 0; i < panelMensajes.Controls.Count && noEncontrado; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               Invalidate();
               noEncontrado = false;
               Mensaje m = (Mensaje)panelMensajes.Controls[i];
               if (m.borrado) {
                  panelMensajes.Controls.Remove(m);
                  numMensajes--;
               }
            }
         }
      }
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
         //((Mensaje)sender).opacidad = 1;
         for (int i = 0; i < panelMensajes.Controls.Count; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               ((Mensaje)panelMensajes.Controls[i]).detenerReloj();
            }
         }
      }

      private void Principal_MouseLeave(object sender, EventArgs e) {
         //((Mensaje)sender).opacidad = 0.8;
         for (int i = 0; i < panelMensajes.Controls.Count; i++) {
            if (panelMensajes.Controls[i].GetType().Name == "Mensaje") {
               ((Mensaje)panelMensajes.Controls[i]).reanudarReloj();
            }
         }
      }

      private void Principal_MouseUp(object sender, MouseEventArgs e) {
         //this.Opacity = 1;

         
         
      }

      private void controlDirecto_Tick(object sender, EventArgs e) {
         controlDirecto.Enabled = false;
         try {
            if (!twitch.estaConectado && !twitch.estaConectando) {
               twitch.conectar(!debug);
            }
               
         }catch{

         }
         try {



            int controlEspectadores = twitch.espectadores(Configuracion.parametro("id_usuario"));
            
            int controlSuscriptores = chat.Twitch.suscriptores(Configuracion.parametro("id_usuario"));

            if (controlEspectadores != espectadores) {
               if (controlEspectadores > espectadores) {
                  if (mostrarEspectadores) {
                     //nuevoEspectador(controlEspectadores);
                     controlEspectador.nuevo = true;
                     nuevoEspectador();
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
            
            if (controlSuscriptores > suscriptores && (imagenNuevoSuscriptor == null || !imagenNuevoSuscriptor.Visible)) {
               if (mostrarSuscriptores) {
                  nuevoSuscriptor(chat.Twitch.listaSuscriptores[chat.Twitch.listaSuscriptores.Count - suscriptores - 1].ToString());
               }
               suscriptores++;
            } else if (controlSuscriptores < suscriptores) {
               suscriptores = controlSuscriptores;
            }
         } catch {

         }
         if(nuevaConexión && DateTime.Now.Subtract(ultimaConexión).TotalSeconds > ESPERA_ENTRE_CONEXIONES) {
            if (!debug) {
               //twitch.mensaje = mensajeBienvenida;
            }
            nuevaConexión = false;
         }
         if (chat.Twitch.nuevosSeguidores.Count > 0) {
            if (mostrarSeguidores) {
               string seguidor = chat.Twitch.nuevosSeguidores[0];
               chat.Twitch.nuevosSeguidores.RemoveAt(0);
               nuevoSeguidor(seguidor);
            }
         }
         nuevaHora();
         controlDirecto.Enabled = true;
      }
      private void controlDirecto2_Tick(object sender, EventArgs e) {
         controlDirecto2.Enabled = false;
         int controlSeguidores = chat.Twitch.seguidores(Configuracion.parametro("id_usuario"));
         
         if (controlSeguidores > seguidores && (imagenNuevoSeguidor == null || !imagenNuevoSeguidor.Visible)) {
            
            seguidores++;
         } else if (controlSeguidores < seguidores) {
            seguidores = controlSeguidores;
         }
         controlDirecto2.Enabled = true;

      }
      private void nuevoEspectador(){//int espectadores) {
         /*try {
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

         }/**/
         if (!marquesina.Visible) {
            Timer temporizadorMarquesina = new Timer();
            //string titulo = twitch.titulo;
            marquesina.Text = twitch.titulo;
            marquesina.Left = tamañoEscritorio.Width;
            marquesina.Top = tamañoEscritorio.Height - marquesina.Height;
            //double velocidad=
            /*temporizadorMarquesina.Interval = 1;//(int)(2000/(tamañoEscritorio.Width + marquesina.Width));1000/30
            temporizadorMarquesina.Enabled = true;
            temporizadorMarquesina.Tick += (object sender, EventArgs e) => {
                  //System.Diagnostics.Trace.WriteLine(marquesina.Left+">"+ -marquesina.Width);
                  if (marquesina.Left > -marquesina.Width) {
                     marquesina.Left-=5;
                  } else {
                     marquesina.Visible = false;
                     ((Timer)sender).Dispose();
                  }
               };
            marquesina.Visible = true;/**/
         }
      }
      private void fugaEspectador(int espectadores) {
         try {
            Alerta imagen = imagenFugaEspectador;
            if (imagen == null) {
               imagen = new Alerta();
               imagen.urlImagen = Configuracion.parametro("imagen_fuga_espectador");
               int w = 300;
               int h = 200;
               imagen.ancho = w;
               imagen.alto = h;
               imagen.tiempo = 4100;
               //imagen.localizacion = new Point(((Width / 2) - (imagen.Width / 2)) + ((imagen.Width / 2)+20), 0);
               imagen.localizacion = new Point(panelMensajes.Width + 10 + w+10, tamañoEscritorio.Height - h - 20);
               Controls.Add(imagen);
            }
            imagen.mostrar("VAYA!! ahora somos " + espectadores.ToString() + "");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
      }
      private void nuevoSeguidor(string usuario) {
         try {
            if (imagenNuevoSeguidor == null) {
               imagenNuevoSeguidor = crearAlerta("seguidor");
            }
            Controls.Add(imagenNuevoSeguidor);
            //Lobster
            //Dancing Script
            imagenNuevoSeguidor.mostrar("#F Lobster:50:198:255:193\n" + usuario+ "\n#F Roboto:20:255:255:255\nse ha unido\n#F Dancing Script:30:255:255:255\nBienvenido");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
         
      }
      private void nuevoBit(string usuario, double bits) {
         try {
            //Alerta imagen = imagenBit;
            if (imagenBit == null) {
               imagenBit = crearAlerta("bit");
            }
            Controls.Add(imagenBit);
            //Lobster
            //Dancing Script
            String texto = Configuracion.parametro("bit.texto", "");
            texto = texto.Replace("%USUARIO%", usuario).Replace("%BITS%",bits.ToString()).Replace("\\n","\n");
            imagenBit.mostrar(texto);
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }

      }
      Alerta crearAlerta(string alias) {
         try {
            Alerta imagen = new Alerta();
            
            //imagen = new Alerta();
            imagen.urlImagen = Configuracion.parametro(alias+".imagen");
            int w = int.Parse(Configuracion.parametro(alias + ".w", "400"));
            imagen.ancho = w;
            string hS = Configuracion.parametro(alias + ".h");
            string x = Configuracion.parametro(alias + ".x");
            string y = Configuracion.parametro(alias + ".y");
            int h = 0;
            if (hS != "") {

               h = int.Parse(hS);
            } else {
               h = (int)(((double)imagen.sizeImagen.Width / (double)imagen.sizeImagen.Height) * (double)w); ;
            }
            imagen.alto = h;
            imagen.tiempo = int.Parse(Configuracion.parametro(alias + ".tiempo","6000"));
            /*imagen.localizacion = new Point(
               (x!=""?int.Parse(x):((Width / 2) - (imagen.Width / 2))), 
               (y!=""?int.Parse(y):tamañoEscritorio.Height - 280 - imagen.Height));/**/
            imagen.fuente = new Font("Roboto", 30);
            imagen.Padding = new Padding(imagen.Padding.Left, int.Parse(Configuracion.parametro(alias + ".padding.top", "50")), imagen.Padding.Right, imagen.Padding.Bottom);
            if (x != "") {
               imagen.Left = int.Parse(x);
               if (imagen.Left < 0) {
                  imagen.Left = tamañoEscritorio.Width - imagen.Width - imagen.Padding.Right;
               } else {
                  imagen.Left += imagen.Padding.Left;
               }
            } else {
               imagen.Left = tamañoEscritorio.Width - imagen.Width - imagen.Padding.Right;
            }
            if (y != "") {
               imagen.Top = int.Parse(y);
               if (imagen.Top < 0) {
                  imagen.Top = tamañoEscritorio.Height - imagen.Height - imagen.Padding.Bottom;
               } else {
                  imagen.Top += imagen.Padding.Top;
               }
            } else {
               imagen.Top = tamañoEscritorio.Height - imagen.Height - imagen.Padding.Bottom;
            }
            imagen.Top -= imagen.Margin.Bottom;
            return imagen;
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }
         return null;
      }
      private void nuevoSuscriptor(string usuario) {
         try {
            if (imagenNuevoSuscriptor == null) {
               imagenNuevoSuscriptor = crearAlerta("suscriptor");
            }
            Controls.Add(imagenNuevoSuscriptor);
            imagenNuevoSuscriptor.mostrar("#F Lobster:50:255:190:190\n" + usuario + "\n#F Lobster:30:255:255:255\nse ha suscrito");
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }

      }

      private void mensajeDeBienvenidaToolStripMenuItem_Click(object sender, EventArgs e) {
         Bienvenida mensaje = new Bienvenida();
         mensaje.ShowDialog();
         //twitch.mensaje = mensajeBienvenida;
      }

      private void Principal_FormClosing(object sender, FormClosingEventArgs e) {
         botonNotificacion.Visible = false;
      }

      private void timer1_Tick(object sender, EventArgs e) {
        /*string mensaje = chat.Gitter.leerMensaje(Configuracion.parametro("gitter_sala"));
        if (mensaje.Length > 0) {
            nuevoMensaje(mensaje, 0);
        }/**/
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
      private void sobreControlEspectador(object sender, EventArgs e) {
         //((Control)sender).Top = tamañoEscritorio.Height-5;
      }
      private void fueraControlEspectador(object sender, EventArgs e) {

         //((Control)sender).Top = tamañoEscritorio.Height - ((Control)sender).Height;
      }
      private void pulsadoControlEspectador(object sender, EventArgs e) {
         //((Control)sender).Top = tamañoEscritorio.Height - 5;
      }
      private void soltadoControlEspectador(object sender, EventArgs e) {

         //((Control)sender).Top = tamañoEscritorio.Height - ((Control)sender).Height;
      }


      PictureBox fantasma = null;
      private void moverRatonExtensionTwitch(double x, double y) {
         if (fantasma == null) {
            fantasma = new PictureBox();
            fantasma.Image = Image.FromFile("recursos/fantasma.png");
            fantasma.Width = 64;
            fantasma.Height = 64;
            fantasma.Visible = true;
            
            Controls.Add(fantasma);
            Controls.SetChildIndex(fantasma, 0);
         }
         fantasma.Top = (int)(y*tamañoEscritorio.Height);
         fantasma.Left = (int)(x*tamañoEscritorio.Width);
         /*this.Cursor = new Cursor(Cursor.Current.Handle);
         Cursor.Position = new Point((int)x,(int)y);*/
      }

      bool estoyEnGrande = false;
      private void button1_Click(object sender, EventArgs e) {
         if (!estoyEnGrande) {
            //chat.OBS.enviarComando("CE Camara en grande");
            chat.OBS.enviarComando("CF Texto:true");
         } else {
            //chat.OBS.enviarComando("CE Escena");
            chat.OBS.enviarComando("CF Texto:false");
         }
         estoyEnGrande = !estoyEnGrande;
      }
   }
}
