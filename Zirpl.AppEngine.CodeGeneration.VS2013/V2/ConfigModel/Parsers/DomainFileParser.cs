﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Zirpl.AppEngine.CodeGeneration.TextTemplating;
using Zirpl.AppEngine.CodeGeneration.V2.ConfigModel.Parsers.JsonModel;
using Zirpl.AppEngine.Model.Metadata;
using Zirpl.IO;

namespace Zirpl.AppEngine.CodeGeneration.V2.ConfigModel.Parsers
{
    public class DomainFileParser
    {
        /// <summary> 
        /// This method currently assumes the following conventions
        /// - all Domain types are defined in files with extension .domain.zae
        /// - all Domain type config files are in \_config\
        /// - all Domain type config files are in a root folder inside \_config\ that specifies which Project (ModelProject, DataServiceProject etc etc)
        /// - the name of the file (minus extension) is the Domain type (class) name
        /// - the file path (within the Project folder, minus the file name) is the subnamespace within the project
        /// </summary>
        /// <param name="app"></param>
        /// <param name="domainFilePaths"></param>
        /// <returns></returns>
        public virtual IEnumerable<DomainTypeInfo> Parse(AppInfo app, IEnumerable<String> domainFilePaths)
        {
            var list = new List<DomainTypeInfo>();
            foreach (var path in domainFilePaths)
            {
                DomainTypeJson json = null;
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var content = fileStream.ReadAllToString();
                    json = JsonConvert.DeserializeObject<DomainTypeJson>(content);
                }

                
                #region validation of input

                if (!json.IsPersistable.GetValueOrDefault(true)
                    && json.IsStaticLookup.GetValueOrDefault())
                {
                    throw new ConfigFileException("IsStaticLookup cannot be true if IsPersistable is false", path);
                }
                if (json.IsStaticLookup.GetValueOrDefault()
                    && json.IsAbstract.GetValueOrDefault())
                {
                    throw new ConfigFileException("IsStaticLookup and IsAbstract cannot both be true", path);
                }                
                if (!json.IsPersistable.GetValueOrDefault(true)
                    && (json.IsVersionable.GetValueOrDefault()
                        || json.IsAuditable.GetValueOrDefault()
                        || json.IsEndUserExtendable.GetValueOrDefault()
                        || json.IsInsertable.GetValueOrDefault()
                        || json.IsUpdatable.GetValueOrDefault()
                        || json.IsDeletable.GetValueOrDefault()
                        || json.IsMarkDeletable.GetValueOrDefault()))
                {
                    throw new ConfigFileException("IsPersistable is false but one of the following is true: IsVersionable, IsAuditable, IsExtendable, IsInsertable, IsUpdatable, IsDeletable, IsMarkDeletable", path);
                }
                if (json.IsStaticLookup.GetValueOrDefault()
                    && (json.IsVersionable.GetValueOrDefault()
                        || json.IsAuditable.GetValueOrDefault()
                        || json.IsEndUserExtendable.GetValueOrDefault()
                        || json.IsInsertable.GetValueOrDefault()
                        || json.IsUpdatable.GetValueOrDefault()
                        || json.IsDeletable.GetValueOrDefault()
                        || json.IsMarkDeletable.GetValueOrDefault()))
                {
                    throw new ConfigFileException("IsStaticLookup is false but one of the following is true: IsVersionable, IsAuditable, IsExtendable, IsInsertable, IsUpdatable, IsDeletable, IsMarkDeletable", path);
                }
                if (!json.IsStaticLookup.GetValueOrDefault()
                    && json.EnumValues.Any())
                {
                    throw new ConfigFileException("IsStaticLookup is false but EnumValues are present", path);
                }
                if (json.IsStaticLookup.GetValueOrDefault()
                    && !String.IsNullOrEmpty(json.InheritsFrom))
                {
                    throw new ConfigFileException("StaticLookup types cannot inherit from anything in", path);
                }
                if (!json.IsPersistable.GetValueOrDefault(true)
                    && json.Id != null)
                {
                    throw new ConfigFileException("Id can only be specific if IsPersistable is true", path);
                }
                if (json.Id != null
                    && json.Id.AutoGenerationBehavior.HasValue
                    && json.Id.AutoGenerationBehavior.Value == AutoGenerationBehaviorTypeEnum.None)
                {
                    throw new ConfigFileException("AutoGenerationBehavior of None is not supported", path);
                }
                if (json.Id != null
                    && json.Id.DataType.HasValue
                    && (json.Id.DataType.Value != DataTypeEnum.Guid
                        && json.Id.DataType.Value != DataTypeEnum.Int
                        && json.Id.DataType.Value != DataTypeEnum.Long
                        && json.Id.DataType.Value != DataTypeEnum.SByte
                        && json.Id.DataType.Value != DataTypeEnum.Short
                        && json.Id.DataType.Value != DataTypeEnum.UInt
                        && json.Id.DataType.Value != DataTypeEnum.ULong
                        && json.Id.DataType.Value != DataTypeEnum.UShort
                        && json.Id.DataType.Value != DataTypeEnum.Byte))
                {
                    throw new ConfigFileException("Invalid Id DataType", path);
                }
                if (json.Id != null
                    && json.Id.DataType.HasValue
                    && json.Id.DataType.Value == DataTypeEnum.String
                    && json.Id.IsNullable.GetValueOrDefault())
                {
                    throw new ConfigFileException("Id DataType of String cannot be Nullable" + path);
                }

                #endregion

                DomainTypeInfo domainType = new DomainTypeInfo();
                domainType.Config = json;
                domainType.ConfigFilePath = path;

                domainType.IsAbstract = json.IsAbstract.GetValueOrDefault();
                domainType.IsPersistable = json.IsPersistable.GetValueOrDefault(true);
                domainType.IsStaticLookup = json.IsStaticLookup.GetValueOrDefault();
                domainType.IsVersionable = json.IsVersionable.GetValueOrDefault(domainType.IsPersistable && !domainType.IsStaticLookup);
                domainType.IsAuditable = json.IsAuditable.GetValueOrDefault(domainType.IsPersistable && !domainType.IsStaticLookup);
                domainType.IsExtendable = json.IsEndUserExtendable.GetValueOrDefault();
                domainType.IsInsertable = json.IsInsertable.GetValueOrDefault(domainType.IsPersistable && !domainType.IsStaticLookup);
                domainType.IsUpdatable = json.IsUpdatable.GetValueOrDefault(domainType.IsPersistable && !domainType.IsStaticLookup);
                domainType.IsDeletable = json.IsDeletable.GetValueOrDefault(domainType.IsPersistable && !domainType.IsStaticLookup);
                domainType.IsMarkDeletable = json.IsMarkDeletable.GetValueOrDefault();

                domainType.Name = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
                if (!VisualStudioExtensions.IsValidTypeName(domainType.Name))
                {
                    throw new Exception(String.Format("Invalid resulting class name of '{0}'. Rename file to be in format 'ValidClassName.domain.zae': {1}", domainType.Name, domainType.ConfigFilePath));
                }
                var relativeDirectory = Path.GetDirectoryName(path).SubstringAfterLastInstanceOf("_config\\");
                var tempUniqueName = relativeDirectory.Replace('\\', '.') + "." + domainType.Name;
                var whichProject = tempUniqueName.SubstringUntilFirstInstanceOf("Project.", StringComparison.InvariantCultureIgnoreCase);
                var whichProjectLower = whichProject.ToLower();
                if (whichProjectLower == "model")
                {
                    domainType.DestinationProject = app.ModelProject;
                }
                else if (whichProjectLower == "dataservice")
                {
                    domainType.DestinationProject = app.DataServiceProject;
                }
                else if (whichProjectLower == "service")
                {
                    domainType.DestinationProject = app.ServiceProject;
                }
                else if (whichProjectLower == "webcore")
                {
                    domainType.DestinationProject = app.WebCoreProject;
                }
                else if (whichProjectLower == "web")
                {
                    domainType.DestinationProject = app.WebProject;
                }
                else if (whichProjectLower == "testscommon")
                {
                    domainType.DestinationProject = app.TestsCommonProject;
                }
                else if (whichProjectLower == "dataservicetests")
                {
                    domainType.DestinationProject = app.DataServiceTestsProject;
                }
                else if (whichProjectLower == "servicetests")
                {
                    domainType.DestinationProject = app.ServiceTestsProject;
                }
                else
                {
                    throw new Exception("DestinationProject unknown: " + whichProject);
                }
                var subNamespace = tempUniqueName.SubstringAfterFirstInstanceOf(whichProject + "Project.", StringComparison.InvariantCultureIgnoreCase)
                                                        .SubstringUntilLastInstanceOf("." + domainType.Name)
                                                        .SubstringUntilLastInstanceOf(domainType.Name);
                domainType.Namespace = domainType.DestinationProject.GetDefaultNamespace() +
                                        (String.IsNullOrEmpty(subNamespace) ? "" : ".") + subNamespace;
                if (!VisualStudioExtensions.IsValidNamespace(domainType.Namespace))
                {
                    throw new Exception(String.Format("Invalid resulting Namespace of '{0}' at: {1}", domainType.Namespace, domainType.ConfigFilePath));
                }
                if (!String.IsNullOrEmpty(json.PluralName))
                {
                    domainType.PluralName = json.PluralName;
                }
                else
                {
                    domainType.PluralName = this.GetPluralName(domainType.Name);
                }
                if (!VisualStudioExtensions.IsValidTypeName(domainType.PluralName))
                {
                    throw new Exception(String.Format("Invalid resulting PluralName of '{0}' at: {1}", domainType.PluralName, domainType.ConfigFilePath));
                }

                list.Add(domainType);
            }


