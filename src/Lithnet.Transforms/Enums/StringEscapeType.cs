// -----------------------------------------------------------------------
// <copyright file="StringCase.cs" company="Ryan Newington">
// Copyright (c) 2013 Ryan Newington
// </copyright>
// -----------------------------------------------------------------------

namespace Lithnet.Transforms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// The type of casing to apply to a string
    /// </summary>
    [DataContract(Name = "string-escape-type", Namespace = "http://lithnet.local/Lithnet.IdM.Transforms/v1/")]
    public enum StringEscapeType
    {
        /// <summary>
        /// Escapes XML element
        /// </summary>
        [Description("Escape XML element")]
        [EnumMember(Value = "xml-element")]
        XmlElement = 0,
        
        /// <summary>
        /// Escape DN components
        /// </summary>
        [Description("Escape LDAP DN component")]
        [EnumMember(Value = "ldap-dn")]
        LdapDN = 1,

        /// <summary>
        /// Escapes XML attribute
        /// </summary>
        [Description("Escape XML attribute")]
        [EnumMember(Value = "xml-attribute")]
        XmlAttribute = 2,

        /// <summary>
        /// Escapes XML element
        /// </summary>
        [Description("Unescape XML")]
        [EnumMember(Value = "unescape-xml-element")]
        XmlUnescape = 3,

        /// <summary>
        /// Escapes XML element
        /// </summary>
        [Description("Escape HTML element")]
        [EnumMember(Value = "escape-html-element")]
        HtmlEscape = 5,

        /// <summary>
        /// Escapes XML element
        /// </summary>
        [Description("Unescape HTML element")]
        [EnumMember(Value = "unescape-html-element")]
        HtmlUnescape = 6,
    }
}
