using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStoreAPI.Filters
{
    public class MySampleResultFilterAttribute : Attribute, IResultFilter
    {
        private readonly ILogger<MySampleResultFilterAttribute> _logger;
        private readonly string _name;

        private Guid _randomId;

        public MySampleResultFilterAttribute(ILogger<MySampleResultFilterAttribute> logger, string name = "Global")
        {
            _logger = logger;
            _randomId = Guid.NewGuid();
            _name = name;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation($"Result Filter - After - {_randomId}");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogInformation($"Result Filter - Before - {_randomId}");
        }
    }
}
