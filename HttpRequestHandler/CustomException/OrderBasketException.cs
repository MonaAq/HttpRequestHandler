using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace HttpRequestHandler.CustomException
{
    public class OrderBasketException : SchedulerException
    {
        public OrderBasketException(string message) : base(message)
        {

        }

        public OrderBasketException(ModelStateDictionary modelState) : base(modelState)
        {

        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
