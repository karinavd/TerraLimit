using System.Reflection;
using TerraLimit.Api.DTOs;
using TerraLimit.Api.Models;

namespace TerraLimit.Api.Extensions
{
    public static class MyMapper
    {
        public static TDest MapTo<TDest>(object source) where TDest : new()
        {
            if (source is null) return default!;

            var destination = new TDest();

            Type sourceType = source.GetType();
            Type destType = typeof(TDest);

            foreach (PropertyInfo sourceProp in sourceType.GetProperties())
            {
                PropertyInfo? destProp = destType.GetProperty(sourceProp.Name);

                if (destProp is not null && destProp.CanWrite && destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                {
                    var value = sourceProp.GetValue(source);

                    destProp.SetValue(destination, value);
                }
            }
            return destination;
        }
    }
}