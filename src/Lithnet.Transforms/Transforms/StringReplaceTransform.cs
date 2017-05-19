using System.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Lithnet.MetadirectoryServices;

namespace Lithnet.Transforms
{
    /// <summary>
    /// Performs a lookup against an internal table within the class and returns a replacement value
    /// </summary>
    [DataContract(Name = "string-replace", Namespace = "http://lithnet.local/Lithnet.IdM.Transforms/v1/")]
    [System.ComponentModel.Description("String find and replace")]
    public class StringReplaceTransform : Transform
    {
        /// <summary>
        /// Initializes a new instance of the SimpleLookupTransform class
        /// </summary>
        public StringReplaceTransform()
        {
            this.Initialize();
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
                yield return ExtendedAttributeType.Binary;
                yield return ExtendedAttributeType.Boolean;
                yield return ExtendedAttributeType.Integer;
            }
        }

        [DataMember(Name = "ignore-case")]
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Gets or sets the list of lookup items
        /// </summary>
        [DataMember(Name = "lookup-items")]
        public ObservableCollection<LookupItem> LookupItems { get; set; }

        /// <summary>
        /// Performs the transformation against the specified value
        /// </summary>
        /// <param name="inputValue">The incoming value to transform</param>
        /// <returns>The transformed value</returns>
        protected override object TransformSingleValue(object inputValue)
        {
            return this.LookupReplacement(TypeConverter.ConvertData<string>(inputValue));
        }

        private static string ReplaceValue(string original, string findValue, string replaceValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = original.IndexOf(findValue, comparison);

            while (index != -1)
            {
                sb.Append(original.Substring(previousIndex, index - previousIndex));
                sb.Append(replaceValue);
                index += findValue.Length;

                previousIndex = index;
                index = original.IndexOf(findValue, index, comparison);
            }

            sb.Append(original.Substring(previousIndex));
            return sb.ToString();
        }

        /// <summary>
        /// Gets the replacement value from the internal lookup table
        /// </summary>
        /// <param name="inputValue">The value to lookup</param>
        /// <returns>The replacement value</returns>
        private object LookupReplacement(string inputValue)
        {
            foreach (LookupItem item in this.LookupItems)
            {
                inputValue = StringReplaceTransform.ReplaceValue(inputValue, item.CurrentValue, item.NewValue, this.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            }

            return inputValue;
        }

        /// <summary>
        /// Initializes the class
        /// </summary>
        private void Initialize()
        {
            this.LookupItems = new ObservableCollection<LookupItem>();
        }

        /// <summary>
        /// Performs pre-deserialization initialization
        /// </summary>
        /// <param name="context">The current serialization context</param>
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            this.Initialize();
        }
    }
}
