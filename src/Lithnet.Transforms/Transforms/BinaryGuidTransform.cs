using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Lithnet.MetadirectoryServices;

namespace Lithnet.Transforms
{
    /// <summary>
    /// Converts binary GUIDs to strings and vice-versa
    /// </summary>
    [DataContract(Name = "binary-guid", Namespace = "http://lithnet.local/Lithnet.IdM.Transforms/v1/")]
    [System.ComponentModel.Description("Binary GUID converter")]
    public class BinaryGuidTransform : Transform
    {
        /// <summary>
        /// Initializes a new instance of the BinaryGuidTransform class
        /// </summary>
        public BinaryGuidTransform()
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
                yield return ExtendedAttributeType.Binary;
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
                yield return ExtendedAttributeType.Binary;
            }
        }

        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Performs the transformation against the specified value
        /// </summary>
        /// <param name="inputValue">The incoming value to transform</param>
        /// <returns>The transformed value</returns>
        protected override object TransformSingleValue(object inputValue)
        {
            if (inputValue is byte[] b)
            {
                Guid g = new Guid(b);
                string format = string.IsNullOrWhiteSpace(this.Format) ? "D" : this.Format;
                return g.ToString(format);
            }

            if (inputValue is string s)
            {
                Guid g = new Guid(s);
                return g.ToByteArray();
            }

            throw new UnknownOrUnsupportedDataTypeException("The value passed to the transform was not a string or byte array");

        }
    }
}
