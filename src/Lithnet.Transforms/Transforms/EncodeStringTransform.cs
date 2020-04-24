﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Lithnet.MetadirectoryServices;

namespace Lithnet.Transforms
{
    /// <summary>
    /// Encodes a value as a string
    /// </summary>
    [DataContract(Name = "encode-string", Namespace = "http://lithnet.local/Lithnet.IdM.Transforms/v1/")]
    [System.ComponentModel.Description("String encode")]
    public class EncodeStringTransform : Transform
    {
        /// <summary>
        /// Initializes a new instance of the EncodeStringTransform class
        /// </summary>
        public EncodeStringTransform()
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
                yield return ExtendedAttributeType.Integer;
            }
        }

        /// <summary>
        /// Gets or sets the type of hash to apply to the incoming value
        /// </summary>
        [DataMember(Name = "encode-format")]
        public StringEncodeFormat EncodeFormat { get; set; }

        /// <summary>
        /// Executes the transformation against the specified value
        /// </summary>
        /// <param name="inputValue">The incoming value to transform</param>
        /// <returns>The transformed value</returns>
        protected override object TransformSingleValue(object inputValue)
        {
            return this.EncodeValue(inputValue);
        }

        /// <summary>
        /// Hashes the value using the specified hash type
        /// </summary>
        /// <param name="value">The incoming value to transform</param>
        /// <returns>The transformed value</returns>
        private object EncodeValue(object value)
        {
            switch (this.EncodeFormat)
            {
                case StringEncodeFormat.Base32:
                    return this.EncodeBase32(value);

                case StringEncodeFormat.Base64:
                    return this.EncodeBase64(value);

                default:
                    return this.GetInputBytes(value, this.EncodeFormat);
            }
        }

        /// <summary>
        /// Converts the value to a base32 string
        /// </summary>
        /// <param name="value">The string, integer, or binary value to hash</param>
        /// <returns>The encoded value</returns>
        private string EncodeBase32(object value)
        {
            byte[] input = this.GetInputBytes(value, StringEncodeFormat.UTF8);
            return Base32Encoding.ToString(input);
        }

        private string EncodeBase64(object value)
        {
            byte[] input = this.GetInputBytes(value, StringEncodeFormat.UTF8);
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// Converts the input string, integer or binary value to its byte[] representation
        /// </summary>
        /// <param name="input">A string, integer, or binary value</param>
        /// <returns>A byte array representing the input value</returns>
        private byte[] GetInputBytes(object input, StringEncodeFormat format)
        {
            if (input == null)
            {
                return null;
            }

            if (input is byte[] bytes)
            {
                return bytes;
            }

            if (input is string inputString)
            {
                if (string.IsNullOrWhiteSpace(inputString))
                {
                    return null;
                }

                switch (format)
                {
                    case StringEncodeFormat.UTF7:
                        return Encoding.UTF7.GetBytes(inputString);

                    case StringEncodeFormat.UTF8:
                        return Encoding.UTF8.GetBytes(inputString);

                    case StringEncodeFormat.UTF16:
                        return Encoding.Unicode.GetBytes(inputString);

                    case StringEncodeFormat.UTF32:
                        return Encoding.UTF32.GetBytes(inputString);

                    case StringEncodeFormat.ASCII:
                        return Encoding.ASCII.GetBytes(inputString);

                    default:
                        throw new InvalidOperationException();
                }
            }

            if (input is long || input is int)
            {
                long inputInt = (long)input;
                return inputInt == 0 ? null : BitConverter.GetBytes(inputInt);
            }

            throw new InvalidOperationException("The data type is unknown");
        }
    }
}