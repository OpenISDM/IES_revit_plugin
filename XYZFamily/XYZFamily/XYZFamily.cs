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


namespace ExtractXYZ
{
    [Transaction(TransactionMode.Manual)]
    public class XYZFamily : IExternalCommand
    {
        public class FamilyFilter : ISelectionFilter //implement filter
        {
            bool ISelectionFilter.AllowElement(Element elem)
            {
                //return (elem.GetType().Equals(typeof(FamilyInstance)));//allow selecting if type = family
                return elem.Name == "Low Ceiling";//now only Family1 is allowed to be selected, just or the family names together if we have more types of beacon.           
            }

            bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
        const double _FeetTomm = 3.28084; //unit conversion
        public static double unitConversion(double ftValue)//conversion function
        {
            return ftValue / _FeetTomm;
            //return ftValue;       
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            FamilyFilter ff = new FamilyFilter();
            IList<Reference> sel = uidoc.Selection.PickObjects(ObjectType.Element, ff);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + doc.Title.Remove(doc.Title.Length - 4) + ".csv";//store the output data on desktop
            string output = "";
            using (StreamWriter sw = new StreamWriter(path, false))//overwrite the original file
            {
                foreach (Reference r in sel)
                {
                    try
                    {
                        Element e = doc.GetElement(r);
                        FamilyInstance fi = e as FamilyInstance;
                        LocationPoint lp = fi.Location as LocationPoint;
                        output = fi.Category.Name + "," + fi.Name + "," + fi.Id + "," + unitConversion(lp.Point.X) + "," + unitConversion(lp.Point.Y) + "," + unitConversion(lp.Point.Z);
                        sw.WriteLine(output);
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