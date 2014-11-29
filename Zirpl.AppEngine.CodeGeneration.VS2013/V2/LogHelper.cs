﻿using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextTemplating;
using Zirpl.AppEngine.CodeGeneration.TextTemplating;
using Zirpl.AppEngine.CodeGeneration.V2;

namespace Zirpl.AppEngine.CodeGeneration.V2
{
    public static class LogHelper
    {
        /// <summary>
        /// Writes a line to the build pane in visual studio and activates it
        /// </summary>
        /// <param name="message">Text to output - a \n is appended</param>
        public static void LogLineToBuildPane(this Object anything, string message)
        {
            TextTransformationSession.Instance.CallingTemplate.Host.LogToBuildPane(String.Format("{0}\n", message));
        }
    }
}
