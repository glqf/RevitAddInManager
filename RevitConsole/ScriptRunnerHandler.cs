﻿using System.Reflection;
using Autodesk.Revit.UI;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
#pragma warning disable CS8618

namespace RevitConsole
{
    public class ScriptRunnerHandler : IExternalEventHandler
    {
        public string ScriptText { get; internal set; }

        public string ScriptResult { get; private set; }

        public RoslynEditorViewModel RoslynEditorViewModel { get; set; }
        public IProgress<string> Progress { get; set; }

        public void Execute(UIApplication app)
        {
            var assembliesToRef = new List<Assembly>
            {
                typeof(object).Assembly, //mscorlib
                typeof(Autodesk.Revit.UI.UIApplication).Assembly, // Microsoft.CodeAnalysis.Workspaces
                typeof(Autodesk.Revit.DB.Document).Assembly, // Microsoft.Build
            };

            var namespaces = new List<string>
            {
                "Autodesk.Revit.UI",
                "Autodesk.Revit.DB",
                "Autodesk.Revit.DB.Structure",
                "System",
                "System.Collections.Generic",
                "System.IO",
                "System.Linq"
            };

            ScriptGlobals globals = new ScriptGlobals(Progress) { doc = app.ActiveUIDocument.Document, uidoc = app.ActiveUIDocument };

            var options = ScriptOptions.Default.AddReferences(assembliesToRef).WithImports(namespaces);

            try
            {
                object result = CSharpScript.EvaluateAsync<object>(ScriptText, options, globals).Result;
                if ( !(result is null) )
                    RoslynEditorViewModel.Result += CSharpObjectFormatter.Instance.FormatObject(result) + Environment.NewLine;
                RoslynEditorViewModel.IsRunning = System.Windows.Visibility.Collapsed;
            }
            catch ( AggregateException AggEx )
            {
                AggEx.Handle(ex =>
                                {
                                    RoslynEditorViewModel.Result += CSharpObjectFormatter.Instance.FormatObject(ex) + Environment.NewLine;
                                    return true;
                                }
                            );
                RoslynEditorViewModel.IsRunning = System.Windows.Visibility.Collapsed;
            }
            catch ( System.Exception ex )
            {
                RoslynEditorViewModel.Result += CSharpObjectFormatter.Instance.FormatObject(ex) + Environment.NewLine;
                RoslynEditorViewModel.IsRunning = System.Windows.Visibility.Collapsed;
            }
            finally
            {
                RoslynEditorViewModel.IsRunning = System.Windows.Visibility.Collapsed;
            }
        }

        public string GetName()
        {
            return "A Script Runner";
        }


    }
}