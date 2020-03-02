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
      private static string _titulo = "";
     
      private static int misSeguidores = 0;
      static bool seguidoresControlados = false;
      static bool procesandoSeguidores = false;

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
                                    Datos usuarios = BD.consulta("select id,avatar from usuarios where alias='" + aliasUsuario + "'");
                                    if (usuarios.Length == 0) {
                                       String avatar = null;
                                       JSON usuario = infoUsuario(aliasUsuario);
                                       try {
                                          
                                          avatar = usuario["data"][0]["profile_image_url"].ToString();
                                       } catch { }

                                       int numeroAfectados = BD.ejecutar("insert into usuarios (alias, avatar, nombre, userID) values ('" + aliasUsuario + "'," + (avatar != null ? "'" + avatar + "'" : "null") + ",'"+ usuario["data"][0]["display_name"].ToString() + "','"+ usuario["data"][0]["id"].ToString() + "')");
                                       if (numeroAfectados == 1) {
                                          idUsuario = BD.id;
                                       }
                                    } else {
                                       if (usuarios[0]["avatar"] == null || usuarios[0]["avatar"].ToString().Length == 0) {
                                          JSON usuario = infoUsuario(aliasUsuario);
                                          string avatar = usuario["data"][0]["profile_image_url"].ToString();
                                          BD.ejecutar("update usuarios set avatar='" + avatar + "' where alias='" + aliasUsuario + "'");
                                       }
                                       idUsuario = long.Parse(usuarios[0]["id"].ToString());
                                    }
                                    string texto = coincidencias.Groups["6"].Value;
                                    double puntuacion = toxicidad(texto);

                                    texto = tratarMensaje(texto, puntuacion);
                                    if (enBaseDatos) {

                                       try {
                                          texto= texto.Replace("\"", "\\\"").Replace("'", "''");
                                          BD.ejecutar("insert into mensajes (idEstado, idUsuario, mensaje,puntuacion) values (1," + idUsuario + ", '" + texto + "'," + puntuacion.ToString().Replace(",", ".") + ")");
                                       }catch(Exception ex1) {
                                          BD.ejecutar("insert into mensajes (idEstado, idUsuario, mensaje,puntuacion) values (1," + idUsuario + ", '" + aUTF8(texto) + "'," + puntuacion.ToString().Replace(",", ".") + ")");
                                       }
                                    }
                                 } catch (Exception ex) {
                                    LOG.debug(ex.Message, "Conectar a twitch");
                                 }


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

      private string aUTF8(string texto) {
         string resultado = "";
         byte[] textoConvertido = System.Text.Encoding.UTF32.GetBytes(texto);
         //string textoUTF16 = System.Text.Encoding.UTF32.GetString();
         int j = 0;
         for(; j < textoConvertido.Length;) {
            int valorCaracter=(textoConvertido[j+3]<<32) | (textoConvertido[j + 2] << 16) | (textoConvertido[j + 1] << 8) | (textoConvertido[j]);
            
            if (valorCaracter < 256) {
               resultado += (char)(valorCaracter);
            } else {
               resultado += "&#" + valorCaracter + ";";
            }
            j += 4;
         }
         return resultado;
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
                              mensaje = "{\"type\":\"LISTEN\",\"nonce\":\"\",\"data\":{\"topics\": [\"channel-bits-events-v2." + Configuracion.parametro("id_usuario") + "\",\"whispers." + Configuracion.parametro("id_usuario") + "\",\"channel-subscribe-events-v1." + Configuracion.parametro("id_usuario") + "\"],\"auth_token\": \"" + Configuracion.parametro("oauth") + "\"}}";
                              await clienteTopic.SendAsync(new ArraySegment<byte>(arrayBytes(mensaje)), System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken2);
                              break;
                           case 1:
                              if (respuesta != ESPERAS[0]) {
                                 JSON respuestaJSON = new JSON();
                                 respuestaJSON.parse(respuesta);
                                 switch (respuestaJSON["type"].ToString()) {
                                    case "MESSAGE":
                                       if (respuestaJSON["data"]["topic"].ToString() == "whispers." + Configuracion.parametro("id_usuario")) {
                                          string texto = ((Entidad)respuestaJSON["data"]["message"])["body"].ToString();
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
                  } catch (Exception ex1) {
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
         foreach (string palabra in palabras) {
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
            if (texto.IndexOf("hola") >= 0) {
               mensaje = "¿Que tal?";
            }
         }
         return miTexto;
      }
      #region "API"
      static public System.Collections.Generic.List<string> nuevosSeguidores = new List<string>();
      static public int seguidores(string usuario) {
         
         int respuesta = 0;
         /*curl - H 'Client-ID: uo6dggojyb8d6soh92zknwmi5ej1q2' \
         -X GET 'https://api.twitch.tv/helix/users/follows?to_id=23161357'
         */
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Client-ID: " + Configuracion.parametro("id_cliente"));

         //Primero cogemos el total de usuarios que me siguen
         //misSeguidores = 0;
         Datos ultimoSeguidor = null;
         try {
            misSeguidores = int.Parse(BD.consulta("select count(1) seguidores from seguidores where siguiendo=1")[0]["seguidores"].ToString());
            ultimoSeguidor = BD.consulta("select u.nombre \"nombre\", s.fecha fecha from seguidores s, usuarios u where u.id=s.idUsuario and s.siguiendo=1 order by fecha desc limit 1");
         } catch (Exception ex) {
            LOG.debug(ex.Message);
         }

         json.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario, cabeceras);
         respuesta = int.Parse(json["total"].ToString());
         if (respuesta != misSeguidores ||
            (ultimoSeguidor != null && ultimoSeguidor.Length > 0 && respuesta > 0 && ultimoSeguidor[0]["nombre"].ToString() != json["data"][0]["from_name"].ToString().ToLower())) {
            //Hay cambios entre mis seguidores.
            //Pueden suceder 2 casos que se haya ido alguien o que tengamos nuevos usuarios.
            //listarSeguidores(json, usuario, cabeceras);
            //JSON usuarioJSON = infoUsuarioId("173488279");
            //Para el caso de que perdamos seguidores haremos la taréa desantendida para evitar conflictos.
            if (respuesta == misSeguidores || respuesta < misSeguidores) {
               quitarSeguidores(json, usuario, cabeceras);
               //misSeguidores = int.Parse(BD.consulta("select count(1) seguidores from seguidores where siguiendo=1")[0]["seguidores"].ToString());
            } else {
               ponerSeguidores(json, usuario, cabeceras, ultimoSeguidor);
            }
            

         }
         //seguidoresControlados = true;

         return respuesta;
      }
      static private string segui="";
      static private void listarSeguidores(JSON json, string usuario, ArrayList cabeceras) {
         
         try {
            if (json.hayClave("pagination")) {
               string siguiente = json["pagination"]["cursor"].ToString();
               //Esperamos un tiempo para evitar que Twitch nos bloquee.
               System.Threading.Thread.Sleep(100);
               JSON json2 = new JSON();
               json2.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario + "&after=" + siguiente, cabeceras);
               listarSeguidores(json2, usuario, cabeceras);
            } else {
               LOG.debug( "Poner Seguidores");
            }
         } catch (Exception ex) {
            LOG.debug(ex.Message, "Poner Seguidores");
         }
         
         List<Dictionary<string, Entidad>> listado = (List<Dictionary<string, Entidad>>)json["data"].valor;
         for (int i = listado.Count - 1; i >= 0; i--) {
            Dictionary<string, Entidad> dato = listado[i];
            segui += (segui.Length > 0 ? ", " : "") + dato["from_name"].ToString().ToLower();
         }
         
      }
      static private List<string> quitarSeguidores(JSON json, string usuario, ArrayList cabeceras, int iteracion = 0) {
         List<string> nombres = null;
         try {
            if (json.hayClave("pagination")) {
               string siguiente = json["pagination"]["cursor"].ToString();
               //Esperamos un tiempo para evitar que Twitch nos bloquee.
               System.Threading.Thread.Sleep(100);
               JSON json2 = new JSON();
               json2.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario + "&after=" + siguiente, cabeceras);
               nombres = quitarSeguidores(json2, usuario, cabeceras, iteracion + 1);
            } else {
               return null;
            }
         } catch (Exception ex) {
            
            LOG.debug(ex.Message, "Poner Seguidores");
         }
         if (nombres == null) {
            nombres = new List<string>();
         }
         List<Dictionary<string, Entidad>> listado = (List<Dictionary<string, Entidad>>)json["data"].valor;
         for (int i = 0; i < listado.Count; i++) {
            nombres.Add(listado[i]["from_name"].ToString());
         }
         if (iteracion == 0) { 
            string listaNombres = "";
            for (int i = 0; i < nombres.Count; i++) {
               listaNombres += (i > 0 ? "," : "") + "'" + nombres[i] + "'";
            }
            try {
               BD.ejecutar("update seguidores set siguiendo=0 where idUsuario in (select id from usuarios u where u.nombre not in("+listaNombres+"))");
            } catch (Exception ex) {
               LOG.debug(ex.Message, "Poner Seguidores update");
            }
         } 
         return nombres;
      }
      static private int ponerSeguidores(JSON json, string usuario, ArrayList cabeceras, Datos ultimoSeguidor) {
         int respuesta = 0;
         DateTime fechaUltimoSeguidor;
         if (ultimoSeguidor != null && ultimoSeguidor.Length > 0) {
            fechaUltimoSeguidor = DateTime.Parse(ultimoSeguidor[0]["fecha"].ToString());
         } else {
            fechaUltimoSeguidor = DateTime.Now;
         }
         List<Dictionary<string, Entidad>> listado = (List<Dictionary<string, Entidad>>)json["data"].valor;
         for (int i = listado.Count - 1; i >= 0; i--) {
            Dictionary<string, Entidad> dato = listado[i];
            //foreach (Dictionary<string, Entidad> dato in (List<Dictionary<string, Entidad>>)json["data"].valor) {
            try {
               DateTime seguidoEl = DateTime.Parse(dato["followed_at"].ToString());
               if (ultimoSeguidor == null || ultimoSeguidor.Length == 0 || seguidoEl > fechaUltimoSeguidor) {
                  if (i == listado.Count - 1) {
                     //Estamos en el más antiguo, bajamos un nivel
                     try {
                        if (json.hayClave("pagination")) {
                           string siguiente = json["pagination"]["cursor"].ToString();
                           //Esperamos un tiempo para evitar que Twitch nos bloquee.
                           System.Threading.Thread.Sleep(100);
                           JSON json2 = new JSON();
                           json2.cargarJson("https://api.twitch.tv/helix/users/follows?to_id=" + usuario + "&after=" + siguiente, cabeceras);
                           respuesta += ponerSeguidores(json2, usuario, cabeceras, ultimoSeguidor);
                        }
                     } catch (Exception ex) { 
                        LOG.debug(ex.Message,"Poner Seguidores"); 
                     }
                  }
                  respuesta++;
                  Datos usuarios = BD.consulta("select id,avatar from usuarios where nombre='" + dato["from_name"].ToString() + "'");
                  if (usuarios.Length == 0) {
                     String avatar = null;
                     JSON usuarioJSON = infoUsuarioId(dato["from_id"].ToString());
                     avatar = usuarioJSON["data"][0]["profile_image_url"].ToString();
                     BD.ejecutar("insert into usuarios (alias,nombre, avatar,userID) values ('"+usuarioJSON["data"][0]["login"].ToString().ToLower()+"','" + usuarioJSON["data"][0]["display_name"].ToString() + "'," + (avatar != null ? "'" + avatar + "'" : "null") + ",'"+ usuarioJSON["data"][0]["id"].ToString() + "')");
                     usuarios = BD.consulta("select id,avatar from usuarios where nombre='" + dato["from_name"].ToString() + "'");
                  }
                  if (usuarios[0]["avatar"] == null || usuarios[0]["avatar"].ToString().Length == 0) {
                     JSON usuarioJSON = infoUsuario(dato["from_name"].ToString());
                     string avatar = usuarioJSON["data"][0]["profile_image_url"] != null ? usuarioJSON["data"][0]["profile_image_url"].ToString() : null;
                     BD.ejecutar("update usuarios set avatar='" + avatar + "' where nombre='" + dato["from_name"].ToString() + "'");
                  }

                  BD.ejecutar("insert into seguidores (idUsuario, fecha) values(" + usuarios[0]["id"].ToString() + ",'" + seguidoEl.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                  nuevosSeguidores.Add(dato["from_name"].ToString());

               }
            } catch (Exception ex) {
               LOG.debug(ex.Message, "Cargar Seguidores");
            }
         }
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
      public static JSON infoUsuarioNombre(string usuario) {
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

         respuesta.cargarJson("https://api.twitch.tv/helix/users?display_name=" + usuario, cabeceras);
         return respuesta;
      }
      public static JSON infoUsuarioId(string idUsuario) {
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

         respuesta.cargarJson("https://api.twitch.tv/helix/users?id=" + idUsuario, cabeceras);
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
         if (!json.hayClave("data")) {
            return 0;
         }
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
         string post = "{comment: {text: \""+texto.ToLower().Replace("http","").Replace("://","")+ "\"},requestedAttributes: { TOXICITY: { },SEVERE_TOXICITY: { } }}";
         try {
            json.cargarJson(url, cabeceras, post, "application/json");
         }catch(Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);
         }
         if (json==null || json.esVacio()) {
            return 0.7;
         }
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
