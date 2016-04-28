using System;
using CoreLocation;

namespace Navigator.iOS
{
    public static class LocationHelper
    {
        static CLLocationManager locationManager = new CLLocationManager();

        static CLLocation lastLocation = null;

        const double PIx = Math.PI;
        const double RADIO = 6378.16;

        public static CLLocationManager UpdateLocation()
        {
            locationManager.RequestWhenInUseAuthorization();

            locationManager.StartUpdatingLocation();

            SetLastLocationOnUpdated();

            return locationManager;
        }

        static void SetLastLocationOnUpdated()
        {
            locationManager.LocationsUpdated += (sender, e) =>
            {
                foreach (CLLocation location in e.Locations)
                    lastLocation = location;
            };
        }
}

}

