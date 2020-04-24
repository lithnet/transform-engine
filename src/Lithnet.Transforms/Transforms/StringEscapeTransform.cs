using System.Web;

namespace Lithnet.Transforms
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Lithnet.MetadirectoryServices;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Escapes a string using the specified escaping rules
    /// </summary>
    [DataContract(Name = "string-escape", Namespace = "http://lithnet.local/Lithnet.IdM.Transforms/v1/")]
    [System.ComponentModel.Description("String escape")]
    public class StringEscapeTransform : Transform
    {
        private static Dictionary<string, string> replacements = new Dictionary<string, string>()
                { {@"\", @"\\"},
                  {@"#", @"\#"},
                  {@"+", @"\+"},
                  {@";", @"\;"},
                  {@"=", @"\="},
                  {"\"", "\\\""},
                  {@"<", @"\<"},
                  {@",", @"\,"},
                  {@">", @"\>"}};

        /// <summary>
        /// Initializes a new instance of the StringCaseTransform class
        /// </summary>
        public StringEscapeTransform()
        {
        }

        /// <summary>
        /// Defines the data types that this transform may return
        /// </summary>
        public override IEnumerable<ExtendedAttributeType> PossibleReturnTypes
        {
            get
            {
                yield return ExtendedAttributeType.String;
            }
        }

        /// <summary>
        /// Defines the input data types that this transform allows
        /// </summary>
        public override IEnumerable<ExtendedAttributeType> AllowedInputTypes
        {
            get
            {
                yield return ExtendedAttributeType.String;
                yield return ExtendedAttributeType.Boolean;
                yield return ExtendedAttributeType.Integer;
            }
        }

        /// <summary>
        /// Gets or sets the type of escaping to apply to the string
        /// </summary>
        [DataMember(Name = "escape-type")]
        public StringEscapeType EscapeType { get; set; }

        /// <summary>
        /// Executes the transformation against the specified value
        /// </summary>
        /// <param name="inputValue">The incoming value to transform</param>
        /// <returns>The transformed value</returns>
        protected override object TransformSingleValue(object inputValue)
        {
            return this.EscapeValue(TypeConverter.ConvertData<string>(inputValue));
        }

        /// <summary>
        /// Escapes the specified string
        /// </summary>
        /// <param name="value">The incoming value to transform</param>
        /// <returns>A copy of the original value with its case modified</returns>
        private object EscapeValue(string value)
        {
            switch (this.EscapeType)
            {
                case StringEscapeType.XmlElement:
                    return this.EscapeXmlElement(value);

                case StringEscapeType.XmlAttribute:
                    return this.EscapeXmlAttribute(value);

                case StringEscapeType.LdapDN:
                    return this.EscapeLdapDN(value);

                case StringEscapeType.HtmlEscape:
                    return this.EscapeHtml(value);

                case StringEscapeType.HtmlUnescape:
                    return this.UnescapeHtml(value);

                case StringEscapeType.XmlUnescape:
                    return this.UnescapeXml(value);

                default:
                    throw new ArgumentException();
            }
        }

        private string EscapeHtml(object value)
        {
            string s = value as string;
            return string.IsNullOrEmpty(s) ? s : System.Net.WebUtility.HtmlEncode(s);
        }

        private string UnescapeHtml(object value)
        {
            string s = value as string;
            return string.IsNullOrEmpty(s) ? s : System.Net.WebUtility.HtmlDecode(s);
        }

        private string EscapeXmlElement(object value)
        {
            string s = value as string;

            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        private string UnescapeXml(object value)
        {
            string s = value as string;

            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }

        private string EscapeXmlAttribute(string value)
        {
            return this.EscapeXmlElement(value).Replace("&apos;", "'");
        }

        private string EscapeLdapDN(string s)
        {
            string ret = s;

            //escape the chars that need to be escaped
            foreach (var pair in StringEscapeTransform.replacements)
            {
                ret = ret.Replace(pair.Key, pair.Value);
            }

            var whiteSpaceEscapeChars = @"\";
            //escape leading white space
            int whiteSpaceCount = 0;
            while (whiteSpaceCount < ret.Length && Char.IsWhiteSpace(ret[whiteSpaceCount]))
            {
                ret = String.Format("{0}{1}{2}", ret.Substring(0, whiteSpaceCount), whiteSpaceEscapeChars,
                    ret.Substring(whiteSpaceCount));
                whiteSpaceCount += 1 + whiteSpaceEscapeChars.Length;
            }

            //escape trailing whitespace
            if (whiteSpaceCount < ret.Length)
            {
                whiteSpaceCount = ret.Length - 1;
                while (whiteSpaceCount >= 0 && Char.IsWhiteSpace(ret[whiteSpaceCount]))
                {
                    ret = String.Format("{0}{1}{2}", ret.Substring(0, whiteSpaceCount), whiteSpaceEscapeChars,
                        ret.Substring(whiteSpaceCount));
                    whiteSpaceCount--;
                }
            }

            return ret;
        }
    }
}