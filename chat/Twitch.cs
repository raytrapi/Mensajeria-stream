using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajería.chat {
   class Twitch {
      private string oauth;
      private string canal;
      private System.Net.WebSockets.ClientWebSocket cliente;
      private System.Threading.CancellationToken cancellationToken;
      private System.Threading.CancellationToken cancellationToken2;
      private System.Net.WebSockets.ClientWebSocket clienteTopic;
      static public ArrayList listaSeguidores = new ArrayList();
      static public ArrayList listaSuscriptores = new ArrayList();
      private List<string> mensajesEnviar = new List<string>();
      private static DateTime horaEmision;
      private static string _titulo="";
      static bool seguidoresControlados = false;

      public Twitch(string oauth, string canal) {
         this.canal = canal;
         this.oauth = oauth;
         cliente = new System.Net.WebSockets.ClientWebSocket();
         clienteTopic = new System.Net.WebSockets.ClientWebSocket();

      }
      ~Twitch() {
         cliente.Dispose();
      }

      public string mensaje {
         set {
            mensajesEnviar.Add(value);

            if (cliente.State == System.Net.WebSockets.WebSocketState.Open) {
               while (mensajesEnviar.Count > 0) {
                  string mensajeTexto = "PRIVMSG #" + Configuracion.parametro("canal") + " :" + mensajesEnviar[0];
                  cliente.SendAsync(new ArraySegment<byte>(arrayBytes(mensajeTexto)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
                  mensajesEnviar.RemoveAt(0);
               }
            }
         }
      }

      public async void conectar(bool enBaseDatos = true) {

         try {
            await cliente.ConnectAsync(new Uri("ws://irc-ws.chat.twitch.tv"), cancellationToken);

            if (cliente.State == System.Net.WebSockets.WebSocketState.Open) {
               byte[] bufferByte = new byte[6000];
               int estado = 0;
               string[] ESPERAS = { ":tmi.twitch.tv 376 " + canal + " :>", ":End of /NAMES list" };
               System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(.*)\!(.*)@(.*)\.tmi\.twitch\.tv (PRIVMSG|PART) #(.*) :(.*)", System.Text.RegularExpressions.RegexOptions.RightToLeft);

               ArraySegment<byte> buffer = new ArraySegment<byte>(bufferByte);
               await cliente.SendAsync(new ArraySegment<byte>(arrayBytes("PASS oauth:" + oauth)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
               await cliente.SendAsync(new ArraySegment<byte>(arrayBytes("NICK " + canal)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);

               //cliente.Options.
               //a cliente.ReceiveAsync(buffer, cancellationToken);
               //envioDatos.
               System.Net.WebSockets.WebSocketReceiveResult resultado = await cliente.ReceiveAsync(buffer, cancellationToken);
               while (cliente.State == System.Net.WebSockets.WebSocketState.Open) {
                  string[] respuestas = arrayString(bufferByte, resultado.Count).Replace("\n", "\r").Replace("\r\r", "\r").Split('\r');
                  for (int i = 0; i < respuestas.Length; i++) {
                     string respuesta = respuestas[i];
                     if (respuesta == ESPERAS[0]) {
                        estado++;
                     }
                     string mensaje = "";
                     if (respuesta.Length > 0) {
                        switch (estado) {
                           case 1:
                              estado++;
                              mensaje = "JOIN #" + canal;
                              await cliente.SendAsync(new ArraySegment<byte>(arrayBytes(mensaje)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
                              break;


                        }
                        if (respuesta == "PING :tmi.twitch.tv") {
                           await cliente.SendAsync(new ArraySegment<byte>(arrayBytes("PONG :tmi.twitch.tv")), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
                        } else {
                           if (estado > 0) {
                              //System.Diagnostics.Trace.WriteLine(respuesta);
                              //System.Diagnostics.Trace.WriteLine(toxicidad(respuesta));
                              while (mensajesEnviar.Count > 0) {
                                 string mensajeTexto = "PRIVMSG #" + Configuracion.parametro("canal") + " :" + mensajesEnviar[0];
                                 await cliente.SendAsync(new ArraySegment<byte>(arrayBytes(mensajeTexto)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);
                                 mensajesEnviar.RemoveAt(0);
                              }
                              System.Text.RegularExpressions.Match coincidencias = regEx.Match(respuesta);
                              if (coincidencias.Success) {
                                 string[] partes = regEx.GetGroupNames();
                                 //System.Text.RegularExpressions.Group grp
                                 /*foreach (string nombre in partes) {
                                    System.Text.RegularExpressions.Group grp = coincidencias.Groups[nombre];
                                    //Console.WriteLine("   {0}: '{1}'", name, grp.Value);
                                 }*/
                                 System.Diagnostics.Trace.WriteLine((coincidencias.Groups["0"].Value));
                                 System.Diagnostics.Trace.WriteLine("" + (coincidencias.Groups["2"].Value) + " = " + (coincidencias.Groups["6"].Value));
                                 string aliasUsuario = coincidencias.Groups["2"].Value.ToLower();
                                 long idUsuario = -1;
                                 try {
                                    Datos usuarios = BD.consulta("select id,avatar from usuarios where nombre='" + aliasUsuario + "'");
                                    if (usuarios.Length == 0) {
                                       String avatar = null;
                                       try {
                                          JSON usuario = infoUsuario(aliasUsuario);
                                          avatar = usuario["data"][0]["profile_image_url"].ToString();
                                       } catch { }

                                       int numeroAfectados = BD.ejecutar("insert into usuarios (nombre, avatar) values ('" + aliasUsuario + "'," + (avatar != null ? "'" + avatar + "'" : "null") + ")");
                                       if (numeroAfectados == 1) {
                                          idUsuario = BD.id;
                                       }
                                    } else {
                                       if (usuarios[0]["avatar"] == null || usuarios[0]["avatar"].ToString().Length == 0) {
                                          JSON usuario = infoUsuario(aliasUsuario);
                                          string avatar = usuario["data"][0]["profile_image_url"].ToString();
                                          BD.ejecutar("update usuarios set avatar='" + avatar + "' where nombre='" + aliasUsuario + "'");
                                       }
                                       idUsuario = long.Parse(usuarios[0]["id"].ToString());
                                    }
                                    string texto = coincidencias.Groups["6"].Value;
                                    double puntuacion = toxicidad(texto);

                                    texto=tratarMensaje(texto, puntuacion);
                                    if (enBaseDatos) {
                                       BD.ejecutar("insert into mensajes (idEstado, idUsuario, mensaje,puntuacion) values (1," + idUsuario + ", '" + texto.Replace("\"", "\\\"") + "'," + puntuacion.ToString().Replace(",", ".") + ")");
                                    }
                                 } catch { }


                              }


                           }
                        }
                     }
                  }
                  resultado = await cliente.ReceiveAsync(buffer, cancellationToken);
               }
               //resultado.
               //System.Diagnostics.Trace.WriteLine(arrayString(bufferByte));
            }
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }

      }

      public async void conectarTopicos(bool enBaseDatos = true) {

         try {
            await clienteTopic.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), cancellationToken2);
            if (clienteTopic.State == System.Net.WebSockets.WebSocketState.Open) {
               byte[] bufferByte = new byte[6000];
               int estado = 0;
               string[] ESPERAS = { "{ \"type\": \"PONG\" }", ":End of /NAMES list" };
               System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(.*)\!(.*)@(.*)\.tmi\.twitch\.tv (PRIVMSG|PART) #(.*) :(.*)", System.Text.RegularExpressions.RegexOptions.RightToLeft);

               ArraySegment<byte> buffer = new ArraySegment<byte>(bufferByte);
               System.Windows.Forms.Timer ping = new System.Windows.Forms.Timer();
               ping.Interval = 15000;
               ping.Enabled = true;
               ping.Tick += async (object sender, EventArgs e) => {
                  if (clienteTopic.State == System.Net.WebSockets.WebSocketState.Open) {
                     await clienteTopic.SendAsync(new ArraySegment<byte>(arrayBytes("{\"type\":\"PING\"}")), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken2);
                  } else {
                     //await clienteTopic.CloseAsync();
                     await clienteTopic.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), cancellationToken2);
                     if (clienteTopic.State == System.Net.WebSockets.WebSocketState.Open) {
                        await clienteTopic.SendAsync(new ArraySegment<byte>(arrayBytes("{\"type\":\"RECONNECT\"}")), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken2);
                     }
                  }
               };

               /*await cliente.SendAsync(new ArraySegment<byte>(arrayBytes("NICK " + canal)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken);*/
               await clienteTopic.SendAsync(new ArraySegment<byte>(arrayBytes("{\"type\":\"PING\"}")), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken2);
               System.Net.WebSockets.WebSocketReceiveResult resultado = await clienteTopic.ReceiveAsync(buffer, cancellationToken2);
               while (clienteTopic.State == System.Net.WebSockets.WebSocketState.Open) {
                  string[] respuestas = arrayString(bufferByte, resultado.Count).Replace("\n", "\r").Replace("\r\r", "\r").Split('\r');
                  for (int i = 0; i < respuestas.Length; i++) {
                     string respuesta = respuestas[i];
                     string mensaje = "";
                     if (respuesta.Length > 0) {
                        switch (estado) {
                           case 0:
                              estado++;
                              mensaje = "{\"type\":\"LISTEN\",\"nonce\":\"\",\"data\":{\"topics\": [\"channel-bits-events-v2." + Configuracion.parametro("id_usuario") + "\",\"whispers." + Configuracion.parametro("id_usuario") + "\",\"channel-subscribe-events-v1."+ Configuracion.parametro("id_usuario") + "\"],\"auth_token\": \"" + Configuracion.parametro("oauth") + "\"}}";
                              await clienteTopic.SendAsync(new ArraySegment<byte>(arrayBytes(mensaje)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken2);
                              break;
                           case 1:
                              if (respuesta != ESPERAS[0]) {
                                 JSON respuestaJSON = new JSON();
                                 respuestaJSON.parse(respuesta);
                                 switch (respuestaJSON["type"].ToString()) {
                                    case "MESSAGE":
                                       if (respuestaJSON["data"]["topic"].ToString()== "whispers."+ Configuracion.parametro("id_usuario")){
                                          string texto = ((Entidad) respuestaJSON["data"]["message"])["body"].ToString();
                                          double puntuacion = toxicidad(texto);

                                          tratarMensaje(texto, puntuacion);
                                          if (enBaseDatos) {
                                             BD.ejecutar("insert into mensajes (idEstado, idUsuario, mensaje,puntuacion) values (1,1, '" + texto.Replace("\"", "\\\"") + "'," + puntuacion.ToString().Replace(",", ".") + ")");
                                          }
                                       }
                                       break;
                                 }

                                 System.IO.File.AppendAllText("sucripcion.txt", respuesta + "\r\n");
                              }
                              
                              break;

                        }
                        
                     }
                  }
                  try {
                     resultado = await clienteTopic.ReceiveAsync(buffer, cancellationToken);
                  }catch(Exception ex1) {
                     System.Diagnostics.Trace.WriteLine(ex1.Message);
                  }
               }
               //resultado.
               //System.Diagnostics.Trace.WriteLine(arrayString(bufferByte));
            }
         } catch (Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);

         }

      }
 


      private string ponerEmoticonos(string texto) {
         string miTexto = texto;
         string[] palabras = miTexto.Split(new char[] { ' ' });
         foreach(string palabra in palabras) {
            string palabraCorregida = palabra.Replace("'", "''");
            string consulta = "select * from emoticonos where regex='" + palabraCorregida + "'";
            Datos datos = BD.consulta(consulta);
            if (datos.Length > 0) {
               miTexto = miTexto.Replace(palabraCorregida, "%" + datos[0]["id"].ToString() + "%");
            }
         }
         return miTexto;
      }
      private string tratarMensaje(string texto, double puntuacion) {
         string miTexto = ponerEmoticonos(texto);

  


         if (puntuacion < 0.7) {
            texto = texto.Trim().ToLower();
            if (texto.IndexOf("hola")>=0) {
               mensaje = "¿Que tal?";
            }
         }
         return miTexto;
      }
      #region "API"
      static public System.Collections.Generic.List<string> nuevosSeguidores=new List<string>();
      static public int seguidores(string usuario) {
         int respuesta = 0;
         /*curl - H 'Client-ID: uo6dggojyb8d6soh92zknwmi5ej1q2' \
         -X GET 'https://api.twitch.tv/helix/users/follows?to_id=23161357'
         */
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Client-ID: " + Configuracion.parametro("id_cliente"));
         if (!seguidoresControlados) {
            BD.ejecutar("update usuarios set siguiendo=0 ");
         }
         json.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario, cabeceras);

         respuesta = int.Parse(json["total"].ToString());
         if (respuesta != listaSeguidores.Count) {
            if (!seguidoresControlados) {
               listaSeguidores = new ArrayList();
            }

            int limite = (!seguidoresControlados ? 1000:1);

            while (limite-- > 0 && listaSeguidores.Count < respuesta) {
               foreach (Dictionary<string, Entidad> dato in (List<Dictionary<string, Entidad>>)json["data"].valor) {
                  try {
                     if (!seguidoresControlados) {
                        listaSeguidores.Add(dato["from_name"]);
                     }
                     Datos usuarios = BD.consulta("select id,avatar,siguiendo from usuarios where nombre='" + dato["from_name"].ToString().ToLower() + "'");
                     if (usuarios.Length == 0) {
                        String avatar = null;
                        JSON usuarioJSON = infoUsuario(dato["from_name"].ToString());
                        avatar = usuarioJSON["data"][0]["profile_image_url"].ToString();

                        BD.ejecutar("insert into usuarios (nombre, avatar,siguiendo) values ('" + dato["from_name"].ToString().ToLower() + "'," + (avatar != null ? "'" + avatar + "'" : "null") + ",1)");
                        if (seguidoresControlados) {
                           nuevosSeguidores.Add(dato["from_name"].ToString());
                        }
                        
                     } else {
                        if (usuarios[0]["avatar"] == null || usuarios[0]["avatar"].ToString().Length == 0) {
                           JSON usuarioJSON = infoUsuario(dato["from_name"].ToString());
                           string avatar = usuarioJSON["data"][0]["profile_image_url"] != null ? usuarioJSON["data"][0]["profile_image_url"].ToString() : null;
                           BD.ejecutar("update usuarios set avatar='" + avatar + "' where nombre='" + dato["from_name"].ToString().ToLower() + "'");
                        }
                        if (usuarios[0]["siguiendo"].ToString() == "0") {
                           if (seguidoresControlados) {
                              nuevosSeguidores.Add(dato["from_name"].ToString());
                           }
                           BD.ejecutar("update usuarios set siguiendo=1 where nombre='" + dato["from_name"].ToString().ToLower() + "'");
                        }
                     }
                  } catch(Exception ex) {
                     System.Diagnostics.Trace.WriteLine(ex.Message);
                  }
               }
               try {
                  if (!seguidoresControlados) {
                     string siguiente = json["pagination"]["cursor"].ToString();
                     json.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario + "&after=" + siguiente, cabeceras);
                  }
               } catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.Message); }
            }
         }
         if (seguidoresControlados) {
            foreach (string nuevoSeguidor in nuevosSeguidores) {
               if (!listaSeguidores.Contains(nuevoSeguidor)) {
                  listaSeguidores.Add(nuevoSeguidor);
               }
            }
         }
         seguidoresControlados = true;

         return respuesta;
      }

       public int espectadores(string id_usuario) {
         /*
          curl -H 'Client-ID: uo6dggojyb8d6soh92zknwmi5ej1q2' \
         -X GET 'https://api.twitch.tv/helix/streams?first=20'
         */
         int respuesta = 0;
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Client-ID: " + Configuracion.parametro("id_cliente"));

         json.cargarJson("https://api.twitch.tv/helix/streams?user_id=" + id_usuario, cabeceras);
         if (json["data"].Length > 0) {
            if (json["data"][0].ContainsKey("viewer_count")) {
               respuesta = int.Parse(json["data"][0]["viewer_count"].ToString());
               DateTime horaEmision = DateTime.Parse(json["data"][0]["started_at"].ToString());
               if (horaEmision != Twitch.horaEmision) {
                  Twitch.horaEmision = horaEmision;
                  eventoNuevaHora nuevaHora = onNuevaHora;
                  onNuevaHora?.Invoke();
               }
               string titulo = json["data"][0]["title"].ToString();
               if (titulo != Twitch._titulo) {
                  Twitch._titulo = titulo;
               }
            }
         }



         return respuesta;
      }
      public static ArrayList infoEspectadores(string id_usuario) {
         /*
          curl -H 'Client-ID: uo6dggojyb8d6soh92zknwmi5ej1q2' \
         -X GET 'https://api.twitch.tv/helix/streams/metadata' 
         */

         ArrayList respuesta = new ArrayList();
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Client-ID: " + Configuracion.parametro("id_cliente"));

         json.cargarJson("https://api.twitch.tv/helix/streams/metadata?user_id=" + id_usuario, cabeceras);
         if (json["data"].Length > 0) {
            for (int i = 0; i < json["data"].Length; i++) {
               try {
                  respuesta.Add(json["data"][i]["user_name"].ToString());
               } catch {

               }

            }
         }
         return respuesta;
      }
      public static ArrayList infoBits() {
         /*
         curl -H 'Authorization: Bearer cfabdegwdoklmawdzdo98xt2fo512y' \
         -X GET 'https://api.twitch.tv/helix/bits/leaderboard?count=2&period=week'
         */

         ArrayList respuesta = new ArrayList();
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: Bearer " + Configuracion.parametro("oauth"));

         json.cargarJson("https://api.twitch.tv/helix/bits/leaderboard", cabeceras);
         /*if (json["data"].Length > 0) {
            for (int i = 0; i < json["data"].Length; i++) {
               try {
                  respuesta.Add(json["data"][i]["user_name"].ToString());
               } catch {

               }

            }
         }*/
         return respuesta;
      }
      public static ArrayList infoBitsExtensiones(string extension) {
         /*
         curl -H 'Authorization: Bearer cfabdegwdoklmawdzdo98xt2fo512y' \
         -X GET 'helix/extensions/transactions?extension_id=1234'
         */
         ArrayList respuesta = new ArrayList();
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: Bearer " + Configuracion.parametro("oauth"));

         json.cargarJson("https://api.twitch.tv/helix/extensions/transactions?extension_id="+extension, cabeceras);
         /*if (json["data"].Length > 0) {
            for (int i = 0; i < json["data"].Length; i++) {
               try {
                  respuesta.Add(json["data"][i]["user_name"].ToString());
               } catch {

               }

            }
         }*/
         return respuesta;
      }
      public static ArrayList infoCanal() {
         /*
         curl - H 'Accept: application/vnd.twitchtv.v5+json' \ -H 'Client-ID: uo6dggojyb8d6soh92zknwmi5ej1q2' \ -H 'Authorization: OAuth cfabdegwdoklmawdzdo98xt2fo512y' \ -X GET 'https://api.twitch.tv/kraken/channel'
         */

         ArrayList respuesta = new ArrayList();
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Accept: application/vnd.twitchtv.v5+json");
         cabeceras.Add("Client-ID: " + Configuracion.parametro("id_cliente"));
         cabeceras.Add("Authorization: OAuth " + Configuracion.parametro("oauth"));

         json.cargarJson("https://api.twitch.tv/kraken/channel", cabeceras);
         /*if (json["data"].Length > 0) {
            for (int i = 0; i < json["data"].Length; i++) {
               try {
                  respuesta.Add(json["data"][i]["user_name"].ToString());
               } catch {

               }

            }
         }*/
         return respuesta;
      }
      
      public static ArrayList infoExtensiones() {
         /*
          curl -H 'Authorization: Bearer cfabdegwdoklmawdzdo98xt2fo512y' \
          -X GET 'https://api.twitch.tv/helix/users/extensions/list'
         */

         ArrayList respuesta = new ArrayList();
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: Bearer " + Configuracion.parametro("oauth"));

         json.cargarJson("https://api.twitch.tv/helix/users/extensions/list", cabeceras);
         /*if (json["data"].Length > 0) {
            for (int i = 0; i < json["data"].Length; i++) {
               try {
                  respuesta.Add(json["data"][i]["user_name"].ToString());
               } catch {

               }

            }
         }*/
         return respuesta;
      }
      public static JSON infoUsuario(string usuario) {
         JSON respuesta = new JSON();

         /*
          curl -H 'Authorization: Bearer cfabdegwdoklmawdzdo98xt2fo512y' \
         -X GET 'https://api.twitch.tv/helix/users?id=44322889'

         {
           "data": [{
             "id": "44322889",
             "login": "dallas",
             "display_name": "dallas",
             "type": "staff",
             "broadcaster_type": "",
             "description": "Just a gamer playing games and chatting. :)",
             "profile_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-profile_image-1a2c906ee2c35f12-300x300.png",
             "offline_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-channel_offline_image-1a2c906ee2c35f12-1920x1080.png",
             "view_count": 191836881,
             "email": "login@provider.com"
           }]
         }
         **/
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: Bearer " + Configuracion.parametro("oauth"));

         respuesta.cargarJson("https://api.twitch.tv/helix/users?login=" + usuario, cabeceras);
         return respuesta;
      }
      static public int suscriptores(string id_usuario){

         /*curl -H 'Authorization: Bearer cfabdegwdoklmawdzdo98xt2fo512y' \
         -X GET 'https://api.twitch.tv/helix/subscriptions?broadcaster_id=123'*/
         int respuesta = 0;
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: Bearer " + Configuracion.parametro("oauth"));

         json.cargarJson("https://api.twitch.tv/helix/subscriptions?broadcaster_id=" + id_usuario, cabeceras);
         respuesta = json["data"].Length;

         if (respuesta != listaSuscriptores.Count) {
            listaSuscriptores = new ArrayList();

            int limite = 10000;

            while (limite-- > 0 && listaSuscriptores.Count < respuesta) {
               foreach (Dictionary<string, Entidad> dato in (List<Dictionary<string, Entidad>>)json["data"].valor) {
                  try {
                     listaSuscriptores.Add(dato["user_name"]);
                     /*Datos usuarios = BD.consulta("select id,avatar from usuarios where nombre='" + dato["from_name"] + "'");
                     if (usuarios.Length == 0) {
                        String avatar = null;
                        JSON usuarioJSON = infoUsuario(dato["from_name"].ToString());
                        avatar = usuarioJSON["data"][0]["profile_image_url"].ToString();

                        BD.ejecutar("insert into usuarios (nombre, avatar,siguiendo) values ('" + dato["from_name"].ToString().ToLower() + "'," + (avatar != null ? "'" + avatar + "'" : "null") + ",1)");
                     } else {
                        if (usuarios[0]["avatar"] == null || usuarios[0]["avatar"].ToString().Length == 0) {
                           JSON usuarioJSON = infoUsuario(dato["from_name"].ToString());
                           string avatar = usuarioJSON["data"][0]["profile_image_url"] != null ? usuarioJSON["data"][0]["profile_image_url"].ToString() : null;
                           BD.ejecutar("update usuarios set avatar='" + avatar + "', siguiendo=1 where nombre='" + dato["from_name"].ToString().ToLower() + "'");

                        }
                     }/**/
                  } catch { }
               }
               try {
                  string siguiente = json["pagination"]["cursor"].ToString();
                  json.cargarJson("https://api.twitch.tv/helix/subscriptions?broadcaster_id=" + id_usuario + "&after=" + siguiente, cabeceras);
               } catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex.Message); }
            }
         }




         return respuesta;
      }
      private double toxicidad(string texto) {
         double resultado=-1;

         string url="https://commentanalyzer.googleapis.com/v1alpha1/comments:analyze?key="+Configuracion.parametro("api_perspective");
         JSON json = new JSON();
         System.Collections.ArrayList cabeceras = new System.Collections.ArrayList();
         //cabeceras.Add("Content-Type: application/json");
         string post = "{comment: {text: \""+texto+"\"},requestedAttributes: { TOXICITY: { },SEVERE_TOXICITY: { } }}";
         json.cargarJson(url, cabeceras,post,"application/json");
         double.TryParse(((Dictionary<String, Entidad>)((Dictionary<String, Entidad>)((Dictionary<String, Entidad>)json["attributeScores"].valor)["TOXICITY"].valor)["summaryScore"].valor)["value"].ToString(), out resultado);


         return resultado;
      }
      #endregion
      private byte[] arrayBytes(string s) {
         return Encoding.UTF8.GetBytes(s);
      }
      private string arrayString(byte [] b,int caracteres) {
         return Encoding.UTF8.GetString(b, 0, caracteres);
         /*string resultado = "";
         for (int i = 0; i < b.Length && b[i]!=0 && caracteres>i; i++) {
            resultado+= (char)b[i];
         }
         return resultado;*/
      }
      public bool estaConectado {
         get {
            return cliente.State == System.Net.WebSockets.WebSocketState.Open;
         }
      }
      public bool estaConectando {
         get {
            return cliente.State != System.Net.WebSockets.WebSocketState.Open && cliente.State != System.Net.WebSockets.WebSocketState.Closed;
         }
      }
      public double segundosEmision {
         get {
            if (horaEmision.Year < DateTime.Now.Year) {
               return 0;
            }
            DateTime ahora = DateTime.Now;
            return ahora.Subtract(horaEmision).TotalSeconds;
         }
      }
      public string titulo {
         get {
            return _titulo;
         }
      }
      public delegate void eventoNuevaHora();
      public event eventoNuevaHora onNuevaHora;
   }
}
