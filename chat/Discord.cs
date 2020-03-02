using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mensajería.chat {
   class Discord {
      public static ArrayList cogerMensajes(string canal) {
         ArrayList respuesta = new ArrayList();
         //GET/channels/{channel.id}/messages
         JSON mensajes = new JSON();
         ArrayList cabecera = new ArrayList();
         cabecera.Add("Authorization: Bearer " + Configuracion.parametro("discord_token"));
         mensajes.cargarJson(Configuracion.parametro("discord_base_url") + "channels/" + canal + "/messages",cabecera);

         return respuesta;
      }
      
      public static bool autenticacion() {
         //https://discordapp.com/api/oauth2/authorize?response_type=code&client_id=157730590492196864&scope=identify%20guilds.join&state=15773059ghq9183habn&redirect_uri=https://localhost&prompt=consent
         Thread hilo = new Thread(metodoHilo);
         hilo.ApartmentState = ApartmentState.STA;
         hilo.Start();
         hilo.Join();
         return true;
      }
      private static void metodoHilo() {
         //System.Diagnostics.Process.Start("https://discordapp.com/api/oauth2/authorize?response_type=code&client_id="+Configuracion.parametro("discord_client_id") + "&scope=messages.read&redirect_uri=https://programacionextrema.es/proyectos/ext1/servidor/discord&prompt=consent");


         /*
         API_ENDPOINT = 'https://discordapp.com/api/v6'
         CLIENT_ID = '332269999912132097'
         CLIENT_SECRET = '937it3ow87i4ery69876wqire'
         REDIRECT_URI = 'https://nicememe.website'

         def exchange_code(code):
           data = {
             'client_id': CLIENT_ID,
             'client_secret': CLIENT_SECRET,
             'grant_type': 'authorization_code',
             'code': code,
             'redirect_uri': REDIRECT_URI,
             'scope': 'identify email connections'
           }
           headers = {
             'Content-Type': 'application/x-www-form-urlencoded'
           }
           r = requests.post('%s/oauth2/token' % API_ENDPOINT, data=data, headers=headers)
           r.raise_for_status()
           return r.json()
         */
         JSON cogerToken = new JSON();
         ArrayList cabeceras = new ArrayList();
         //cabeceras.Add("Authorization:Basic " + Configuracion.parametro("discord_credenciales_basic"));
         //cabeceras.Add("Content-Type:application/x-www-form-urlencoded");
         cogerToken.cargarJson("https://discordapp.com/api/v6/oauth2/token",cabeceras, "client_id="+Configuracion.parametro("discord_client_id")+"&client_secret="+Configuracion.parametro("discord_client_secret") + "&grant_type=authorization_code&code="+Configuracion.parametro("discord_code_oauth2") +"&redirect_uri=https%3A%2F%2Fprogramacionextrema.es%2Fproyectos%2Fext1%2Fservidor%2Fdiscord&scope=identify%20email%20connections");
         //Validacion val = new Validacion();
         //val.mostrar("https://discordapp.com/api/oauth2/authorize?response_type=code&client_id=157730590492196864&scope=identify%20guilds.join&state=15773059ghq9183habn&redirect_uri=https%3A%2F%2Fnicememe.website&prompt=consent");

     }
   }
}
