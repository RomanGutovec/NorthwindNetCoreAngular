using System;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.MVC.Common
{
    public class LogThisActionAttribute : TypeFilterAttribute
    {
        public LogThisActionAttribute(Type type) : base(type)
        {
            Arguments = new object[] {true};
        }
    }
}
