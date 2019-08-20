using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FrontEnd.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipWelcomeAttribute : Attribute, IFilterMetadata
    {

    }
}
