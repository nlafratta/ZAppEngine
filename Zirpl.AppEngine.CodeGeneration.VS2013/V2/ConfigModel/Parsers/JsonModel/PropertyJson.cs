﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zirpl.AppEngine.Model.Metadata;

namespace Zirpl.AppEngine.CodeGeneration.V2.ConfigModel.Parsers.JsonModel
{
    public class PropertyJson
    {
        public String Name { get; set; }
        public DataTypeEnum? DataType { get; set; }
        public bool? IsNullable { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsMaxLength { get; set; }
        public String MinLength { get; set; }
        public String MaxLength { get; set; }
        public String MinValue { get; set; }
        public String MaxValue { get; set; }
        public String Precision { get; set; }
        public UniquenessTypeEnum? Uniqueness { get; set; }

        public RelationshipJson Relationship { get; set; }
    }
}