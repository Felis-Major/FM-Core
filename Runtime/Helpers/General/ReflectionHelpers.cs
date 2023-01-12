using System;
using System.Linq;
using System.Reflection;

namespace FM.Runtime.Helpers.Reflection
{
    public static class ReflectionHelpers
    {
        public static T GetAttributeForType<T>(Type objectType) where T : Attribute
        {
            return objectType.GetCustomAttribute<T>();
        }

        public static T[] GetAttributesForType<T>(Type objectType) where T : Attribute
        {
            var attributes = objectType.GetCustomAttributes<T>();
            return attributes.ToArray();
        }

        public static object CallGenericMethod<T>(string name, object source, object[] args, params Type[] types)
        {
            // Instantiate node using reflection
            var method = typeof(T).GetMethod(name);
            var action = method.MakeGenericMethod(types);
            return action.Invoke(source, args);
        }
    }
}