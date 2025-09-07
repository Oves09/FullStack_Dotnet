using System.Text.RegularExpressions;

namespace ECommerceApp.Constraints
{
    public class PriceRangeConstraint : IRouteConstraint
    {
        private readonly Regex _priceRangePattern = new Regex(@"^(\d+)-(\d+)$", RegexOptions.Compiled);

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value))
            {
                var priceRange = value?.ToString();
                if (!string.IsNullOrEmpty(priceRange))
                {
                    var match = _priceRangePattern.Match(priceRange);
                    if (match.Success)
                    {
                        var minPrice = int.Parse(match.Groups[1].Value);
                        var maxPrice = int.Parse(match.Groups[2].Value);
                        return minPrice < maxPrice && minPrice >= 0 && maxPrice <= 10000;
                    }
                }
            }
            return false;
        }
    }
}