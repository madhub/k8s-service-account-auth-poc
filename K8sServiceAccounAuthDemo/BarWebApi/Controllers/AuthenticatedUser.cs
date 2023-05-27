using System.Xml.Linq;

namespace FooWebApi.Controllers;

record AuthenticatedUser(IList<string> audiences, string username, string podName);

