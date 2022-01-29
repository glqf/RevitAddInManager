using System.Collections.Immutable;
using System.Reflection;
using System.Windows;
using Autodesk.Revit.UI;
using RoslynPad.Roslyn;

namespace RevitConsole
{
    public static class ConsoleCommand
    {
        public static void Show()
        {
            try
            {

                var handler = new ScriptRunnerHandler();
                ExternalEvent externalEvent = ExternalEvent.Create(handler);
                var assembliesToRef = new List<Assembly>
                {
                    typeof(object).Assembly, //mscorlib
                    typeof(Autodesk.Revit.UI.UIApplication).Assembly,
                    typeof(Autodesk.Revit.DB.Document).Assembly,
                    Assembly.Load("RoslynPad.Roslyn.Windows"),
                    Assembly.Load("RoslynPad.Editor.Windows")
                };
                var roslynHost = new RevitRoslynHost(
                    additionalAssemblies: assembliesToRef,
                    references: RoslynHostReferences.NamespaceDefault.With(typeNamespaceImports: new[] { typeof(UIApplication), typeof(Autodesk.Revit.DB.Document), typeof(Dictionary<,>), typeof(System.Linq.Enumerable), typeof(ScriptGlobals) }),
                    disabledDiagnostics: ImmutableArray.Create("CS1701", "CS1702", "CS0518"));

                var document = new RoslynEditorViewModel(roslynHost, externalEvent, handler);
                RoslynEditor scriptEditor = new RoslynEditor(document);
                scriptEditor.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                handler.Progress = new Progress<string>(message => document.Result += message + Environment.NewLine);
                scriptEditor.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
        }
    }
}
