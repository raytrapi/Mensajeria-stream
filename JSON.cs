using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajería {
   enum TIPO_ENTIDAD { String, Numeric, Date, Array, Entidad};
   class Entidad {
      public Object valor;
      public TIPO_ENTIDAD tipo;
      public Entidad(Object _valor, TIPO_ENTIDAD _tipo) {
         valor = _valor;
         tipo = _tipo;
      }
      public Object this[string clave] {
         get {
            if (tipo == TIPO_ENTIDAD.Entidad) {
               if (((Dictionary<String, Entidad>)valor).ContainsKey(clave)) {
                  return ((Dictionary<String, Entidad>)valor)[clave];
               } else {
                  return null;
               }
            } else {
               return this;
            }
         }
      }
      public Dictionary<string, Entidad> this[int clave] {
         get {
            if (tipo == TIPO_ENTIDAD.Array) {
               return ((List<Dictionary<string, Entidad>>)valor)[clave];
            } else {
               return null;
            }
         }
      }
      public int Length {
         get {
            switch (tipo) {
               case TIPO_ENTIDAD.Array:
                  return ((List<Dictionary<string, Entidad>>)valor).Count;
                  break;
               default:
                  return 1;
            }
         }
      }
      public override string ToString() {
         return valor.ToString();
      }
   }
   class JSON {
      private System.Collections.Generic.Dictionary<String, Entidad> _json;
      private Maquina_Caracteres mc;
      public void parse(string json) {
         /*
          json={"jj":"aa"}
          */
         /*
          DICTIONAY=["jj"]=>{valor="aa",tipo=String}
          */
         _json = new Dictionary<string, Entidad>();
         mc = new Maquina_Caracteres(json);
         List<KeyValuePair<String, Entidad>>  objeto=comenzarObjeto();
         foreach(KeyValuePair<String, Entidad> elemento in objeto) {
            _json[elemento.Key] = elemento.Value;
         }
         
      }
      public bool esVacio() {
         return _json.Count == 0;
      }

      public void cargarJson(string url,System.Collections.ArrayList headers,string post=null,string content_type= "application/x-www-form-urlencoded") {
         try {
            System.Net.HttpWebRequest solicitud = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(url));
            System.Net.WebHeaderCollection cabeceras = new System.Net.WebHeaderCollection();
            foreach (string cabecera in headers) {
               if (cabecera.IndexOf("Accept") == 0) {
                  string[] partes = cabecera.Split(new char[] { ':' }, 2);
                  solicitud.Accept = partes[1].Trim();
               } else {
                  cabeceras.Add(cabecera);
               }
            }
            solicitud.Headers = cabeceras;
            if (post != null) {
               solicitud.Method = "POST";
               solicitud.ContentType = content_type;
               byte[] buffer = new byte[post.Length];
               //post.
               for (int i = 0; i < post.Length; i++) {
                  buffer[i] = (byte)(post[i]);
               }
               System.IO.Stream datos = solicitud.GetRequestStream();
               datos.Write(buffer, 0, post.Length);
            }
            System.Net.HttpWebResponse respuesta = (System.Net.HttpWebResponse)solicitud.GetResponse();
            string json_texto = leerStream(respuesta.GetResponseStream());
            parse(json_texto);
         }catch(Exception ex) {
            System.Diagnostics.Trace.WriteLine(ex.Message);
         }
      }
      private string leerStream(System.IO.Stream stream) {
         byte []buffer=new byte[1000];
         string resultado = "";
         int leidos = 100;
         while (leidos > 0) {
            leidos = stream.Read(buffer, 0, 1000);
            for(int i = 0; i < leidos; i++) {
               resultado += (char)buffer[i];
            }
         }

         return resultado;
      }
      public Entidad this[string clave] {
         get {
            if (_json.Count > 0) {
               return _json[clave];
            } else {
               return null;
            }
         }
      }

      private List<KeyValuePair<String, Entidad>> comenzarObjeto(bool conEtiqueta=true) {
         List<KeyValuePair<String, Entidad>> resultado = new List<KeyValuePair<String, Entidad>>();
         //Comenzamos el objeto
         KeyValuePair<char, String> comienzo=mc.cogerCadena(new char[] { '{',']' });
         char c = comienzo.Key;
         while (c != '}'&& c!=']' && c != '\0') {
            String etiqueta = "";
            if (conEtiqueta) {
               KeyValuePair<char, String> etiquetaLeida = mc.cogerCadena(new char[] { ':', '}' });
               if (etiquetaLeida.Key == '}' || etiquetaLeida.Key == '\0') {
                  return resultado;
               }
               etiqueta = etiquetaLeida.Value.Trim();
               etiqueta = etiqueta.Substring(1, etiqueta.Length - 2).Trim();
            }
            KeyValuePair<char, String> valorLeido = mc.cogerCadena(new char[] { '{', ',', '}', '[' });
            Object valor = valorLeido.Value;
            c = valorLeido.Key;
            TIPO_ENTIDAD tipo=TIPO_ENTIDAD.String;
            switch (c) {
               case ',':
               case '}':
                  valor = ((string)valor).Trim();
                  if (((string)valor)[0] != '"') {
                     if (((string)valor).IndexOf('T') > 0) {
                        tipo = TIPO_ENTIDAD.Date;
                        //TODO: Castear la fecha
                     } else {
                        tipo = TIPO_ENTIDAD.Numeric;
                        try {
                           if (valor == null) {
                              valor = 0;
                           } else {
                              valor = double.Parse(valor.ToString().Replace(".", ","));
                           }
                        } catch {
                           valor = 0;
                        }
                     }
                  } else {
                     
                     valor = ((string)valor).Substring(1, ((string)valor).Length - 2);
                  }
                  resultado.Add(new KeyValuePair<String, Entidad>(etiqueta,
                     new Entidad(valor, 
                     tipo
                     )));
                  break;
               case '[':
                  List<Dictionary<string, Entidad>> elementos = cogerElementos();
                  resultado.Add(new KeyValuePair<String, Entidad>(etiqueta,
                     new Entidad(elementos,
                     TIPO_ENTIDAD.Array
                     )));
                  valorLeido = mc.cogerCadena(new char[] { ',', '}' });
                  c = valorLeido.Key;
                  break;
               case '{':
                  mc.saltar(-1);
                  List<KeyValuePair<String, Entidad>> objetos = comenzarObjeto(true);
                  Dictionary<string, Entidad> diccionario = new Dictionary<string, Entidad>();
                  foreach (KeyValuePair<String, Entidad> objeto in objetos) {
                     diccionario[objeto.Key] = objeto.Value;
                  }
                  resultado.Add(new KeyValuePair<String, Entidad>(etiqueta,
                     new Entidad(diccionario,
                     TIPO_ENTIDAD.Entidad
                     )));
                  valorLeido = mc.cogerCadena(new char[] {  ',', '}' });
                  c = valorLeido.Key;
                  break;
            }
            //System.Diagnostics.Trace.WriteLine(etiqueta + "=" + valor);
            
         }

         return resultado;
         
      }
      private List<Dictionary<string, Entidad>> cogerElementos() {
         List<Dictionary<string, Entidad>> resultado=new List<Dictionary<string, Entidad>>();
         char c = ' ';
         while (c != ']' && c != '\0') {
            List<KeyValuePair<String, Entidad>> objetos = comenzarObjeto(true);
            Dictionary<string, Entidad> diccionario = new Dictionary<string, Entidad>();
            foreach (KeyValuePair<String, Entidad> objeto in objetos) {
               diccionario[objeto.Key] = objeto.Value;
            }
            if (diccionario.Count > 0) {
               resultado.Add(diccionario);
            }
            KeyValuePair<char, String> siguiente = mc.cogerCadena(new char[] { ',', ']' });
            c = siguiente.Key;
         }
         //mc.saltar(-1);
         return resultado;
      }
   }
}