            // handle inheritance
            //
            foreach (var domainType in list.Where(o => !String.IsNullOrEmpty(o.Config.InheritsFrom)).ToList())
            {
                var inheritsFromFullNameTokens = domainType.Config.InheritsFrom.Split('.').Reverse().ToList();
                var inheritsFromClassName = inheritsFromFullNameTokens.First();

                var potentialMatches = this.FindDomainTypes(list, domainType.Config.InheritsFrom);
                if (!potentialMatches.Any())
                {
                    throw new Exception("Could not find domain type matching InheritsFrom in: " + domainType.ConfigFilePath);
                }
                else if (potentialMatches.Count() > 1)
                {
                    throw new Exception("Found more than 1 matching domain type for InheritsFrom in: " + domainType.ConfigFilePath);
                }

                domainType.InheritsFrom = potentialMatches.Single();
                domainType.InheritsFrom.InheritedBy.Add(domainType);
            }
            // this is to ensure the interfaces that are on base classes are handled appropriately
            //
            foreach (var domainType in list.Where(o => o.InheritedBy.Any() && o.InheritsFrom == null))
            {
                // these are the base-most domain types of each heirarchy
                this.AlignInterfacePropertiesInHeirarchy(domainType);
            }


            #region validate inheritance
            
            foreach (var domainType in list.Where(o => o.InheritsFrom != null))
            {
                if (domainType.InheritsFrom.IsStaticLookup)
                {
                    throw new Exception("StaticLookups cannot be used as InheritsFrom in: " + domainType.ConfigFilePath);
                }
                if (domainType.InheritsFrom.IsEnum)
                {
                    throw new Exception("Enums cannot be used as InheritsFrom in: " + domainType.ConfigFilePath);
                }
                if (domainType.InheritsFrom.IsExtendedEntityFieldValue)
                {
                    throw new Exception("CustomValue classes cannot be used as InheritsFrom in: " + domainType.ConfigFilePath);
                }
                if (domainType.Config.Id != null)
                {
                    throw new Exception("Id, if defined, must be defined at the bottom of the Heirarchy: " + domainType.ConfigFilePath);
                }
            }

