using System;
using UIKit;
using System.Collections.Generic;
using Navigator.Pathfinding;

namespace Navigator.iOS
{
    public class CustomDirectionsController
    {
        UITableView _directionsTable;

        TableSource tableSource;


        public CustomDirectionsController (ViewController owner, UITableView searchPredictionTable, List<Room> rooms)
        {
            _directionsTable = searchPredictionTable;
           

            tableSource = new TableSource (owner, rooms);
            _directionsTable.Source = tableSource;
        }
    }
}

