using System;

namespace JdSharp.Core;

public static class TypeExtensions
{
    public static Type GetImplementingClass(this Type type, Type interfaceType)
    {
        Type baseType = null;
        
        if (type.BaseType != null)
            baseType = GetImplementingClass(type.BaseType, interfaceType);

        if (baseType is not null) 
            return baseType;
        
        if (interfaceType.IsAssignableFrom(type))
            return type;

        return null;
    }
}