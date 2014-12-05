﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.Config;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.Config.Parsers;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.TextTemplating;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.TextTemplating.V1;
using Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.TextTemplating.V1.Templates.Model;
using Zirpl.AppEngine.VisualStudioAutomation.TextTemplating;
using Zirpl.Collections;

namespace Zirpl.AppEngine.VisualStudioAutomation.AppGeneration
{
    public static class AppGenerator
    {
        public static void GenerateV1App(this TextTransformation callingTemplate, AppGenerationSettings settings = null, IEnumerable<ITemplateOutputFileBuilderStrategy> strategies = null)
        {
            // set all of the settings defaults
            //
            settings = settings ?? new AppGenerationSettings();
            settings.DataContextName = settings.DataContextName ?? "AppDataContext";
            settings.GeneratedContentRootFolderName = settings.GeneratedContentRootFolderName ?? @"_auto\";
            settings.FileFactory = new V1TemplateOutputFileBuilderStrategyFactory();

            var factory = new V1TemplateOutputFileBuilderStrategyFactory();
            if (strategies != null)
            {
                foreach (var strategy in strategies)
                {
                    factory.AddStrategy(strategy);
                }
            }
            settings.FileFactory = factory;

            GenerateApp(callingTemplate, settings);
        }

        public static void GenerateCustomApp(this TextTransformation callingTemplate, AppGenerationSettings settings = null, IEnumerable<ITemplateOutputFileBuilderStrategy> strategies = null)
        {
            // set all of the settings defaults
            //
            settings = settings ?? new AppGenerationSettings();
            settings.DataContextName = settings.DataContextName ?? "AppDataContext";
            settings.GeneratedContentRootFolderName = settings.GeneratedContentRootFolderName ?? @"_auto\";

            var factory = new CustomTemplateOutputFileBuilderStrategyFactory();
            if (strategies != null)
            {
                foreach (var strategy in strategies)
                {
                    factory.AddStrategy(strategy);
                }
            }
            settings.FileFactory = factory;

            GenerateApp(callingTemplate, settings);
        }

        private static void GenerateApp(TextTransformation callingTemplate, AppGenerationSettings settings)
        {
            try
            {
                using (var session = TransformationContext.Create(callingTemplate))
                {
                    settings.ProjectNamespacePrefix = settings.ProjectNamespacePrefix
                        ?? VisualStudio.Current.GetProjectItem(session.Host.TemplateFile).ContainingProject
                                                  .GetDefaultNamespace().SubstringUntilLastInstanceOf(".");
                    // create the app
                    //
                    var app = new App()
                    {
                        Settings = settings,
                        AppGenerationConfigProject = VisualStudio.Current.GetProjectItem(session.Host.TemplateFile).ContainingProject,
                        ModelProject = VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Model"),
                        DataServiceProject = VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".DataService"),
                        ServiceProject = VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Service"),
                        WebCommonProject =VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Web.Common"),
                        WebProject = VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Web"),
                        TestsCommonProject =VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Tests.Common"),
                        DataServiceTestsProject =VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Tests.DataService"),
                        ServiceTestsProject =VisualStudio.Current.GetProject(settings.ProjectNamespacePrefix + ".Tests.Service"),
                    };
                    
                    // create all of the domain types
                    //
                    var projectItems = app.AppGenerationConfigProject.ProjectItems.GetAllProjectItemsRecursive();
                    var paths = from p in projectItems
                        where p.GetFullPath().ToLowerInvariant().EndsWith(".domain.zae")
                        select p.GetFullPath();
                    new DomainFileParser().ParseDomainTypes(app, paths);

                    // create all of the Template output files
                    //
                    var filesToGenerate = new List<TemplateOutputFile>();
                    foreach (var strategy in app.Settings.FileFactory.GetAllStrategies())
                    {
                        filesToGenerate.AddRange(strategy.BuildOutputFiles(app));   
                    }
                    
                    //session.CallingTemplate.WriteLine(JsonConvert.SerializeObject(app, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));

                    // call the templates to build the files
                    //
                    foreach (var file in filesToGenerate)
                    {
                        foreach (var parameter in app.Settings.GlobalTemplateParameters)
                        {
                            if (file.TemplateParameters.ContainsKey(parameter.Key))
                            {
                                throw new Exception("Global GlobalTemplateParameters in Settings conflict with parameters a file to generate. Key = " + parameter.Key);
                            }
                            file.TemplateParameters.Add(parameter.Key, parameter.Value);
                        }
                        // finally the App
                        file.TemplateParameters.Add("App", app);
                        session.TransformAndCreateFile(file);
                    }
                }
            }
            catch (Exception e)
            {
                if (callingTemplate != null)
                {
                    try
                    {
                        callingTemplate.WriteLine(e.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
                throw;
            }
        }
    }
}
