using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;

namespace Mensajería{
   class LOG {
      public LOG() {

      }
      static public void debug(string texto, string contexto="") {
         System.Diagnostics.Trace.WriteLine((contexto.Length>0?"["+contexto+"]  ":"")+texto);
      }
   }
}
