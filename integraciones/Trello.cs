using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajería.integraciones {
   class Trello {
      static public JSON cogerTablero(string id) {
         /*curl--request GET \
  --url 'https://api.trello.com/1/boards/id?actions=all&boardStars=none&cards=none&card_pluginData=false&checklists=none&customFields=false&fields=name%2Cdesc%2CdescData%2Cclosed%2CidOrganization%2Cpinned%2Curl%2CshortUrl%2Cprefs%2ClabelNames&lists=open&members=none&memberships=none&membersInvited=none&membersInvited_fields=all&pluginData=false&organization=false&organization_pluginData=false&myPrefs=false&tags=false&key=yourApiKey&token=yourApiToken'/**/
         JSON respuesta = new JSON();
         ArrayList cabeceras = new ArrayList();
         respuesta.cargarJson("https://api.trello.com/1/boards/" + id + "?token=" + Configuracion.parametro("toke_trello")+"&key="+ Configuracion.parametro("key_trello"), cabeceras);

         return respuesta;
      }
      static public JSON cogerTableros(string id) {
         /*
          curl --request GET \
  --url 'https://api.trello.com/1/organizations/id/boards?filter=all&fields=all&key=yourApiKey&token=yourApiToken'
          */
         JSON respuesta = new JSON();
         ArrayList cabeceras = new ArrayList();
         respuesta.cargarJson("https://api.trello.com/1/members/" + id+ "/boards?token=" + Configuracion.parametro("toke_trello") + "&key=" + Configuracion.parametro("key_trello"), cabeceras);

         return respuesta;
      }
   }
}
