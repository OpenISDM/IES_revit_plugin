﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.IO;
using XYZFamily;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;

namespace ExtractXYZ
{
    public class CsAddPanel : IExternalApplication
    {
       
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Extract XYZ");

            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdYZFamily",
               "Extract XYZ", thisAssemblyPath, "ExtractXYZ.XYZFamily");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Output the beacon's XYZ-coordinates into text file .";

            // b) large bitmap
            Uri uriImage = new Uri(@"C:\Users\Public_A\Documents\GitHub\IES_revit_plugin\XYZFamilyRibbon\cartesiancoordinates.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class XYZFamily : IExternalCommand
    {
        public class FamilyFilter : ISelectionFilter //implement filter
        {
            bool ISelectionFilter.AllowElement(Element elem)
            {
                //return (elem.GetType().Equals(typeof(FamilyInstance)));//allow selecting if type = family
                if (elem.Name == "Low Ceiling")
                    return elem.Name == "Low Ceiling";
                else if (elem.Name == "High Ceiling")
                    return elem.Name == "High Ceiling";
                else if (elem.Name == "Beacon Std.")
                    return true;
                else
                    return false; //now only Family1 is allowed to be selected, just or the family names together if we have more types of beacon.           
            }

            bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            FamilyFilter ff = new FamilyFilter();
            IList<Reference> sel = uidoc.Selection.PickObjects(ObjectType.Element, ff);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + doc.Title.Remove(doc.Title.Length - 4) + ".csv";//store the output data on desktop
            using (StreamWriter sw = new StreamWriter(path, false))//overwrite the original file
            {
                foreach (Reference r in sel)
                {
                    try
                    {
                        Element e = doc.GetElement(r);
                        FamilyInstance fi = e as FamilyInstance;
                        LocationPoint lp = fi.Location as LocationPoint;
                        Beacon beacon = new Beacon(fi, lp);
                        sw.WriteLine(beacon.toString());
                    }
                    catch
                    {
                        TaskDialog.Show("Revit", "Error!!!\n");
                    }
                }
            }
            return Result.Succeeded;
        }
    }
}