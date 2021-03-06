﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 12.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Zirpl.AppEngine.CodeGeneration.V1.Templates.Validation.EntityFramework.FluentValidation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using Zirpl.AppEngine.Model;
    using Zirpl.AppEngine.Model.Metadata;
    using Zirpl.AppEngine.CodeGeneration;
    using Zirpl.AppEngine.CodeGeneration.TextTemplating;
    using Zirpl.AppEngine.CodeGeneration.V1;
    using Zirpl.AppEngine.CodeGeneration.V1.ConfigModel;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public partial class ValidatorTemplate : ValidatorTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 20 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"


	// Generate Validation classes
	//
	foreach (DomainType domainType in this.Helper.DomainTypesToGenerateValidatorFor)
	{
		this.Helper.StartValidatorFile(domainType);
	

            
            #line default
            #line hidden
            this.Write("\r\nusing System;\r\nusing FluentValidation;\r\nusing Zirpl.AppEngine.Validation;\r\nusin" +
                    "g Zirpl.AppEngine.Validation.EntityFramework;\r\nusing ");
            
            #line 34 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetModelNamespace(domainType)));
            
            #line default
            #line hidden
            this.Write(";\r\nusing ");
            
            #line 35 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsNamespace(domainType)));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nnamespace ");
            
            #line 37 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetValidatorNamespace(domainType)));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public");
            
            #line 39 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(domainType.IsAbstract ? " abstract" : ""));
            
            #line default
            #line hidden
            this.Write(" partial class ");
            
            #line 39 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetValidatorTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 39 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetValidatorBaseDeclaration(domainType)));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t");
            
            #line 40 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetValidatorGenericConstraintDeclaration(domainType)));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n        ");
            
            #line 42 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(domainType.IsAbstract ? "protected" : "public"));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 42 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetValidatorContructorMemberName(domainType)));
            
            #line default
            #line hidden
            this.Write("()\r\n        {\r\n");
            
            #line 44 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

		if (domainType.Properties != null)
        {
			foreach (var property in domainType.Properties)
			{
				if (property.IsCollection)
				{

            
            #line default
            #line hidden
            this.Write("\t\t\t// unsure how to follow this for validation or even if it should with EF- Coll" +
                    "ection property: ");
            
            #line 52 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 53 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

				}
				else
				{
					string propertyType = property.Type;
					propertyType = propertyType.ToLowerInvariant() == "currency" ? "decimal" : propertyType;
					if (propertyType == "string")
					{

            
            #line default
            #line hidden
            this.Write("\t\t\tthis.RuleFor(o => o.");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(")");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.IsRequired ? ".Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()" : ""));
            
            #line default
            #line hidden
            this.Write(".Length(");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_MinLength, ");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 62 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_MaxLength);\r\n");
            
            #line 63 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"


					}
					else if (propertyType.ToLowerInvariant() == "int"
							||propertyType.ToLowerInvariant() == "decimal"
							||propertyType.ToLowerInvariant() == "double"
							||propertyType.ToLowerInvariant() == "byte")
					{
						//var notNullOrNotEmptyTest = property.IsRequired 
						//	? "" 
						//	: (property.MinValue == 0 
						//		|| property.MaxValue == 0
						//		|| (property.MinValue < 0 && property.MaxValue > 0)) 
						//		? ".NotNull()"
						//		: ".NotEmpty()";

            
            #line default
            #line hidden
            this.Write("\t\t\tthis.RuleFor(o => o.");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(")");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.IsRequired ? ".Cascade(CascadeMode.StopOnFirstFailure).NotNull()" : ""));
            
            #line default
            #line hidden
            this.Write(".InclusiveBetween(");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_MinValue, ");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 79 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_MaxValue);\r\n");
            
            #line 80 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

					}
					else if (propertyType.ToLowerInvariant() == "datetime"
							||propertyType.ToLowerInvariant() == "guid")
				    {
					// TODO: what if not required AND nullable BUT has value
						if (property.IsRequired)
				        {

            
            #line default
            #line hidden
            this.Write("\t\t\tthis.RuleFor(o => o.");
            
            #line 89 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(").NotEmpty();\r\n");
            
            #line 90 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

						}
						else
						{

            
            #line default
            #line hidden
            this.Write("\t\t\tthis.When(o =>  o.");
            
            #line 95 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(".HasValue, () => {\r\n\t\t\t\tthis.RuleFor(o => o.");
            
            #line 96 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(").NotEmpty();\r\n\t\t\t});\r\n");
            
            #line 98 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

			          }
			       }
					else if (propertyType.ToLowerInvariant() == "bool")
			       {

            
            #line default
            #line hidden
            this.Write("\t\t\tthis.RuleFor(o => o.");
            
            #line 104 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(")");
            
            #line 104 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.IsRequired ? ".NotNull()" : ""));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 105 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

			     }
					else if (property.IsRelationship)
			      {
						if (property.IsRequired)
				      {

            
            #line default
            #line hidden
            this.Write("            this.ForeignEntityNotNullAndIdMatches(o => o.");
            
            #line 112 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(", o => o.");
            
            #line 112 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("Id,\r\n                ");
            
            #line 113 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 113 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_Name, ");
            
            #line 113 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 113 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("Id_Name);\r\n");
            
            #line 114 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

				      }
						else
				      {

            
            #line default
            #line hidden
            this.Write("            this.ForeignEntityAndIdMatchIfNotNull(o => o.");
            
            #line 119 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(", o => o.");
            
            #line 119 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("Id,\r\n                ");
            
            #line 120 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 120 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("_Name, ");
            
            #line 120 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Helper.GetMetadataConstantsTypeName(domainType)));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 120 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("Id_Name);\r\n");
            
            #line 121 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

					    }
				    }
				}
			}
        }

            
            #line default
            #line hidden
            this.Write("\r\n\t\t\tthis.OnCustomValidation();\r\n        }\r\n\r\n\t\tpartial void OnCustomValidation()" +
                    ";\r\n    }\r\n}\r\n\r\n");
            
            #line 136 "E:\projects\ZAppEngine\Zirpl.AppEngine.CodeGeneration.VS2013\V1\Templates\Validation\EntityFramework\FluentValidation\ValidatorTemplate.tt"

	}

            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        private global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost hostValue;
        /// <summary>
        /// The current host for the text templating engine
        /// </summary>
        public virtual global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost Host
        {
            get
            {
                return this.hostValue;
            }
            set
            {
                this.hostValue = value;
            }
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public class ValidatorTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
