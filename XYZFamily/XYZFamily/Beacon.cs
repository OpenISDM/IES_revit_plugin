using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace XYZFamily
{
    class Beacon
    {
        private String categoryName;
        private String familyName;
        private ElementId elementId;
        private double xLoc;
        private double yLoc;
        private double zLoc;

        /*
         * Initialize beacon object from FamilyInstance and LocationPoint
         * 
         */
         
        public Beacon(FamilyInstance fi, LocationPoint lp)
        {
            this.categoryName = fi.Category.Name;
            this.familyName = fi.Name;
            this.elementId = fi.Id;
            // Stores as feet internally so convert to meters
            this.xLoc = Utilities.feetToMeters(lp.Point.X);
            this.yLoc = Utilities.feetToMeters(lp.Point.Y);
            this.zLoc = Utilities.feetToMeters(lp.Point.Z);
        }

        /*
         * Getter for beacon's category name
         */ 
        public String CategoryName
        {
            get
            {
                return this.categoryName;
            }
        }

        /*
         * Getter for beacon's family name
         */
        public String FamilyName
        {
            get
            {
                return this.familyName;
            }
        }

        /*
         * Getter for beacon's element id
         */
        public ElementId ElementId
        {
            get
            {
                return this.elementId;
            }
        }

        /*
         * Getter for beacon's x location coordinate
         */ 
        public double XLoc
        {
            get
            {
                return this.xLoc;
            }
        }

        /*
         * Getter for beacon's y location coordinate
         */
        public double YLoc
        {
            get
            {
                return this.yLoc;
            }
        }

        /*
         * Getter for beacon's z location coordinate
         */
        public double ZLoc
        {
            get
            {
                return this.zLoc;
            }
        }

        /*
         * Simple string representation of a beacon
         */ 
        public String toString()
        {
            return "Category: " + categoryName + ", Family: " + familyName + ",Element Id: " + elementId + ", (" + xLoc + "," + yLoc + "," + zLoc + ")\n";
        }
    }
}
