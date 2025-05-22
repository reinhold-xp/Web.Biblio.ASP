using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Biblio.WebUI.Filters
{
    public class LocalizedViewAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string defaultLang = "fr";

            var routeData = filterContext.RouteData.Values;

            string lang = (string)routeData["lang"];
            if (!String.IsNullOrEmpty(lang))
            {
                defaultLang = lang;
            }

            var viewResult = filterContext.Result as ViewResultBase;
            if (viewResult != null)
            {
                if (string.IsNullOrWhiteSpace(viewResult.ViewName))
                {
                    viewResult.ViewName = filterContext.RouteData.GetRequiredString("action");
                }

                var v = ViewEngines.Engines.FindView(
                    filterContext.Controller.ControllerContext,
                    viewResult.ViewName + "_" + defaultLang, null
                    );
                if (v.View != null)
                    viewResult.ViewName += "_" + defaultLang;
            }
            base.OnResultExecuting(filterContext);
        }
    }
}