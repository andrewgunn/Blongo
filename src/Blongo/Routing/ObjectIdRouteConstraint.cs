using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace Blongo.Routing
{
    public class ObjectIdRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;

            if (!values.TryGetValue(routeKey, out value) && value != null)
            {
                return false;
            }

            ObjectId objectId;

            return ObjectId.TryParse(value.ToString(), out objectId);
        }
    }
}
