using App.Application;
using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.API.Filters
{
    public class NotFoundFilter<TEntity, TEntityId> : Attribute, IAsyncActionFilter
        where TEntity : BaseEntity<TEntityId>
        where TEntityId : struct
    {
        private readonly IGenericRepository<TEntity, TEntityId> _genericRepository;
        public NotFoundFilter(IGenericRepository<TEntity, TEntityId> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // context.ActionArguments bir Dictionary<string, object>'tir.
            //var idValue = context.ActionArguments.Values.FirstOrDefault();  //controller kısmındaki methodların aldığı ilk parametreyi getirir bize id gelenlerle işlem yapmak istiyoruz biz.

            //var idKey = context.ActionArguments.Keys.FirstOrDefault(); //Keys.FirstOrDefault() ifadesi ise, bu sözlükteki ilk parametrenin ismini (string olarak) getirir.

            var idValue = context.ActionArguments.TryGetValue("id", out var idAsObject) ? idAsObject : null;

            if (idAsObject is not TEntityId id)
            {
                await next();
                return;
            }

            if (await _genericRepository.AnyAsync(id))
            {
                await next();
                return;
            }

            var entityName = typeof(TEntity).Name;

            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"data bulunamamıştır({entityName})({actionName}).");

            context.Result = new NotFoundObjectResult(result);
        }
    }
}
