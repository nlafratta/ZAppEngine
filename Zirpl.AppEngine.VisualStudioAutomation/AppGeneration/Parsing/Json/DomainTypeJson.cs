﻿using System;
using System.Collections.Generic;

namespace Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.Parsing.Json
{
    internal sealed partial class JsonTypes
    {
        public class DomainTypeJson
        {
            public DomainTypeJson()
            {
                this.EnumValues = new List<EnumValueJson>();
                this.Properties = new List<PropertyJson>();
                this.Properties = new List<PropertyJson>();
            }

            public IdJson Id { get; set; }
            public bool? IsPersistable { get; set; }
            public String PluralName { get; set; }
            public String InheritsFrom { get; set; }
            public bool? IsStaticLookup { get; set; }
            public bool? IsAbstract { get; set; }
            public bool? IsVersionable { get; set; }
            public bool? IsAuditable { get; set; }
            public bool? IsExtensible { get; set; }
            public bool? IsInsertable { get; set; }
            public bool? IsUpdatable { get; set; }
            public bool? IsDeletable { get; set; }
            public bool? IsMarkDeletable { get; set; }
            public IList<EnumValueJson> EnumValues { get; set; }
            public IList<PropertyJson> Properties { get; set; }
        }
    }
}