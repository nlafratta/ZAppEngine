﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TextTemplating;
using Zirpl.IO;

namespace Zirpl.AppEngine.VisualStudioAutomation.TextTemplating
{
    public class OutputFileManager : IDisposable
    {
        private IList<OutputFile> CompletedFiles { get; set; }
        private TextTransformationContext Context { get; set; }
        private StringBuilder CallingTemplateOriginalGenerationEnvironment { get; set; }
        private StringBuilder CurrentGenerationEnvironment { get; set; }
        private OutputFile CurrentOutputFile { get; set; }

        public OutputFileManager(TextTransformationContext context)
        {
            this.CallingTemplateOriginalGenerationEnvironment = context.CallingTemplate.GenerationEnvironment;
            this.CurrentGenerationEnvironment = context.CallingTemplate.GenerationEnvironment;
            this.Context = context;
            this.CompletedFiles = new List<OutputFile>();
        }

        public void WriteFile(OutputFile outputFile)
        {
            this.StartFile(outputFile);

            var preprocessFile = outputFile as PreprocessedTextTransformationOutputFile;
            if (preprocessFile != null)
            {

                var template = Activator.CreateInstance(preprocessFile.TemplateType);
                var templateWrapper = new PreprocessedTextTransformationWrapper(template);
                var session = new TextTemplatingSession();
                foreach (var parameter in preprocessFile.TemplateParameters)
                {
                    session[parameter.Key] = parameter.Value;
                }
                session["TemplateOutputFile"] = preprocessFile;
                templateWrapper.Session = session;
                templateWrapper.Initialize(); // Must call this to transfer values.

                this.Context.LogLineToBuildPane("Transforming text for file: " + this.CurrentOutputFile.FullFilePath);
                this.CurrentGenerationEnvironment.Append(templateWrapper.TransformText());
            }

            this.EndFile();
        }

        public void StartFile(OutputFile file)
        {
            this.EndFile();

            this.CurrentOutputFile = file;
            this.CurrentGenerationEnvironment = new StringBuilder();
            this.Context.CallingTemplate.GenerationEnvironment = this.CurrentGenerationEnvironment;
        }

        public void EndFile()
        {
            if (this.CurrentOutputFile != null)
            {
                this.CurrentOutputFile.Content = this.CurrentGenerationEnvironment.ToString();
                this.CurrentGenerationEnvironment = this.CallingTemplateOriginalGenerationEnvironment;
                this.Context.CallingTemplate.GenerationEnvironment = this.CurrentGenerationEnvironment;
                
                // apply parameters to content
                //
                // 1) ensure directory exists       [DONE]
                //      first within file system    [DONE]
                //      then within Solution        [DONE]
                //
                // 2) check out from source control
                //      check if file exists                        [DONE]
                //      if exists, check if different               [DONE]
                //      if different, check if allowed to overwrite [DONE]
                //      if allowed, check out from source control   [DONE]
                //
                // 3) create the file
                //      write the file to disk                  [DONE]
                //      add file to folder project item         [DONE]
                //      write VS properties (if each exists)    [DONE]
                //          CustomTool                          [DONE]
                //          ItemType                            [DONE]
                //      check if autoformat                     [DONE]
                //          format                              [DONE]
                //
                // clean up template placeholders

                // TODO: use template placeholders if should

                PathUtilities.EnsureDirectoryExists(this.CurrentOutputFile.FullFilePath);
                var folder = this.CurrentOutputFile.DestinationProject.GetOrCreateProjectFolder(this.CurrentOutputFile.FolderPathWithinProject);

                if (File.Exists(this.CurrentOutputFile.FullFilePath))
                {
                    var isDifferent =
                        File.ReadAllText(this.CurrentOutputFile.FullFilePath, this.CurrentOutputFile.Encoding) !=
                        this.CurrentOutputFile.Content;
                    if (isDifferent
                        && this.CurrentOutputFile.CanOverrideExistingFile)
                    {
                        if (this.Context.VisualStudio.SourceControl != null
                            && this.Context.VisualStudio.SourceControl.IsItemUnderSCC(this.CurrentOutputFile.FullFilePath)
                            && !this.Context.VisualStudio.SourceControl.IsItemCheckedOut(this.CurrentOutputFile.FullFilePath))
                        {
                            this.Context.VisualStudio.SourceControl.CheckOutItem(this.CurrentOutputFile.FullFilePath);
                        }
                    }
                    else if (isDifferent)
                    {
                        throw new Exception("Could not overwrite file: " + this.CurrentOutputFile.FullFilePath);
                    }
                }

                this.Context.LogLineToBuildPane("Writing file: " + this.CurrentOutputFile.FullFilePath);
                var item = this.Context.VisualStudio.GetProjectItem(this.CurrentOutputFile.FullFilePath);
                if (item != null)
                {
                    item.Remove();
                    //this.CurrentOutputFile.ProjectItem = item;
                    //item.Open();
                    //var td = (TextDocument) item.Document.Object();
                    //td.
                }
                //else
                {
                    File.WriteAllText(this.CurrentOutputFile.FullFilePath, this.CurrentOutputFile.Content);
                    this.CurrentOutputFile.ProjectItem =
                        folder.ProjectItems.AddFromFile(this.CurrentOutputFile.FullFilePath);
                }

                // set VS properties for the ProjectItem
                //
                if (!String.IsNullOrWhiteSpace(this.CurrentOutputFile.CustomTool))
                {
                    this.CurrentOutputFile.ProjectItem.SetPropertyValue("CustomTool", this.CurrentOutputFile.CustomTool);
                }
                if (!String.IsNullOrWhiteSpace(this.CurrentOutputFile.BuildActionString))
                {
                    this.CurrentOutputFile.ProjectItem.SetPropertyValue("ItemType", this.CurrentOutputFile.BuildActionString);
                }

                // autoformat 
                //
                if (this.CurrentOutputFile.AutoFormat)
                {
                    this.Context.VisualStudio.ExecuteVsCommand(this.CurrentOutputFile.ProjectItem, "Edit.FormatDocument"); //, "Edit.RemoveAndSort"));
                }

                this.CurrentOutputFile = null;
            }
        }

        public void Dispose()
        {
            this.EndFile();
        }
    }
}
