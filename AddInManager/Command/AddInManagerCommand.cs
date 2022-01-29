﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddinManager.Model;
using RevitConsole;

namespace RevitAddinManager.Command;

[Transaction(TransactionMode.Manual)]
public class Console : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        ConsoleCommand.Show();
        return Result.Succeeded;
    }
}
[Transaction(TransactionMode.Manual)]
public class AddInManagerManual : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        StaticUtil.RegenOption = RegenerationOption.Manual;
        StaticUtil.TransactMode = TransactionMode.Manual;
        return AddinManagerBase.Instance.ExecuteCommand(commandData, ref message, elements, false);
    }
}
[Transaction(TransactionMode.Manual)]
public class AddInManagerFaceless : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        return AddinManagerBase.Instance.ExecuteCommand(commandData, ref message, elements, true);
    }
}
[Transaction(TransactionMode.Manual)]
public class AddInManagerReadOnly : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        StaticUtil.RegenOption = RegenerationOption.Manual;
        StaticUtil.TransactMode = TransactionMode.ReadOnly;
        return AddinManagerBase.Instance.ExecuteCommand(commandData, ref message, elements, true);
    }
}