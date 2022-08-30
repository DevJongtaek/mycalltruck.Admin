using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mycalltruck.Admin.Class
{
    class LocationHelper
    {
        public PointD GetLocactionFromAddress(CMDataSet.AddressReferencesRow Address)
        {
            PointD r = new PointD();
            //주소로 좌표 검색
            if (Address.X == 0 || Address.Y == 0)
            {
                var address = string.Format("{0} {1} {2}", Address.State, Address.City, Address.Street);
                //var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false&key=AIzaSyDB8gMrSrX7rN63Ioz48watQtYG4nbPHQg", Uri.EscapeDataString(address));

                var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());

                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                var Y = locationElement.Element("lat").Value;
                var X = locationElement.Element("lng").Value;
                Address.X = double.Parse(X);
                Address.Y = double.Parse(Y);
                CMDataSetTableAdapters.AddressReferencesTableAdapter Adapter = new CMDataSetTableAdapters.AddressReferencesTableAdapter();
                Adapter.Update(Address);
            }
            r.X = Address.X;
            r.Y = Address.Y;
            return r;
        }
        public double DistanceFromGPS(double x1, double y1, double x2, double y2)
        {
            double r = 0;
            var theta = x1 - x2;
            var dist = Math.Sin(_deg2rad(y1)) * Math.Sin(_deg2rad(y2)) + Math.Cos(_deg2rad(y1)) * Math.Cos(_deg2rad(y2)) * Math.Cos(_deg2rad(theta));
            dist = Math.Acos(dist);
            dist = _rad2deg(dist);
            if (Double.IsNaN(dist))
            {
                dist = 0;
            }
            //dist = dist * 60 * 1.1515 * 1.609344 * 0.1;
            dist = dist * 60 * 1.609344;
            r += dist;
            return r;
        }
        private double _deg2rad(double deg)
        {
            var radians = 0.0;
            radians = deg * Math.PI / 180.0;
            return radians;
        }
        private double _rad2deg(double rad)
        {
            var deg = 0.0;
            deg = rad * 180.0 / Math.PI;
            return deg;
        }

        public class PointD
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
