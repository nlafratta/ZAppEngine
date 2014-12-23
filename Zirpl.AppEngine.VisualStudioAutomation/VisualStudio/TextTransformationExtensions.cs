﻿using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TextTemplating;
using Zirpl.AppEngine.VisualStudioAutomation.TextTemplating;

namespace Zirpl.AppEngine.VisualStudioAutomation.VisualStudio
{
    public static class TextTransformationExtensions
    {
        public static DTE2 GetDTE(this ITransform textTransformation)
        {
            return (DTE2)((IServiceProvider)textTransformation.Host.Host).GetCOMService(typeof(DTE));
        }

        public static ProjectItem GetProjectItem(this ITransformHost host)
        {
            return host.HostTransform.GetDTE().Solution.GetProjectItem(host.Host.TemplateFile);
        }
    }
}