            #endregion


            //create implicit DomainTypeInfos
            //
            var newDomainTypes = new List<DomainTypeInfo>();
            foreach (var domainType in list)
            {
                // validation checks for these have already been done- we can trust the config is right 
                // if we get to this point
                //
                if (domainType.IsExtendable
                    && (domainType.InheritsFrom == null
                        || !domainType.InheritsFrom.IsExtendable))
                {
                    var extendedFieldValueDomainType = new DomainTypeInfo();
                    extendedFieldValueDomainType.DestinationProject = domainType.DestinationProject;
                    extendedFieldValueDomainType.Name = domainType.Name + "ExtendedFieldValue";
                    extendedFieldValueDomainType.PluralName = extendedFieldValueDomainType.Name + "s";
                    extendedFieldValueDomainType.Namespace = domainType.Namespace;
                    extendedFieldValueDomainType.IsPersistable = true;
                    extendedFieldValueDomainType.IsExtendedEntityFieldValue = true;
                    extendedFieldValueDomainType.Extends = domainType;
                    domainType.ExtendedBy = extendedFieldValueDomainType;

                    extendedFieldValueDomainType.IsVersionable = domainType.IsVersionable;
                    extendedFieldValueDomainType.IsAuditable = false; //domainType.IsAuditable;
                    extendedFieldValueDomainType.IsInsertable = domainType.IsInsertable;
                    extendedFieldValueDomainType.IsUpdatable = domainType.IsUpdatable;
                    extendedFieldValueDomainType.IsDeletable = domainType.IsDeletable;
                    extendedFieldValueDomainType.IsMarkDeletable = false; //domainType.IsMarkDeletable;

                    newDomainTypes.Add(extendedFieldValueDomainType);
                }
                if (domainType.Config.EnumValues.Any())
                {
                    // create DomainTypeInfo for the enum
                    //
                    var enumDomainType = new DomainTypeInfo();
                    enumDomainType.DestinationProject = domainType.DestinationProject;
                    enumDomainType.Name = domainType.Name + "Enum";
                    enumDomainType.Namespace = domainType.Namespace;
                    enumDomainType.PluralName = enumDomainType.Name + "s";
                    enumDomainType.IsEnum = true;
                    enumDomainType.EnumDescribes = domainType;
                    domainType.DescribedByEnum = enumDomainType;
                    enumDomainType.EnumDataType = (domainType.Config != null 
                                                    && domainType.Config.Id != null 
                                                    && domainType.Config.Id.DataType.HasValue) 
                                                   ? domainType.Config.Id.DataType.Value 
                                                   : DataTypeEnum.Byte;
                    foreach (var enumValue in domainType.Config.EnumValues)
                    {
                        enumDomainType.EnumValues.Add(new EnumValueInfo() {Id = Int32.Parse(enumValue.Id), Name = enumValue.Name});
                    }
                    newDomainTypes.Add(enumDomainType);
                }
            }
            list.AddRange(newDomainTypes);

