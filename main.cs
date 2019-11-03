namespace Mensajería {
   static class Mensajeria { 
      static void Main(string[] args) {
         bool debug = false;
         if (args.Length > 0) {
            debug = args[0] == "P";
         }
         Principal principal = new Principal(debug);
         
         principal.ShowDialog();
      }
   }
}