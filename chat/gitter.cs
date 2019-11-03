using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajería.chat {
   class Gitter {


      static string ultimoID = "";
      static public string leerMensaje(string sala) {
         string respuesta = "";
         
         JSON json = new JSON();
         ArrayList cabeceras = new ArrayList();
         cabeceras.Add("Authorization: BEARER " + Configuracion.parametro("gitter_token"));

         json.cargarJson("https://api.gitter.im/v1/rooms/"+sala+"/chatMessages?limit=1"+(ultimoID!=""? "&afterId="+ultimoID:""), cabeceras);
         
         
         if (!json.esVacio()) {
            respuesta = (string)json["text"].valor;
            ultimoID = (string)json["id"].valor;
         }
         return respuesta;
      }


   }
}
