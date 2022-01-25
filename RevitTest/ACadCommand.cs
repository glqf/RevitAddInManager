using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;

namespace Test
{
    public class ACadCommand
    {
        [CommandMethod("TestCommand")]
        public void TestCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage("Hello TestCommand");
        }
    }
}
