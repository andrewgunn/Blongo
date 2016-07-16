using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Blongo.ModelBinding
{
    public class ObjectIdModelBinder : IModelBinder
    {

        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;

            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                modelType = modelType.GetGenericArguments()[0];
            }

            if (modelType != typeof(ObjectId))
            {
                return Task.CompletedTask;
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                var model = ObjectId.Parse(valueProviderResult.ToString());
             
                bindingContext.Result = ModelBindingResult.Success(model);

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    exception,
                    bindingContext.ModelMetadata);

                return Task.CompletedTask;
            }
        }
    }
}
