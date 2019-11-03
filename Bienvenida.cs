using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mensajería {
   public partial class Bienvenida : Form {
      public Bienvenida() {
         InitializeComponent();
         mensaje.Text = Principal.mensajeBienvenida;
      }

      private void Bienvenida_Leave(object sender, EventArgs e) {
         this.Close();
      }

      private void Bienvenida_Enter(object sender, EventArgs e) {
         mensaje.Text = Principal.mensajeBienvenida;
      }

      private void bCambiar_Click(object sender, EventArgs e) {
         Principal.mensajeBienvenida= mensaje.Text;
         this.Close();
      }
   }
}
