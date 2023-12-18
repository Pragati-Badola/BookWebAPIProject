using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Xml.Linq;

namespace BookStoreAPI.Filters
{
    public class MySampleResourceFilterAttribute : Attribute, IResourceFilter
    {
        private readonly string _name;
 
        public MySampleResourceFilterAttribute(string name, int order = 0)
        {
            this._name = name;
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"Resource Filter - After - {_name}");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.Result = new ContentResult()
            {
                Content = "This is a shortcircuited pipeline"
        };
            Console.WriteLine($"Resource Filter - Before - {_name}");
        }
    }
}
