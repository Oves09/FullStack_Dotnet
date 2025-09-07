using System.Text.RegularExpressions;

namespace AdvancedRoutingApp.Constraints
{
    public class GuidRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value))
            {
                var stringValue = value?.ToString();
                return !string.IsNullOrEmpty(stringValue) && Guid.TryParse(stringValue, out _);
            }
            return false;
        }
    }
}