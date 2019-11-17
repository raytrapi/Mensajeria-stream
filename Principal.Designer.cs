namespace Mensajería {
   partial class Principal {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
         this.botonNotificacion = new System.Windows.Forms.NotifyIcon(this.components);
         this.menuBandeja = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.mensajeDeBienvenidaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.agregador = new System.Windows.Forms.Timer(this.components);
         this.controlDirecto = new System.Windows.Forms.Timer(this.components);
         this.panelMensajes = new System.Windows.Forms.Panel();
         this.controlGitter = new System.Windows.Forms.Timer(this.components);
         this.horaLímiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
         this.horaLimite = new System.Windows.Forms.TextBox();
         this.panelHoraLimite = new System.Windows.Forms.Panel();
         this.label1 = new System.Windows.Forms.Label();
         this.menuBandeja.SuspendLayout();
         this.panelHoraLimite.SuspendLayout();
         this.SuspendLayout();
         // 
         // botonNotificacion
         // 
         this.botonNotificacion.ContextMenuStrip = this.menuBandeja;
         this.botonNotificacion.Icon = ((System.Drawing.Icon)(resources.GetObject("botonNotificacion.Icon")));
         this.botonNotificacion.Text = "Mensajería";
         this.botonNotificacion.Visible = true;
         // 
         // menuBandeja
         // 
         this.menuBandeja.ImageScalingSize = new System.Drawing.Size(19, 19);
         this.menuBandeja.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mensajeDeBienvenidaToolStripMenuItem,
            this.toolStripMenuItem1,
            this.horaLímiteToolStripMenuItem,
            this.toolStripSeparator1,
            this.cerrarToolStripMenuItem});
         this.menuBandeja.Name = "menuBandeja";
         this.menuBandeja.Size = new System.Drawing.Size(232, 88);
         // 
         // mensajeDeBienvenidaToolStripMenuItem
         // 
         this.mensajeDeBienvenidaToolStripMenuItem.Name = "mensajeDeBienvenidaToolStripMenuItem";
         this.mensajeDeBienvenidaToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
         this.mensajeDeBienvenidaToolStripMenuItem.Text = "Mensaje de Bienvenida";
         this.mensajeDeBienvenidaToolStripMenuItem.Click += new System.EventHandler(this.mensajeDeBienvenidaToolStripMenuItem_Click);
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
         // 
         // cerrarToolStripMenuItem
         // 
         this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
         this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
         this.cerrarToolStripMenuItem.Text = "Cerrar";
         this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.CerrarToolStripMenuItem_Click);
         // 
         // agregador
         // 
         this.agregador.Enabled = true;
         this.agregador.Interval = 2000;
         this.agregador.Tick += new System.EventHandler(this.Agregador_Tick);
         // 
         // controlDirecto
         // 
         this.controlDirecto.Enabled = true;
         this.controlDirecto.Interval = 2000;
         this.controlDirecto.Tick += new System.EventHandler(this.controlDirecto_Tick);
         // 
         // panelMensajes
         // 
         this.panelMensajes.Dock = System.Windows.Forms.DockStyle.Left;
         this.panelMensajes.Location = new System.Drawing.Point(0, 0);
         this.panelMensajes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.panelMensajes.Name = "panelMensajes";
         this.panelMensajes.Size = new System.Drawing.Size(645, 778);
         this.panelMensajes.TabIndex = 1;
         // 
         // controlGitter
         // 
         this.controlGitter.Enabled = true;
         this.controlGitter.Interval = 1000;
         this.controlGitter.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // horaLímiteToolStripMenuItem
         // 
         this.horaLímiteToolStripMenuItem.Name = "horaLímiteToolStripMenuItem";
         this.horaLímiteToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
         this.horaLímiteToolStripMenuItem.Text = "Hora límite";
         this.horaLímiteToolStripMenuItem.Click += new System.EventHandler(this.horaLímiteToolStripMenuItem_Click);
         // 
         // toolStripMenuItem1
         // 
         this.toolStripMenuItem1.Name = "toolStripMenuItem1";
         this.toolStripMenuItem1.Size = new System.Drawing.Size(228, 6);
         // 
         // horaLimite
         // 
         this.horaLimite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.horaLimite.Dock = System.Windows.Forms.DockStyle.Fill;
         this.horaLimite.Font = new System.Drawing.Font("Roboto", 11.26957F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.horaLimite.Location = new System.Drawing.Point(5, 24);
         this.horaLimite.Name = "horaLimite";
         this.horaLimite.Size = new System.Drawing.Size(151, 29);
         this.horaLimite.TabIndex = 2;
         this.horaLimite.Leave += new System.EventHandler(this.horaLimite_Leave);
         // 
         // panelHoraLimite
         // 
         this.panelHoraLimite.BackColor = System.Drawing.Color.White;
         this.panelHoraLimite.Controls.Add(this.horaLimite);
         this.panelHoraLimite.Controls.Add(this.label1);
         this.panelHoraLimite.Location = new System.Drawing.Point(661, 21);
         this.panelHoraLimite.Name = "panelHoraLimite";
         this.panelHoraLimite.Padding = new System.Windows.Forms.Padding(5);
         this.panelHoraLimite.Size = new System.Drawing.Size(161, 63);
         this.panelHoraLimite.TabIndex = 3;
         this.panelHoraLimite.Visible = false;
         // 
         // label1
         // 
         this.label1.Dock = System.Windows.Forms.DockStyle.Top;
         this.label1.Font = new System.Drawing.Font("Roboto Light", 10.01739F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(5, 5);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(151, 19);
         this.label1.TabIndex = 3;
         this.label1.Text = "Termina en:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Principal
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1648, 778);
         this.Controls.Add(this.panelHoraLimite);
         this.Controls.Add(this.panelMensajes);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.Name = "Principal";
         this.Opacity = 0.8D;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "Principal";
         this.TopMost = true;
         this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Principal_FormClosing);
         this.MouseLeave += new System.EventHandler(this.Principal_MouseLeave);
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Principal_MouseMove);
         this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Principal_MouseUp);
         this.menuBandeja.ResumeLayout(false);
         this.panelHoraLimite.ResumeLayout(false);
         this.panelHoraLimite.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.NotifyIcon botonNotificacion;
      private System.Windows.Forms.ContextMenuStrip menuBandeja;
      private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
      private System.Windows.Forms.Timer agregador;
      private System.Windows.Forms.Timer controlDirecto;
      private System.Windows.Forms.ToolStripMenuItem mensajeDeBienvenidaToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.Panel panelMensajes;
      private System.Windows.Forms.Timer controlGitter;
      private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem horaLímiteToolStripMenuItem;
      private System.Windows.Forms.TextBox horaLimite;
      private System.Windows.Forms.Panel panelHoraLimite;
      private System.Windows.Forms.Label label1;
   }
}