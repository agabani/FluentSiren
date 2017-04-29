using System;

namespace FluentSiren.Enums
{
    internal static class TypeExtensions
    {
        internal static string GetName(this Type? type)
        {
            return type?.GetName();
        }

        internal static string GetName(this Type type)
        {
            switch (type)
            {
                case Type.Hidden:
                    return "hidden";
                case Type.Text:
                    return "text";
                case Type.Search:
                    return "search";
                case Type.Tel:
                    return "tel";
                case Type.Url:
                    return "url";
                case Type.Email:
                    return "email";
                case Type.Password:
                    return "password";
                case Type.DateTime:
                    return "datetime";
                case Type.Date:
                    return "date";
                case Type.Month:
                    return "month";
                case Type.Week:
                    return "week";
                case Type.Time:
                    return "time";
                case Type.DateTimeLocal:
                    return "datetime-local";
                case Type.Number:
                    return "number";
                case Type.Range:
                    return "range";
                case Type.Color:
                    return "color";
                case Type.Checkbox:
                    return "checkbox";
                case Type.Radio:
                    return "radio";
                case Type.File:
                    return "file";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}