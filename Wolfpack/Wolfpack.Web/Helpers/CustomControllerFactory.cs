using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wolfpack.Data;

namespace Wolfpack.Web.Helpers
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                var context = new Context();
                var controller = Activator.CreateInstance(controllerType, new[] { context }) as Controller;
                return controller;
            }
            else
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}