﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.TextTemplating;
using Zirpl.AppEngine.Logging;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.TextTemplating;
using Zirpl.AppEngine.VisualStudioAutomation.VisualStudio.Logging;
using Zirpl.Reflection;

namespace Zirpl.AppEngine.VisualStudioAutomation.TextTemplating
{
    public static class TextTransformationExtensions
    {
        public static void SetUp(this TextTransformation textTransformation)
        {
            LogFactory.Initialize((IServiceProvider)textTransformation.Wrap().Host);
        }

        public static void CleanUp(this TextTransformation textTransformation)
        {
            textTransformation.SetUp();
            textTransformation.Wrap().FileManager.EndFile();
        }

        public static ITextTransformation Wrap(this TextTransformation textTransformation)
        {
            textTransformation.SetUp();
            return new TextTransformationWrapper(textTransformation);
        }

        public static void RunTemplates(this TextTransformation textTransformation, ITemplateRunner templateRunner, ITemplateProvider templateProvider, IOutputFileProvider outputFileProvider)
        {
            try
            {
                //textTransformation.SetUp();
                var fileManager = textTransformation.Wrap().FileManager;
                templateRunner.RunTemplates(fileManager, templateProvider, outputFileProvider);
            }
            catch (Exception e)
            {
                try
                {
                    LogManager.GetLog().Debug(e.ToString());
                    textTransformation.WriteLine(e.ToString());
                }
                catch (Exception)
                {
                }
                throw;
            }
            finally
            {
                textTransformation.CleanUp();
            }
        }

        //private static void AssertContext(Object textTransformation)
        //{
        //    if (TextTransformationContext.Instance == null)
        //    {
        //        if (textTransformation is TextTransformation)
        //        {
        //            ((TextTransformation)textTransformation).CreateContext();
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("No Context exists. Use 'using (var context = this.CreateContext())' within a normal template.");
        //        }
        //    }
        //}

        //public static void UseNewCSharpFile(this Object textTransformation, String fileName, Project destinationProject = null)
        //{
        //    textTransformation.UseNewCSharpFile(fileName, null, destinationProject);
        //}

        //public static void UseNewCSharpFile(this Object textTransformation, String fileName, String folderWithinProject = null, Project destinationProject = null)
        //{
        //    AssertContext(textTransformation);
        //    if (!Path.HasExtension(fileName))
        //    {
        //        fileName += ".cs";
        //    }
        //    textTransformation.UseNewFile(fileName, folderWithinProject, destinationProject, BuildActionTypeEnum.Compile);
        //}

        //public static void UseNewCSharpFile(this Object textTransformation, String fileName, String folderWithinProject = null, String destinationProjectName = null)
        //{
        //    AssertContext(textTransformation);
        //    if (!Path.HasExtension(fileName))
        //    {
        //        fileName += ".cs";
        //    }
        //    textTransformation.UseNewFile(fileName, folderWithinProject, destinationProjectName, BuildActionTypeEnum.Compile);
        //}

        //public static void UseNewFile(this Object textTransformation, String fileName, String folderWithinProject = null, String destinationProjectName = null, BuildActionTypeEnum? buildAction = null, String customTool = null, bool? autoFormat = null, bool? overwrite = null, Encoding encoding = null)
        //{
        //    AssertContext(textTransformation);
        //    var project = String.IsNullOrEmpty(destinationProjectName)
        //        ? null
        //        : TextTransformationContext.Instance.VisualStudio.GetProject(destinationProjectName);

        //    textTransformation.UseNewFile(fileName, folderWithinProject, project, buildAction, customTool, autoFormat, overwrite, encoding);
        //}

        //public static void UseNewFile(this Object textTransformation, String fileName, String folderWithinProject = null, Project destinationProject = null, BuildActionTypeEnum? buildAction = null, String customTool = null, bool? autoFormat = null, bool? overwrite = null, Encoding encoding = null)
        //{
        //    AssertContext(textTransformation);
        //    var outputFile = new OutputFile()
        //    {
        //        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName),
        //        FileExtension = Path.GetExtension(fileName),
        //        DestinationProject = destinationProject ?? TextTransformationContext.Instance.CallingTemplateProjectItem.ContainingProject,
        //        FolderPathWithinProject = folderWithinProject,
        //        CustomTool = customTool
        //    };
        //    outputFile.BuildAction = buildAction ?? outputFile.BuildAction;
        //    outputFile.CanOverrideExistingFile = overwrite ?? outputFile.CanOverrideExistingFile;
        //    outputFile.AutoFormat = autoFormat ?? outputFile.AutoFormat;
        //    outputFile.Encoding = encoding ?? outputFile.Encoding;

        //    textTransformation.UseNewFile(outputFile);
        //}

        //public static void UseNewFile(this Object textTransformation, OutputFile outputFile)
        //{
        //    AssertContext(textTransformation);
        //    TextTransformationContext.Instance.StartFile(new TextTransformationWrapper(textTransformation), outputFile);
        //}

        //public static void UseDefaultFile(this Object textTransformation)
        //{
        //    AssertContext(textTransformation);
        //    TextTransformationContext.Instance.EndFile();
        //}
    }
}