            // create implicit properties
            //
            // Id properties, which are ALWAYS at the bottom of the heirarchy
            //
            foreach (var domainType in list.Where(o => o.IsPersistable && o.InheritsFrom == null))
            {
                var configToUse = domainType.IsExtendedEntityFieldValue
                    ? this.GetBaseMostDomainType(domainType.Extends).Config
                    : domainType.Config;

                var property = new DomainPropertyInfo();
                property.Name = "Id";
                property.IsPrimaryKey = true;
                
                property.AutoGenerationBehavior = (configToUse != null 
                                                && configToUse.Id != null 
                                                && configToUse.Id.AutoGenerationBehavior.HasValue)
                    ? configToUse.Id.AutoGenerationBehavior.Value
                    : AutoGenerationBehaviorTypeEnum.ByDataStore;
                property.DataType = (configToUse != null
                                    && configToUse.Id != null
                                    && configToUse.Id.DataType.HasValue)
                    ? configToUse.Id.DataType.Value
                    : domainType.IsStaticLookup
                        ? DataTypeEnum.Byte
                        : DataTypeEnum.Long;
                property.IsNullable = (configToUse != null
                                                && configToUse.Id != null
                                                && configToUse.Id.IsNullable.HasValue)
                    ? configToUse.Id.IsNullable.Value
                    : false;
                property.Owner = domainType;

                domainType.Properties.Add(property);
                domainType.IdProperty = property;
            }

            foreach (var domainType in list.Where(o => o.IsVersionable && (o.InheritsFrom == null || !o.InheritsFrom.IsVersionable)))
            {
                var property = new DomainPropertyInfo();
                property.Name = "RowVersion";
                property.DataType = DataTypeEnum.ByteArray;
                property.IsRowVersion = true;
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByDataStore;
                domainType.Properties.Add(property);
                property.Owner = domainType;
            }
            foreach (var domainType in list.Where(o => o.IsAuditable && (o.InheritsFrom == null || !o.InheritsFrom.IsAuditable)))
            {
                var property = new DomainPropertyInfo();
                property.Name = "CreatedUserId";
                property.IsForAuditableInterface = true;
                property.DataType = DataTypeEnum.String;
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByFramework;
                property.IsRequired = true;
                property.MaxLength = 256;
                property.MinLength = 1;
                property.UniquenessType = UniquenessTypeEnum.NotUnique;
                property.Owner = domainType;
                domainType.Properties.Add(property);

                property = new DomainPropertyInfo();
                property.Name = "UpdatedUserId";
                property.IsForAuditableInterface = true;
                property.DataType = DataTypeEnum.String;
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByFramework;
                property.IsRequired = true;
                property.MaxLength = 256;
                property.MinLength = 1;
                property.UniquenessType = UniquenessTypeEnum.NotUnique;
                property.Owner = domainType;
                domainType.Properties.Add(property);

                property = new DomainPropertyInfo();
                property.Name = "CreatedDate";
                property.IsForAuditableInterface = true;
                property.DataType = DataTypeEnum.DateTime;
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByFramework;
                property.IsNullable = true;
                property.IsRequired = true;
                property.UniquenessType = UniquenessTypeEnum.NotUnique;
                property.Owner = domainType;
                domainType.Properties.Add(property);

                property = new DomainPropertyInfo();
                property.Name = "UpdatedDate";
                property.IsForAuditableInterface = true;
                property.DataType = DataTypeEnum.DateTime;
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByFramework;
                property.IsNullable = true;
                property.IsRequired = true;
                property.UniquenessType = UniquenessTypeEnum.NotUnique;
                property.Owner = domainType;
                domainType.Properties.Add(property);
            }

