using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#pragma warning disable CS8618

namespace RevitConsole
{
    public class ScriptGlobals
    {
        public Document doc;
        public UIDocument uidoc;
        private readonly IProgress<string> progress;

        public ScriptGlobals(IProgress<string> Progress)
        {
            progress = Progress;
        }

        public void Print(string Message)
        {
            progress.Report(Message);
        }
    }
}