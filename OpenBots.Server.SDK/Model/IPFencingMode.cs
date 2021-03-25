/* 
 * OpenBots Server API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = OpenBots.Server.SDK.Client.SwaggerDateConverter;

namespace OpenBots.Server.SDK.Model
{
    /// <summary>
    /// Defines IPFencingMode
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
        public enum IPFencingMode
    {
        /// <summary>
        /// Enum AllowMode for value: AllowMode
        /// </summary>
        [EnumMember(Value = "AllowMode")]
        AllowMode = 1,
        /// <summary>
        /// Enum DenyMode for value: DenyMode
        /// </summary>
        [EnumMember(Value = "DenyMode")]
        DenyMode = 2    }
}