            foreach (var domainType in list.Where(o => o.IsMarkDeletable && (o.InheritsFrom == null || !o.InheritsFrom.IsMarkDeletable)))
            {
                var property = new DomainPropertyInfo();
                property.Name = "IsMarkedDeleted";
                property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.ByFramework;
                property.DataType = DataTypeEnum.Boolean;
                property.IsForMarkDeletedInterface = true;
                property.IsNullable = false;
                property.IsRequired = true;
                property.UniquenessType = UniquenessTypeEnum.NotUnique;
                property.Owner = domainType;
                domainType.Properties.Add(property);
            }
            foreach (var domainType in list.Where(o => o.IsExtendable && (o.InheritsFrom == null || !o.InheritsFrom.IsExtendable)))
            {
                var fromProperty = new DomainPropertyInfo();
                fromProperty.Name = "ExtendedFieldValues";
                fromProperty.DataType = DataTypeEnum.Relationship;
                fromProperty.IsForExtendableInterface = true;
                fromProperty.Owner = domainType;
                domainType.Properties.Add(fromProperty);

                var toProperty = new DomainPropertyInfo()
                {
                    Name = "ExtendedEntity",
                    DataType = DataTypeEnum.Relationship,
                    IsForExtendedEntityFieldValueInterface = true,
                    IsRequired = true,
                    Owner = domainType.ExtendedBy
                };
                domainType.ExtendedBy.Properties.Add(toProperty);

                var foreignKeyOnTo = new DomainPropertyInfo()
                {
                    Name = "ExtendedEntityId",
                    DataType = domainType.IdProperty.DataType,
                    IsForeignKey = true,
                    IsForExtendedEntityFieldValueInterface = true,
                    IsRequired = true,
                    Owner = domainType.ExtendedBy
                };
                domainType.ExtendedBy.Properties.Add(foreignKeyOnTo);

                var relationship = new RelationshipInfo()
                {
                    Type = RelationshipTypeEnum.OneToMany,
                    DeletionBehavior = RelationshipDeletionBehaviorTypeEnum.CascadeDelete,
                    From = domainType,
                    To = domainType.ExtendedBy,
                    NavigationPropertyOnFrom = fromProperty,
                    NavigationPropertyOnTo = toProperty,
                    ForeignKeyOnTo = foreignKeyOnTo
                };

                fromProperty.Relationship = relationship;
                toProperty.Relationship = relationship;
                foreignKeyOnTo.Relationship = relationship;
                domainType.Relationships.Add(relationship);
                domainType.ExtendedBy.Relationships.Add(relationship);

                // also the Value field
                var valueProperty = new DomainPropertyInfo()
                {
                    Name = "Value",
                    DataType = DataTypeEnum.String,
                    IsForExtendedEntityFieldValueInterface = true,
                    IsMaxLength = true,
                    Owner = domainType.ExtendedBy
                };
                domainType.ExtendedBy.Properties.Add(valueProperty);
            }
            foreach (var domainType in list.Where(o => o.IsStaticLookup))
            {
                var nameProperty = new DomainPropertyInfo()
                {
                    Name = "Name",
                    IsForIsStaticLookupInterface = true,
                    DataType = DataTypeEnum.String,
                    IsRequired = true,
                    MinLength = 1,
                    MaxLength = 1024,
                    Owner = domainType
                };
                domainType.Properties.Add(nameProperty);
            }

