using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;

namespace Mensajería{
   class Datos {
      System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, Object>> datos;
      public Datos() {
         datos = new List<Dictionary<string, object>>();
      }
      public Dictionary<string,object> this[int idx] {
         get {
            if(idx>=0 && idx < datos.Count) {
               return datos[idx];
            }else if (idx == datos.Count) {
               datos.Add(new Dictionary<string, object>());
               return datos[idx];
            }

            return null;
         }
      }
      public int Length{
         get {
            return datos.Count;
         }
      }
   }
   class BD {
     private static MySql.Data.MySqlClient.MySqlConnection conexion = null;
      private static long _id = -1;
       //TODO: ESTO ESTÁ MAL NO UTUILIZAR PAR OTRAS COSAS
      public static Datos consulta(string consulta) {
         Datos resultado = new Datos();

         if (conexion == null) {
            conexion = new MySql.Data.MySqlClient.MySqlConnection("Database=" + System.Configuration.ConfigurationManager.AppSettings["base_datos"] + ";Data Source=" + System.Configuration.ConfigurationManager.AppSettings["servidor"] + ";User Id=" + System.Configuration.ConfigurationManager.AppSettings["usuario"] + ";Password=" + System.Configuration.ConfigurationManager.AppSettings["clave"]+ ";CharSet=utf8");
                //conexion.Open();
         }

         MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(consulta);
         comando.Connection = conexion;
         comando.Connection.Open();
         try {
            MySql.Data.MySqlClient.MySqlDataReader datos = comando.ExecuteReader();
            //int numeroFilas = 0;
            int iFila = 0;
            while (datos.Read()) {
               object[] fila = new object[datos.FieldCount];
               System.Collections.Generic.Dictionary<string, object> campos = resultado[iFila++];
               datos.GetValues(fila);
               for (int i = 0; i < datos.FieldCount; i++) {
                  campos[datos.GetName(i)] = fila[i];
               }
            }
            
         } catch(Exception ex) {
            throw ex;
         } finally {
            comando.Connection.Close();
         }
         
         return resultado;
      }
      /**
       * Ejecuta la consulta y devuelve el resultado , Ojo, se ha de cerrar la conexión
       * */
      public static int ejecutar(string consulta) { 
         if (conexion == null) { 
            conexion = new MySql.Data.MySqlClient.MySqlConnection("Database=" + System.Configuration.ConfigurationManager.AppSettings["base_datos"] + ";Data Source=" + System.Configuration.ConfigurationManager.AppSettings["servidor"] + ";User Id=" + System.Configuration.ConfigurationManager.AppSettings["usuario"] + ";Password=" + System.Configuration.ConfigurationManager.AppSettings["clave"]+";charset=utf8mb4");
            //conexion.Open();
         }
         int resultado = 0;
         MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(consulta);
         comando.Connection = conexion;
         comando.Connection.Open();
         try {
            resultado= comando.ExecuteNonQuery();
            _id=comando.LastInsertedId;
         } catch (Exception ex) {
            throw ex;
         } finally {
            comando.Connection.Close();
         }
         return resultado;
      }
      public static long id {
         get {
            return _id;
         }
      }
   }
}
