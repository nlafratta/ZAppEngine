﻿using System.Collections.Generic;
using EnvDTE;
using Zirpl.AppEngine.VisualStudioAutomation.TextTemplating;

namespace Zirpl.AppEngine.VisualStudioAutomation.AppGeneration.Config
{
    public class App
    {
        public App()
        {
            this.DomainTypes = new List<DomainType>();
            this.FilesToGenerate = new List<TemplateOutputFile>();
        }

        public AppGenerationSettings Settings { get; internal set; }

        public IList<DomainType> DomainTypes { get; private set; }
        public IList<TemplateOutputFile> FilesToGenerate { get; private set; } 


        public Project AppGenerationConfigProject { get; internal set; }
        public Project ModelProject { get; internal set; }
        public Project DataServiceProject { get; internal set; }
        public Project DataServiceTestsProject { get; internal set; }
        public Project ServiceProject { get; internal set; }
        public Project WebProject { get; internal set; }
        public Project WebCommonProject { get; internal set; }
        public Project ServiceTestsProject { get; internal set; }
        public Project TestsCommonProject { get; internal set; }
    }
}