            foreach (var domainType in list.Where(o => o.Config != null && o.Config.Properties.Any()))
            {
                foreach (var json in domainType.Config.Properties)
                {
                    #region Property validation
                    if (String.IsNullOrWhiteSpace(json.Name))
                    {
                        throw new ConfigFileException("Invalid PropertyName: " + json.Name, domainType.ConfigFilePath);
                    }
                    Action verifyMinMaxLengthPropertiesNotUsed = delegate() {
                        if (!String.IsNullOrEmpty(json.MinLength)
                            || !String.IsNullOrEmpty(json.MaxLength)
                            || json.IsMaxLength.GetValueOrDefault())
                        {
                            throw new ConfigFileException("Cannot use MinLength, MaxLength or IsMaxLength with integer DataTypes: " + json.Name, domainType.ConfigFilePath);
                        }
                    }; 
                    Action verifyRelationshipPropertiesNotUsed = delegate()
                    {
                        if (!String.IsNullOrEmpty(json.RelationshipTo)
                            || !String.IsNullOrEmpty(json.RelationshipOtherSidePropertyName)
                            || json.RelationshipType.GetValueOrDefault() != RelationshipTypeEnum.None
                            || json.RelationshipDeletionBehavior.GetValueOrDefault() != RelationshipDeletionBehaviorTypeEnum.None
                            || json.RelationshipOtherSidePropertyUniquenessType.GetValueOrDefault() != UniquenessTypeEnum.None)
                        {
                            throw new ConfigFileException("Cannot use RelationshipTo, NavigationPropertyName, RelationshipDeletionBehavior or RelationshipType with non-relationship DataTypes: " + json.Name, domainType.ConfigFilePath);
                        }
                    };
                    Action verifyNullablePropertyNotUsed = delegate()
                    {
                        if (json.IsNullable.GetValueOrDefault())
                        {
                            throw new ConfigFileException("Cannot use Nullable for specified DataType: " + json.Name, domainType.ConfigFilePath);
                        }
                    };
                    Action verifyMinMaxValuePropertiesNotUsed = delegate()
                    {
                        if (!String.IsNullOrEmpty(json.MinValue)
                            || !String.IsNullOrEmpty(json.MaxValue))
                        {
                            throw new ConfigFileException("Cannot use Nullable for specified DataType: " + json.Name, domainType.ConfigFilePath);
                        }
                    };
                    Action verifyPrecisionPropertiesNotUsed = delegate()
                    {
                        if (!String.IsNullOrEmpty(json.Precision))
                        {
                            throw new ConfigFileException("Cannot use Precision for specified DataType: " + json.Name, domainType.ConfigFilePath);
                        }
                    };
                    DataTypeEnum dataType = json.DataType.GetValueOrDefault(DataTypeEnum.String);
                    switch (dataType)
	                {
                        case DataTypeEnum.None:
                        case DataTypeEnum.Object:
                        case DataTypeEnum.SByte:
                        case DataTypeEnum.UShort:
                        case DataTypeEnum.UInt:
                        case DataTypeEnum.ULong:
                            throw new ConfigFileException("Unsupported or invalid Property DataType: " + json.Name, domainType.ConfigFilePath);
                        case DataTypeEnum.Byte:
                        case DataTypeEnum.Short:
                        case DataTypeEnum.Int:
                        case DataTypeEnum.Long:
	                        verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();

                            // TODO: validate Min/Max Value
                            break;
                        case DataTypeEnum.Char:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();
                            break;
                        case DataTypeEnum.Boolean:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();
                            break;
                        case DataTypeEnum.Float:
                        case DataTypeEnum.Double:
                        case DataTypeEnum.Decimal:
                        case DataTypeEnum.Currency:
                        case DataTypeEnum.Percentage:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();

                            // TODO: validate Min/Max Value, Precision
                            break;
                        case DataTypeEnum.String:
                        case DataTypeEnum.EmailAddress:
                        case DataTypeEnum.Url:
                            verifyRelationshipPropertiesNotUsed();
	                        verifyNullablePropertyNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();

                            // TODO: validate Min/Max Length, IsMaxLength
                            break;
                        case DataTypeEnum.Date:
                        case DataTypeEnum.Time:
                        case DataTypeEnum.DateTime:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();

                            // TODO: validate Min/Max Value
                            break;
                        case DataTypeEnum.Relationship:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyNullablePropertyNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();

                            // TODO: more validation here for actual relationship properties
                            break;
                        case DataTypeEnum.Guid:
                            verifyMinMaxLengthPropertiesNotUsed();
	                        verifyRelationshipPropertiesNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();
                            break;
                        case DataTypeEnum.Image:
                        case DataTypeEnum.ByteArray:
                            verifyRelationshipPropertiesNotUsed();
	                        verifyNullablePropertyNotUsed();
	                        verifyMinMaxValuePropertiesNotUsed();
	                        verifyPrecisionPropertiesNotUsed();

                            // TODO: validate Min/Max Length, IsMaxLength
                            break;
                        default:
                            throw new ConfigFileException("Invalid Property DataType: " + json.Name, domainType.ConfigFilePath);
                    }
                    #endregion
                    
                    if (json.DataType != DataTypeEnum.Relationship)
                    {
                        // Value type
                        var property = new DomainPropertyInfo();
                        property.Name = json.Name;
                        property.DataType = json.DataType.GetValueOrDefault(DataTypeEnum.String);
                        property.AutoGenerationBehavior = AutoGenerationBehaviorTypeEnum.None;
                        property.IsNullable = json.IsNullable.GetValueOrDefault();
                        property.IsRequired = json.IsRequired.GetValueOrDefault();
                        property.IsMaxLength = json.IsMaxLength.GetValueOrDefault();
                        property.MinLength = String.IsNullOrEmpty(json.MinLength) ? 0 : Int64.Parse(json.MinLength);
                        property.MaxLength = String.IsNullOrEmpty(json.MaxLength) ? 0 : Int64.Parse(json.MaxLength);
                        property.MinValue = json.MinValue;
                        property.MaxValue = json.MaxValue;
                        property.Precision = json.Precision;
                        property.UniquenessType = json.UniquenessType.GetValueOrDefault(UniquenessTypeEnum.NotUnique);
                        property.Owner = domainType;
                        domainType.Properties.Add(property);
                    }
                    else
                    {
                        var potentialMatches = this.FindDomainTypes(list, json.RelationshipTo);
                        if (!potentialMatches.Any())
                        {
                            throw new Exception("Could not find domain type matching RelationshipTo in: " + domainType.ConfigFilePath);
                        }
                        else if (potentialMatches.Count() > 1)
                        {
                            throw new Exception("Found more than 1 matching domain type for RelationshipTo in: " + domainType.ConfigFilePath);
                        }
                        var toEntity = potentialMatches.Single();
                        var fromEntity = domainType;
                        var fromProperty = new DomainPropertyInfo();
                        fromProperty.Name = json.Name;
                        fromProperty.DataType = DataTypeEnum.Relationship;
                        DomainPropertyInfo toProperty = null;
                        DomainPropertyInfo foreignKeyOnTo = null;
                        DomainPropertyInfo foreignKeyOnFrom = null;
                        var relationship = new RelationshipInfo();
                        
                        // TODO: support the rest of the relationship types

                        switch (json.RelationshipType.GetValueOrDefault(RelationshipTypeEnum.None))
                        {
                            case RelationshipTypeEnum.None:
                                throw new ConfigFileException("Invalid RelationShipType: " + json.Name, domainType.ConfigFilePath);
                            case RelationshipTypeEnum.OneToMany:

                                // collection property on the from

                                if (!String.IsNullOrEmpty(json.RelationshipOtherSidePropertyName))
                                {
                                    toProperty = new DomainPropertyInfo();
                                    toProperty.Name = json.RelationshipOtherSidePropertyName;
                                    toProperty.DataType = DataTypeEnum.Relationship;
                                    toProperty.IsRequired = json.IsRelationshipOtherSidePropertyRequired.GetValueOrDefault();
                                    toProperty.UniquenessType =
                                        json.RelationshipOtherSidePropertyUniquenessType.GetValueOrDefault(
                                            UniquenessTypeEnum.NotUnique);

                                    foreignKeyOnTo = new DomainPropertyInfo();
                                    foreignKeyOnTo.Name = json.RelationshipOtherSidePropertyName + "Id";
                                    foreignKeyOnTo.DataType = fromEntity.IdProperty.DataType;
                                    foreignKeyOnTo.IsForeignKey = true;
                                    foreignKeyOnTo.IsRequired = json.IsRelationshipOtherSidePropertyRequired.GetValueOrDefault();
                                    foreignKeyOnTo.IsNullable = foreignKeyOnTo.IsRequired;
                                    foreignKeyOnTo.UniquenessType = toProperty.UniquenessType;
                                }

                                relationship.Type = RelationshipTypeEnum.OneToMany;
                                relationship.DeletionBehavior = json.RelationshipDeletionBehavior.GetValueOrDefault(RelationshipDeletionBehaviorTypeEnum.CascadeDelete);
       
                                
                                break;
                            case RelationshipTypeEnum.ManyToOne:

                                // collection property on the to
                                
                                fromProperty.IsRequired = json.IsRequired.GetValueOrDefault();
                                fromProperty.UniquenessType = json.UniquenessType.GetValueOrDefault(UniquenessTypeEnum.NotUnique);

                                foreignKeyOnFrom = new DomainPropertyInfo();
                                foreignKeyOnFrom.Name = json.RelationshipOtherSidePropertyName + "Id";
                                foreignKeyOnFrom.DataType = fromEntity.IdProperty.DataType;
                                foreignKeyOnFrom.IsForeignKey = true;
                                foreignKeyOnFrom.IsRequired = json.IsRequired.GetValueOrDefault();
                                foreignKeyOnFrom.IsNullable = foreignKeyOnFrom.IsRequired;
                                foreignKeyOnFrom.UniquenessType = fromProperty.UniquenessType;

                                if (!String.IsNullOrEmpty(json.RelationshipOtherSidePropertyName))
                                {
                                    toProperty = new DomainPropertyInfo();
                                    toProperty.Name = json.RelationshipOtherSidePropertyName;
                                    toProperty.DataType = DataTypeEnum.Relationship;
                                }

                                break;
                        }
                        relationship.From = fromEntity;
                        relationship.To = toEntity;
                        relationship.Type = json.RelationshipType.GetValueOrDefault();
                        relationship.DeletionBehavior = json.RelationshipDeletionBehavior.GetValueOrDefault(
                                RelationshipDeletionBehaviorTypeEnum.Restricted);
                        relationship.NavigationPropertyOnFrom = fromProperty;
                        relationship.NavigationPropertyOnTo = toProperty;
                        relationship.ForeignKeyOnTo = foreignKeyOnTo;
                        relationship.ForeignKeyOnFrom = foreignKeyOnFrom;

                        fromProperty.Relationship = relationship;
                        fromProperty.Owner = fromEntity;
                        fromEntity.Properties.Add(fromProperty);
                        if (foreignKeyOnFrom != null)
                        {
                            foreignKeyOnFrom.Relationship = relationship;
                            foreignKeyOnFrom.Owner = fromEntity;
                            fromEntity.Properties.Add(foreignKeyOnFrom);
                        }
                        if (toProperty != null)
                        {
                            toProperty.Relationship = relationship;
                            toProperty.Owner = toEntity;
                            toEntity.Properties.Add(toProperty);
                        }
                        if (foreignKeyOnTo != null)
                        {
                            foreignKeyOnTo.Relationship = relationship;
                            foreignKeyOnTo.Owner = toEntity;
                            toEntity.Properties.Add(foreignKeyOnTo);
                        }
                        fromEntity.Relationships.Add(relationship);
                        toEntity.Relationships.Add(relationship);
                    }
                }
            }





