using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace LyncBillingBase.DataAccess
{
    public class Invoker
    {
        public  static Func<T, TReturn> BuildTypedGetter<T, TReturn>(PropertyInfo propertyInfo)
        {
            Func<T, TReturn> reflGet = (Func<T, TReturn>) Delegate.CreateDelegate(typeof(Func <T, TReturn>), propertyInfo.GetGetMethod());
            
            return reflGet;
        }


        public  static Action<T, TProperty> BuildTypedSetter<T, TProperty>(PropertyInfo propertyInfo)
        {
            Action<T, TProperty> reflSet = (Action<T, TProperty>) Delegate.CreateDelegate (typeof(Action <T, TProperty>), propertyInfo.GetSetMethod());

            return reflSet;
        }


        public  static Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            
            var info = propertyInfo.GetSetMethod();

            Type type = propertyInfo.PropertyType;

            var exTarget = Expression.Parameter(targetType, "t");
            
            var exValue = Expression.Parameter(typeof(object), "p");

            var condition = Expression.Condition(
                // test
                Expression.Equal(exValue, Expression.Constant(DBNull.Value)),
                // if true
                Expression.Default(type),
                // if false
                Expression.Convert(exValue, type)
            );

            var exBody = Expression.Call(
               Expression.Convert(exTarget, info.DeclaringType),
               info,
               condition
            );

            var lambda = Expression.Lambda<Action<T, object>>(exBody, exTarget, exValue);
            
            var action = lambda.Compile();

            return action;
        }


        public  static Func<T, object > BuildUntypedGetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            
            var info = propertyInfo.GetGetMethod();
            
            var type = info.ReturnType;
 
            var exTarget = Expression.Parameter (targetType, "t" );
            var exBody = Expression.Call (exTarget, info);
            var exBody2 = Expression.Convert (exBody, typeof(object));
            var lambda = Expression.Lambda<Func<T, object>>(exBody2, exTarget);
            
            var action = lambda.Compile ();

            return action;
        }

    }

}
