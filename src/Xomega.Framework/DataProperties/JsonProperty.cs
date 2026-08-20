// Copyright (c) 2026 Xomega.Net. All rights reserved.

using System.Text.Json;

namespace Xomega.Framework.Properties
{
    /// <summary>
    /// A data property that holds a string value with JSON content, and displays it
    /// as a formatted (pretty-printed) JSON string when it's not editable.
    /// </summary>
    public class JsonProperty : TextProperty
    {
        /// <summary>
        /// Options used to serialize the JSON value with indentation for display purposes.
        /// </summary>
        protected static readonly JsonSerializerOptions PrettyPrintOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        ///  Constructs a JsonProperty with a given name and adds it to the parent data object under this name.
        /// </summary>
        /// <param name="parent">The parent data object to add the property to if applicable.</param>
        /// <param name="name">The property name that should be unique within the parent data object.</param>
        public JsonProperty(DataObject parent, string name)
            : base(parent, name)
        {
            Validator += ValidateJson;
        }

        /// <summary>
        /// Converts the value to a given format. For the <see cref="ValueFormat.DisplayString"/> format
        /// the value is parsed as JSON and reformatted with indentation for a better readability.
        /// If the value is not a valid JSON string, it is returned unchanged.
        /// </summary>
        /// <param name="value">A single value to convert to the given format.</param>
        /// <param name="format">The value format to convert the value to.</param>
        /// <returns>The value converted to the given format.</returns>
        protected override object ConvertValue(object value, ValueFormat format)
        {
            if (format == ValueFormat.DisplayString && value is string json)
            {
                try
                {
                    using (var doc = JsonDocument.Parse(json))
                    {
                        return JsonSerializer.Serialize(doc, PrettyPrintOptions);
                    }
                }
                catch (JsonException)
                {
                    return value;
                }
            }
            return base.ConvertValue(value, format);
        }

        /// <summary>
        /// A validation function that checks if the value is a well-formed JSON string
        /// and reports a validation error if it is not.
        /// </summary>
        /// <param name="dp">Data property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <param name="row">The row in a list object or null for regular data objects.</param>
        public static void ValidateJson(DataProperty dp, object value, DataRow row)
        {
            if (dp == null || value == null) return;
            var json = value as string;
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                using (var doc = JsonDocument.Parse(json))
                {
                }
            }
            catch (JsonException)
            {
                dp.AddValidationError(row, Messages.Validation_JsonFormat, dp);
            }
        }
    }
}
