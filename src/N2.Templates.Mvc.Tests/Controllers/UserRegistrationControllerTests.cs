using N2.Templates.Mvc.Controllers;
using N2.Templates.Mvc.Items.Items;
using NUnit.Framework;
using MvcContrib.TestHelper;

namespace N2.Templates.Mvc.Tests.Controllers
{
	[TestFixture]
	public class UserRegistrationControllerTests
	{
		[Test]
		public void Index()
		{
			var controller = new UserRegistrationController(null, null)
			                 	{
			                 		CurrentItem = new UserRegistration()
			                 	};

			var result = controller.Index()
				.AssertViewRendered();

			Assert.That(result.ViewName, Is.EqualTo("~/Views/Shared/Register.ascx"));
		}
	}
}