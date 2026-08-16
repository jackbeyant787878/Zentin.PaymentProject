using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zentin.Payment.Application.Contracts
{
    /// <summary>
    /// supports dynamic extension fields
    /// </summary>
    [JsonSerializable(typeof(ExtensibleDto))]
    public abstract class ExtensibleDto
    {
        [JsonExtensionData]
        private IDictionary<string, object>? _extensionData;

        [JsonIgnore]
        public IDictionary<string, object> ExtensionData
        {
            get => _extensionData ??= new Dictionary<string, object>();
            set => _extensionData = value;
        }

        public void AddExtension(string key, object? value)
        {
            ExtensionData[key] = value ?? string.Empty;
        }
    }
}
