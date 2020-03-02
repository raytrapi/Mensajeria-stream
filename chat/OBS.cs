using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace Mensajería.chat {
   class OBS {
      static NamedPipeClientStream pipe=null;
      ~OBS() {
         if (pipe != null) {
            pipe.Dispose();
         }
      }

      static void conectar() {
         if (pipe != null) {
            try {
               pipe.Close();
            } catch { }
         }
         pipe = new NamedPipeClientStream(".","control_directo",PipeDirection.InOut);
         pipe.Connect(2000);
      }
      public static void enviarComando(string comando) {
         try {
            if (pipe == null ) {
               conectar();
            }
            if (!pipe.IsConnected) {
               conectar();
               pipe.Connect(2000);
               
            }
            if (pipe.IsConnected) {
               byte[] buffer=new byte[500];
               for(int i = 0; i < comando.Length; i++) {
                  buffer[i] = (byte)comando[i];
               }
               buffer[comando.Length] = 0;
               pipe.Write(buffer, 0, comando.Length+1);
               //pipe.;
            }
         } catch (Exception ex){

            System.Diagnostics.Trace.WriteLine("No puedo conectarme con el PIPE");
            System.Diagnostics.Trace.WriteLine(ex.Message);
         }
      }
   }
}
