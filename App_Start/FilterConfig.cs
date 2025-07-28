using System.Web;
using System.Web.Mvc;

namespace Proyecto_2___Paula_Ulate_Medrano
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
