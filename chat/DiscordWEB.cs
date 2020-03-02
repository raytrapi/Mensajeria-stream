using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensajería.chat {
   public partial class Validacion : Form {
      public Validacion() {
         InitializeComponent();

      }
      public void mostrar(string URL) {
         navegador.ScriptErrorsSuppressed = false;
         navegador.Url = new Uri(URL);
         this.ShowDialog();
      }
   }
}
