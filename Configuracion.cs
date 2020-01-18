using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajería {
   class Configuracion {
      static private System.Collections.Generic.Dictionary<string, string> configuracion;
      
      static public string parametro(string clave) {
         if (configuracion==null) {
            configuracion = new Dictionary<string, string>();
            cargar(System.Configuration.ConfigurationManager.AppSettings["ficheroIRC"]);
         }
         if (configuracion.ContainsKey(clave)) {
            return configuracion[clave];
         } else {
            return "";
         }
      }
      static public string parametro(string clave, string defecto) {
         if (configuracion == null) {
            configuracion = new Dictionary<string, string>();
            cargar(System.Configuration.ConfigurationManager.AppSettings["ficheroIRC"]);
         }
         if (configuracion.ContainsKey(clave)) {
            return configuracion[clave];
         } else {
            return defecto;
         }
      }

      static private void cargar(string ruta) {

         /*
         parametro=valor
         parametro= valor
         parametro =valor
         #parametro = valor=lkll
         */
         string texto = System.IO.File.ReadAllText(ruta);
         string[] lineas = texto.Replace("\n", "\r").Replace("\r\r", "\r").Split('\r');
         for(int i = 0; i < lineas.Length; i++) {
            string linea = lineas[i].Trim();
            if (linea.Length > 0 && linea[0]!='#') {
               string[] partes = linea.Split(new char[] { '=' }, 2);
               if (partes.Length == 2) {
                  configuracion[partes[0].Trim()] = partes[1].Trim();
               }
            }
         }
      }
   }
}
