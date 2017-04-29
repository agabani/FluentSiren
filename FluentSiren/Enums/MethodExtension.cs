using System;

namespace FluentSiren.Enums
{
    internal static class MethodExtension
    {
        internal static string GetName(this Method? method)
        {
            return method?.GetName();
        }

        internal static string GetName(this Method method)
        {
            switch (method)
            {
                case Method.Delete:
                    return "DELETE";
                case Method.Get:
                    return "GET";
                case Method.Patch:
                    return "PATCH";
                case Method.Post:
                    return "POST";
                case Method.Put:
                    return "PUT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }
    }
}