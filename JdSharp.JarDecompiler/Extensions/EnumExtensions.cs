using System;
using JdSharp.JarDecompiler.Enums;

namespace JdSharp.JarDecompiler.Extensions
{
    public static class EnumExtensions
    {
        public static string ToStringValue(this AccessFlagEnum accessFlagEnum)
            => accessFlagEnum switch
            {
                AccessFlagEnum.AccAbstract => "ACC_ABSTRACT",
                AccessFlagEnum.AccAnnotation => "ACC_ANNOTATION",
                AccessFlagEnum.AccEnum => "ACC_ENUM",
                AccessFlagEnum.AccPublic => "ACC_PUBLIC",
                AccessFlagEnum.AccFinal => "ACC_FINAL",
                AccessFlagEnum.AccSuper => "ACC_SUPER",
                AccessFlagEnum.AccInterface => "ACC_INTERFACE",
                AccessFlagEnum.AccSynthetic => "ACC_SYNTHETIC",
                _ => throw new ArgumentOutOfRangeException(nameof(accessFlagEnum), accessFlagEnum, "Not able to get string value from AccessFlagEnum")
            };

        public static string FasterToString<T>(this T enumValue) where T : Enum
        {
            if (enumValue is null) 
                throw new ArgumentNullException(nameof(enumValue));
            return nameof(enumValue);
        }
    }
}