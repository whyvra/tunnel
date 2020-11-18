using System;
using System.Linq;
using System.Linq.Expressions;

namespace Whyvra.Blazor.Forms.Infrastructure
{
    public static class ReflectionExtensions
    {
        public static Func<object, object> GetGetter<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            var expr = lambda.Compile();
            return x => expr((TModel) x);
        }

        public static Action<object, object> GetSetter<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            var setter = CreateSetter(lambda).Compile();
            return (x, y) => setter((TModel) x, (TProperty) y);
        }

        private static Expression<Action<TModel, TProperty>> CreateSetter<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var value = Expression.Parameter(typeof(TProperty));
            var body = Expression.Assign(selector.Body, value);

            return Expression.Lambda<Action<TModel, TProperty>>(body,
                selector.Parameters.Single(),
                value);
        }
    }
}