            // validation of names
            //
            if (list.GroupBy(p => p.FullName).Where(g => g.Count() > 1).Any())
            {
                throw new Exception("2 DomainTypes with the same name resulted");
            }
            foreach (var domainType in list)
            {
                // TODO: this only checks per class, not per heirarchy
                if (domainType.Properties.GroupBy(p => p.Name).Where(g => g.Count() > 1).Any())
                {
                    throw new Exception("2 Properties with the same name resulted in: " + domainType.ConfigFilePath);
                }
            }
            

            //this.LogLineToBuildPane(JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
            
            return list;
        }

        private void AlignInterfacePropertiesInHeirarchy(DomainTypeInfo domainType)
        {
                foreach (var child in domainType.InheritedBy)
                {
                    // if inheriting an interface, then this class HAS to have it
                    //
                    child.IsVersionable = domainType.IsVersionable
                        ? true
                        : child.IsVersionable;
                    child.IsAuditable = domainType.IsAuditable
                        ? true
                        : child.IsAuditable;
                    child.IsExtendable = domainType.IsExtendable
                        ? true
                        : child.IsExtendable;
                    child.IsMarkDeletable = domainType.IsMarkDeletable
                        ? true
                        : child.IsMarkDeletable;

                    this.AlignInterfacePropertiesInHeirarchy(child);
                    // TODO: if we start using IsDeletable, IsUpdatable, IsInsertable as interfaces on the domain type, then we need to apply those here
                }
        }

