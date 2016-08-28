namespace Blongo.ModelBinding
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ObjectIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Metadata.IsComplexType)
            {
                return new ObjectIdModelBinder();
            }

            return null;
        }
    }
}