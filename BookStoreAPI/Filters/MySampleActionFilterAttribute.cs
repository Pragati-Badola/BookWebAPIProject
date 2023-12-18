using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStoreAPI.Filters
{
    public class MySampleActionFilterAttribute : Attribute, IActionFilter, IOrderedFilter
    {
        private readonly string _name;
        public int Order
        {
            get; set;
        }

        public MySampleActionFilterAttribute(string name, int order = 0) {
            this._name = name;
            this.Order = order;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Action Filter - After - {_name} {Order}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Action Filter - Before - {_name} {Order}");
        }
    }
}
