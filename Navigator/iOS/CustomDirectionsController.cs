using System.Collections.Generic;
using Navigator.Pathfinding;
using UIKit;

namespace Navigator.iOS
{
    public class CustomDirectionsController
    {
        private readonly UITableView _directionsTable;
        private readonly TableSource tableSource;

        public CustomDirectionsController(ViewController owner, UITableView searchPredictionTable, List<Room> rooms)
        {
            _directionsTable = searchPredictionTable;


            tableSource = new TableSource(owner, rooms);
            _directionsTable.Source = tableSource;
        }
    }
}