        protected IEnumerable<DomainTypeInfo> FindDomainTypes(IEnumerable<DomainTypeInfo> list, String partialFullName)
        {
            var partialFullNameTokens = partialFullName.Split('.').Reverse().ToList();
            var className = partialFullNameTokens.First();

            var potentialMatches = (from dt in list
                                    where dt.Name.ToLowerInvariant() == className.ToLowerInvariant()
                                    select dt).ToList();

            // even though there may only be one, we still take this step because
            // we want to ensure the entire namespace matches
            //
            for (int i = potentialMatches.Count() - 1; i >= 0; i--)
            {
                var potentialMatch = potentialMatches[i];
                var potentialMatchFullNameTokens = potentialMatch.FullName.Split('.').Reverse().ToList();
                if (partialFullNameTokens.Count() > potentialMatchFullNameTokens.Count())
                {
                    // by definition this can't be a match since the
                    // number of namespace tokens is greater than the match
                    //
                    potentialMatches.Remove(potentialMatch);
                }
                else
                {
                    for (int j = 0; j < partialFullNameTokens.Count(); j++)
                    {
                        if (potentialMatchFullNameTokens[j] != partialFullNameTokens[j])
                        {
                            potentialMatches.Remove(potentialMatch);
                            break;
                        }
                    }
                }
            }
            return potentialMatches;
        }

        protected DomainTypeInfo GetBaseMostDomainType(DomainTypeInfo domainType)
        {
            if (domainType.InheritsFrom != null)
            {
                domainType = domainType.InheritsFrom;
            }
            return domainType;
        }

        protected virtual String GetPluralName(String className)
        {
            if (className.EndsWith("s"))
            {
                return className + "es";
            }
            else if (className.EndsWith("y"))
            {
                return className.Substring(0, className.Length - 1) + "ies";
            }
            else
            {
                return className + "s";
            }
        }
    }
}
