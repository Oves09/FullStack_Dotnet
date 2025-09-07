namespace ECommerceApp.Constraints
{
    public class CategoryConstraint : IRouteConstraint
    {
        private readonly string[] _validCategories = { 
            "electronics", "clothing", "books", "home", "sports", "toys", "beauty", "automotive" 
        };

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value))
            {
                var category = value?.ToString()?.ToLowerInvariant();
                return !string.IsNullOrEmpty(category) && _validCategories.Contains(category);
            }
            return false;
        }
    }
}