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
            RibbonPanel panel = app.CreateRibbonPanel("Beacon1");
            panel.Enabled = true;
            panel.Visible = true;
            panel.Name = "Beacon Panel1";
            panel.Title = "Beacons addin Wizard1";
            PushButtonData pbd = new PushButtonData("Beacon1", "Beacon1", @"C:\Users\jameschi\Documents\Visual Studio 2013\Projects\pickPoint\pickPoint\bin\Debug\pickPoint.dll", "pickPoint");
            PushButton pb = panel.AddItem(pbd) as PushButton;
            pb.ToolTip = "Beacon 1";
            panel.AddSeparator();

            RibbonPanel panel2 = app.CreateRibbonPanel("Beacon2");
            panel2.Enabled = true;
            panel2.Visible = true;
            panel2.Name = "Beacon Panel2";
            panel2.Title = "Beacons addin Wizard2";
            PushButtonData pbd2 = new PushButtonData("Beacon2", "Beacon2", @"C:\Users\jameschi\Documents\Visual Studio 2013\Projects\pickPoint\pickPoint\bin\Debug\pickPoint.dll", "pickPoint");
            PushButton pb2 = panel2.AddItem(pbd2) as PushButton;
            pb2.ToolTip = "Beacon 2";
            panel2.AddSeparator();
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
