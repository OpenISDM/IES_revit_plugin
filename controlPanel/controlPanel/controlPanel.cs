using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.IO;
namespace controlPanel
{
    public class controlPanel : IExternalApplication
    {
        public Result OnStartup(Autodesk.Revit.UI.UIControlledApplication app)
        {
            RibbonPanel panel = app.CreateRibbonPanel("Beacons");
            panel.Enabled = true;
            panel.Visible = true;
            panel.Name = "first Panel";
            panel.Title = "addinWizard";
            PushButtonData pbd = new PushButtonData("cmd1", @"C:\Users\jameschi\Documents\Visual Studio 2013\Projects\pickPoint\pickPoint\bin\Debug\pickPoint.dll", "cmd1", "cmd1");
            PushButton pb = panel.AddItem(pbd) as PushButton;
            pb.ToolTip = "Ext command1";
            panel.AddSeparator();

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
