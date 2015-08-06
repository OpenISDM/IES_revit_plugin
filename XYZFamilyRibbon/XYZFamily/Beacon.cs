using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;
using Point = GeoJSON.Net.Geometry.Point;
using Newtonsoft.Json;

namespace XYZFamily
{
    class Beacon
    {
        private String categoryName;
        private String beaconType;
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
            this.beaconType = fi.Name;
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
         * Getter for beacon's type
         */
        public String BeaconType
        {
            get
            {
                return this.beaconType;
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
         * Getter for a beacon's coordinates in Point format
         */ 
        public Point BeaconCoordinates
        {
            get
            {
                return new Point(new GeographicPosition(xLoc, yLoc, zLoc));
            }
        }

        /*
         * GeoJSON Feature Representation of a beacon
         */
        public Feature toGeoJSONFeature()
        {
            var properties = new Dictionary<String, object>();
            properties.Add("Beacon Type", beaconType);
            properties.Add("Element Id", elementId);

            var feature = new Feature(BeaconCoordinates, properties);
            return feature;
        }

        /*
         * Simple string representation of a beacon
         */ 
        public String toString()
        {
            return "Category: " + categoryName + ", Beacon Type: " + beaconType + ",Element Id: " + elementId + ", (" + xLoc + "," + yLoc + "," + zLoc + ")\n";
        }
    }
}
