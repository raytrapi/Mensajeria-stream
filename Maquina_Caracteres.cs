using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Mensajería {
   class Maquina_Caracteres {
      private String texto;
      int posicionActual = 0;
      ArrayList caracteresComodin = new ArrayList();
      ArrayList caracteresCadena = new ArrayList();
      char[] nulos = new char[] { '\n' };
      bool noEntreComillas = true;
      public Maquina_Caracteres(String texto) {
         this.texto = texto;
         caracteresComodin.Add('\\');
         caracteresCadena.Add('"');
      }
      public void saltar(int pos) {
         posicionActual += pos;
         if (posicionActual < 0 || posicionActual > texto.Length) {
            posicionActual = 0;
         }

      }
      public KeyValuePair<char, String> cogerCadena(char[] finales) {
         String resultado = "";
         while (posicionActual < texto.Length) {
            char c = texto[posicionActual++];
            if (caracteresCadena.IndexOf(c)>=0) {
               noEntreComillas = !noEntreComillas;
            }
            if (noEntreComillas) {
               foreach (char final in finales) {
                  if (final == c) {
                     return new KeyValuePair<char, String>(final, resultado);
                  }
               }
            }
            bool correcto = true;
            for(int i=0;i<nulos.Length && correcto; i++) {
               correcto = c != nulos[i];
            }
            if (correcto) { 
               resultado += c;
            }
         }
         return new KeyValuePair<char, String>('\0',resultado);
      }
   }
}
