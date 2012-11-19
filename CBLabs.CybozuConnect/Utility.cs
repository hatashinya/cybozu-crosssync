using System;
using System.Xml;

namespace CBLabs.CybozuConnect
{
    public static class Utility
    {
        public static string HtmlEscape(string text)
        {
            return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&#039;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string XmlEscape(string text)
        {
            return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string XmlAttributeEscape(string text)
        {
            return HtmlEscape(text).Replace("\r", "&#xD;").Replace("\n", "&#xA;");
        }

        public static string FormatXSDDate(DateTime date)
        {
            return date.ToString("yyyy'-'MM'-'dd");
        }

        public static string FormatXSDDateTime(DateTime datetime)
        {
            return datetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
        }

        public static string FormatISO8601(DateTime datetime)
        {
            return datetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
        }

        public static DateTime ParseXSDDate(string text)
        {
            string[] values = text.Split(new char[] { '-', 'T', ' ' });
            if (values.Length < 3)
            {
                throw new CybozuException("Not xsd:date format.");
            }

            try
            {
                return new DateTime(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
            }
            catch (Exception)
            {
                throw new CybozuException("Not xsd:date format.");
            }
        }

        public static DateTime ParseXSDDateTime(string text, bool utc)
        {
            return ParseISO8601(text, utc);
        }

        public static DateTime ParseISO8601(string text, bool utc)
        {
            string[] values = text.Split(new char[] { '-', 'T', ':', 'Z' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < 5)
            {
                throw new CybozuException("Not ISO8601 format.");
            }

            try
            {
                int second = (values.Length < 6) ? 0 : int.Parse(values[5]);
                DateTimeKind kind = utc ? DateTimeKind.Utc : DateTimeKind.Local;
                return new DateTime(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]), second, kind);
            }
            catch (Exception)
            {
                throw new CybozuException("Not ISO8601 format.");
            }
        }

        public static string SafeAttribute(XmlNode node, string name)
        {
            XmlAttribute attr = node.Attributes[name];
            return (attr == null || string.IsNullOrEmpty(attr.Value)) ? string.Empty : attr.Value;
        }

        public static string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
        }
    }
}
