using System.Web.Mvc;
using N2.Web.Mvc;

namespace N2.Templates.Mvc.Areas.Blog
{
    /// <summary>
    /// Class for area registration.
    /// </summary>
    public class BlogAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to be registered.
        /// </summary>
        /// <returns>The name of the area to be registered.</returns>
        public override string AreaName
        {
            get
            {
                return "Blog";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapContentRoute<Models.Pages.BlogBase>();
        }
    }
}
