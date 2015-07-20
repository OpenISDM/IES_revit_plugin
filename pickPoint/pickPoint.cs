using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;

[TransactionAttribute(TransactionMode.Manual)]

public class pickPoint : IExternalCommand
{
    public static Element FindElementByName(Document doc, Type targetType, string targetName)//helper function
    {
        return new FilteredElementCollector(doc).OfClass(targetType).FirstOrDefault<Element>(e => e.Name.Equals(targetName));
    }
    private void placeFamily(Autodesk.Revit.DB.Document doc, XYZ point)//private method to load and place a family
    {
        String fileName = @"C:\Users\jameschi\Documents\Family1.rfa";
        UIDocument uidoc = new UIDocument(doc);
        // try to load family
        Family family = FindElementByName(doc, typeof(Family), "Family1") as Family;
        if(null == family)//family not present
        {
            Transaction trans = new Transaction(doc);
            trans.Start("Load family");
            doc.LoadFamily(fileName, out family);
            trans.Commit();
        }

        //try to place family
        ISet<ElementId> familySymbolIds = family.GetFamilySymbolIds();
        foreach (ElementId id in familySymbolIds)//loop through all the symbols in set
        {
            Transaction trans = new Transaction(doc);
            trans.Start("Place family");
            FamilySymbol symbol = family.Document.GetElement(id) as FamilySymbol;
            if (!symbol.IsActive)//new function in revit2016
                symbol.Activate();
            FamilyInstance instance = doc.Create.NewFamilyInstance(point, symbol, StructuralType.NonStructural);
            //pickpoint method, this method can not place the beacon to the location I want, so maybe try other overloading method or use PromptForFamilyInstancePlacement method!
            trans.Commit();
            //uidoc.PromptForFamilyInstancePlacement(symbol);//interactively method but still has bug, maybe it's because of the implementation of beacons family, ask Terence for help!
        }
    }
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //Get application and document objects
        UIApplication uiApp = commandData.Application;
        Document doc = uiApp.ActiveUIDocument.Document;
        Selection sel = uiApp.ActiveUIDocument.Selection;
           
        //Pick a point
        XYZ point = sel.PickPoint("Please pick a point to place group");
        string strCoords = "Selected point is " + point.ToString();
        TaskDialog.Show("Revit", strCoords);

        //place the family instance at a specific point
        placeFamily(doc, point);
        return Result.Succeeded;
    }
}