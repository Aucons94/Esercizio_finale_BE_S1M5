using System.Web;
using System.Web.Mvc;

namespace Eserciziofinale_S1M5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
