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
                AccessFlagEnum.AccPrivate => "ACC_PRIVATE",
                AccessFlagEnum.AccProtected => "ACC_PROTECTED",
                AccessFlagEnum.AccStatic => "ACC_STATIC",
                AccessFlagEnum.AccBridge => "ACC_BRIDGE",
                AccessFlagEnum.AccVarargs => "ACC_VARARGS",
                AccessFlagEnum.AccNative => "ACC_NATIVE",
                AccessFlagEnum.AccStrict => "ACC_STRICT",
                _ => throw new ArgumentOutOfRangeException(nameof(accessFlagEnum), accessFlagEnum,
                    "Not able to get string value from AccessFlagEnum")
            };

        public static string ToJavaClass(this AccessFlagEnum accessFlagEnum)
            => accessFlagEnum switch
            {
                AccessFlagEnum.AccAbstract => "abstract",
                AccessFlagEnum.AccAnnotation => "@",
                AccessFlagEnum.AccEnum => "enum",
                AccessFlagEnum.AccPublic => "public",
                AccessFlagEnum.AccFinal => "final",
                AccessFlagEnum.AccInterface => "interface",
                AccessFlagEnum.AccPrivate => "private",
                AccessFlagEnum.AccProtected => "protected",
                AccessFlagEnum.AccStatic => "static",
                AccessFlagEnum.AccNative => "native",
                _ => string.Empty
            };

        public static string ToJavaField(this AccessFlagEnum accessFlagEnum)
            => accessFlagEnum switch
            {
                AccessFlagEnum.AccAbstract => "abstract",
                AccessFlagEnum.AccPublic => "public",
                AccessFlagEnum.AccFinal => "final",
                AccessFlagEnum.AccInterface => "interface",
                AccessFlagEnum.AccPrivate => "private",
                AccessFlagEnum.AccProtected => "protected",
                AccessFlagEnum.AccStatic => "static",
                AccessFlagEnum.AccBridge => "volatile",
                AccessFlagEnum.AccVarargs => "transient",
                _ => string.Empty
            };

        public static string ToJavaMethod(this AccessFlagEnum accessFlagEnum)
            => accessFlagEnum switch
            {
                AccessFlagEnum.AccPublic => "public",
                AccessFlagEnum.AccProtected => "protected",
                AccessFlagEnum.AccPrivate => "private",
                AccessFlagEnum.AccNative => "native",
                AccessFlagEnum.AccFinal => "final",
                AccessFlagEnum.AccStatic => "static",
                AccessFlagEnum.AccSuper => "synchronized",
                AccessFlagEnum.AccAbstract => "abstract",
                _ => string.Empty
            };
    }
}