using System;
using System.Security.Principal;
using System.Web.UI;

namespace N2.Definitions
{
	/// <summary>
	/// Classes implementing this interface can add a graphical representation to 
	/// a control hierarchy.
	/// </summary>
	public interface IContainable : IUniquelyNamed, IComparable<IContainable>
    {
        /// <summary>Gets or sets the name of a container containing this container.</summary>
        string ContainerName { get; set;}

        /// <summary>The order of this container compared to other containers and editors. Editors within the container are sorted according to their sort order.</summary>
        int SortOrder { get; set; }

        /// <summary>Adds a containable control to a container and returns it.</summary>
        /// <param name="context">The context containing the container onto which to add the containable control.</param>
		/// <remarks>The added control must be added to the Control property of the context.</remarks>
        void AddTo(ContainableContext context);
    }
}
