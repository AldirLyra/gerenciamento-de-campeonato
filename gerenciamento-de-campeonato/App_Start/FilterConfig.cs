using System.Web;
using System.Web.Mvc;

namespace gerenciamento_de_campeonato
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
