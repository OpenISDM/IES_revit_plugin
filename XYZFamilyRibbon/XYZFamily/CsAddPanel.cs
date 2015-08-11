/*
License and copyright Statement: to be added

Module Name:
 Extract XYZ

Abstract:
This file contains the source code of the Beacon Selection addin for Autodesk Revit 2016.
The following contains code to select and filter beacons, record the coordinates,
and publish technical specifications into a .csv file.

Authors:
Your name (your email address) 15-Oct-2012

Major Revisions:
 None

Environment:
 User mode only.
*/

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
using XYZFamily;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Point = GeoJSON.Net.Geometry.Point;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Feature;

namespace ExtractXYZ
{
    public class CsAddPanel : IExternalApplication
    {

        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Extract Beacons");

            // Create a push button in panel
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdYZFamily",
               "Extract Beacons", thisAssemblyPath, "ExtractXYZ.XYZFamily");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Output the beacon's XYZ-coordinates into text file .";

            Uri uriImage = new Uri(@"pack://application:,,,/XYZFamily;component/Resources/cartesiancoordinates.png");

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
        // Allow selecting if type Beacon Family
                if (elem.Name == "Low Ceiling")
                    return true;
                else if (elem.Name == "High Ceiling")
                    return true;
                else if (elem.Name == "Beacon Std.")
                    return true;
                else
                    return false; 
        /* Now only the Beacon Family is allowed to be selected       
        If more beacons types are desired, insert above */
            }

            bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Create a new feature collections object, beacons will be represented by features
            FeatureCollection features = new FeatureCollection();

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            FamilyFilter ff = new FamilyFilter();
            IList<Reference> sel = uidoc.Selection.PickObjects(ObjectType.Element, ff);
        //store the output data on desktop
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) 
                + "\\" 
                + doc.Title.Remove(doc.Title.Length - 4) 
                + ".json";

        //Overwrite the original file if action is duplicated
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (Reference r in sel)
                {
                    try
                    {
                        Element e = doc.GetElement(r);
                        FamilyInstance fi = e as FamilyInstance;
                        LocationPoint lp = fi.Location as LocationPoint;

                        // Create a new beacon and add it to the feature collection as a feature
                        Beacon beacon = new Beacon(fi, lp);
                        features.Features.Add(beacon.toGeoJSONFeature());
                    }
                    catch (Exception e)
                    {
                        TaskDialog.Show("Revit", e.ToString());

                    }
                }
                // Convert the features collection to GeoJSON and output to external file
                sw.WriteLine(JsonConvert.SerializeObject(features));
            }
            return Result.Succeeded;
        }
    }
}