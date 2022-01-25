using ACadAddinManager.View;
using ACadAddinManager.ViewModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace ACadAddinManager.Command
{
    public class AddinManagerCommand
    {
        [CommandMethod("AddinManager",CommandFlags.Session)]
        public void TestMethod()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor editor = doc.Editor;
            editor.WriteMessage("Add-in Manager Loaded");
            FrmAddInManager frmAddInManager = new FrmAddInManager(new AddInManagerViewModel());
            Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowModelessWindow(frmAddInManager);
        }
    }
}