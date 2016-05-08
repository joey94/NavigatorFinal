using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CoreGraphics;
using CoreLocation;
using CoreMotion;
using Foundation;
using Navigator.Pathfinding;
using UIKit;
using CoreAnimation;
using Navigator.Helpers;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Navigator.Primitives;

namespace Navigator.iOS
{
    public partial class ViewController : UIViewController
    {

        //Instantiate step detector and collision class
        private ICollision col;
        public WallCollision wallColTest;

		//Will contain graph data
        private Graph floorPlanGraph;

		//Arrow to display users location
		private LocationArrowImageView locationArrow;

		//Will de used to display the floorplans
		private UIImageView floorplanImageView;
        private UIImage floorplanWallCol;
        private UIImage floorplanImageNoGrid;
        private UIImage floorplanImageWithGrid;
        private UIImage floorplanFirstFloorNoGrid;
        private UIImage floorplanFirstFloorWallCol;
        private UIImage floorplanSci;
        private UIImage wallCollImg;

		//Keeps track of steps taken
        private int GlobalStepCounter = 0;

		//Will hold the paths users should follow
		private PathView pathView;

        //Location manager for heading information
        private CLLocationManager locationManager;
        private CMMotionManager motionManager;

        private List<CGPoint> directionsPointList = new List<CGPoint>();

		//Toggle for button press
        private int toggle = 1;

        private int count = 0;

        private nfloat endX = 0, endY = 0;

        private Pathfinding.Pathfinding pf;

        private bool pathDisplayed = false;

        private int directionCount = 0;

        public int floor = 0;

        public GraphLocatable StartNavigationPosition = null;
        public GraphLocatable EndNavgiationPosition = null;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // For accelerometer readings
            motionManager = new CMMotionManager();
            motionManager.AccelerometerUpdateInterval = 0.01; // 100Hz

            //To handle long presses and bring up path start/end menu
            var longPressManager = new UILongPressGestureRecognizer();

            //Graph loading code
			//Graph loading code
			var assembly = Assembly.GetExecutingAssembly();
			var asset = assembly.GetManifestResourceStream("Navigator.iOS.Resources.dcsfloorWideDoors.xml");
            var asset2 = assembly.GetManifestResourceStream("Navigator.iOS.Resources.dcsFloor1.xml");
            var assetSci = assembly.GetManifestResourceStream("Navigator.iOS.Resources.ConFloor.xml");


			pf = new Pathfinding.Pathfinding(new Dictionary<int, Stream>()
				{
					{0,assembly.GetManifestResourceStream("Navigator.iOS.Resources.dcsfloorWideDoors.xml")},
					{1,assembly.GetManifestResourceStream("Navigator.iOS.Resources.dcsFloor1.xml")},
                    {2,assembly.GetManifestResourceStream("Navigator.iOS.Resources.ConFloor.xml")}
				},assembly.GetManifestResourceStream("Navigator.iOS.Resources.Rooms.xml") );
			pf.CurrentFloor = 0;

			while (true)
			{
				if (pf.Ready)            

					break;
				Thread.Sleep(500);
			}












				
			//set up the search bar and prediction box
			SearchBar.TintColor = UIColor.White;
			UITextView.AppearanceWhenContainedIn (typeof(UISearchBar)).BackgroundColor = UIColor.White;
			UITextView.AppearanceWhenContainedIn (typeof(UISearchBar)).TintColor = UIColor.White;
			UITextField.AppearanceWhenContainedIn (typeof(UISearchBar)).BackgroundColor = UIColor.White;
			UITextField.AppearanceWhenContainedIn (typeof(UISearchBar)).TintColor = UIColor.White;


			var shadowView = new UIView(SearchPredictionTable.Frame);
			shadowView.BackgroundColor = UIColor.White;
			shadowView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
			shadowView.Layer.ShadowOpacity = 1.0f;
			shadowView.Layer.ShadowRadius = 6.0f;
			shadowView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
			shadowView.Layer.ShouldRasterize = true;
			shadowView.Layer.MasksToBounds = false;
			Add (shadowView);


			var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Dark);
			var topblurView = new UIVisualEffectView (blur) {
				Frame = new RectangleF (0, 0, (float) View.Frame.Width, 90)
			};
			var bottomblurView = new UIVisualEffectView (blur) {
				Frame = new RectangleF (0, (float) View.Frame.Height - 70, (float) View.Frame.Width, 70)
			};

			View.Add (topblurView);
			View.Add (bottomblurView);

			View.BringSubviewToFront (SearchPredictionTable);
			View.BringSubviewToFront (returnButton);
			View.BringSubviewToFront (SearchBar);
			View.BringSubviewToFront (directionsButton);











			var searchController = new CustomSearchController(this, SearchBar, SearchPredictionTable, shadowView, pf.Rooms);
            //var directionsController = new CustomDirectionsController (this, directionsTable, pf.Rooms);

            floorPlanGraph = Graph.Load(asset);
            floorplanWallCol = UIImage.FromBundle("Images/dcsfloorWideDoors.png");
            wallCollImg = floorplanWallCol;

            col = new Collision(floorPlanGraph, new StepDetector());

            ((Collision)col).WallCol = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));
            wallColTest = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));

            pathView = new PathView (wallColTest, this);

            col.SetLocation(707.0f, 677.0f);
            col.PassHeading(90);
            col.PositionChanged += HandleStepsTaken;

			//Container for floorplan and any overlaid images
            var container = new UIView();

			//Will contain floorplan images
            floorplanImageView = new UIImageView();

			//Load floorplan images
            floorplanImageNoGrid = UIImage.FromBundle("Images/FinalDcsFloor1.png");
            floorplanImageWithGrid = UIImage.FromBundle("Images/dcsFloorWideDoorsGrid.png");
            floorplanFirstFloorNoGrid = UIImage.FromBundle ("Images/final2ndFloorDisplay.png");
            floorplanSci = UIImage.FromBundle ("Images/ConFloorGrid");
            floorplanFirstFloorWallCol = UIImage.FromBundle ("Images/dcsFloor1.png");


			//Initiate the location arrow
            locationArrow = new LocationArrowImageView();
            locationArrow.ScaleFactor = floorplanView.ZoomScale;
            pathView.ScaleFactor = floorplanView.ZoomScale;
            setStartPoint(690.0f, 840.0f, this.floor);

			//Set sizes for floorplan view and path view
            floorplanView.ContentSize = floorplanImageNoGrid.Size;
            pathView.Frame = new CGRect(new CGPoint(0, 0), floorplanImageNoGrid.Size);

			//Add subviews to the container (including pathview and floorplanview)
            container.AddSubview(floorplanImageView);
            container.AddSubview(locationArrow);
            floorplanImageView.AddSubview(pathView);
            changeFloorPlanImage(floorplanImageView, floorplanImageNoGrid);
            container.SizeToFit();

			//Adjust scrolling and zooming properties for the floorplanView
            floorplanView.MaximumZoomScale = 1f;
            floorplanView.MinimumZoomScale = .25f;
            floorplanView.AddSubview(container);
            floorplanView.ViewForZoomingInScrollView += (UIScrollView sv) => { return floorplanImageView; };

			//Variables needed to convert device acceleration to world z direction acceleration
			double accelX = 0, accelY = 0, accelZ = 0;

			//Scale location arrow and paths when zooming the floorplan
            floorplanView.DidZoom += (sender, e) =>
            {
                locationArrow.ScaleFactor = floorplanView.ZoomScale;
                pathView.ScaleFactor = floorplanView.ZoomScale;
            };










			floorplanView.DraggingStarted += (sender, e) => 
			{
				handleAnimate(topblurView, bottomblurView, true);
			};

			var tapGestureRecognizer = new UITapGestureRecognizer();
			tapGestureRecognizer.NumberOfTapsRequired = 1;
			tapGestureRecognizer.AddTarget (() => {
				handleAnimate (topblurView, bottomblurView, false);
			});
			floorplanView.AddGestureRecognizer(tapGestureRecognizer);

			//Pass acceleremoter values to the collision class
            motionManager.StartAccelerometerUpdates(NSOperationQueue.CurrentQueue,
                (data, error) =>
                {
					accelX = data.Acceleration.X*9.8;
					accelY = data.Acceleration.Y*9.8;
					accelZ = Math.Sqrt(Math.Pow(accelX, 2) + Math.Pow(accelY, 2) + Math.Pow(data.Acceleration.Z*9.8, 2));

                    col.PassSensorReadings(CollisionSensorType.Accelometer, accelX,
                        accelY, accelZ);
                    //displayAccelVal((float)accelZ);
                });

			//LongPressManager will cause the path input menu to appear after a stationary long press
            longPressManager.AllowableMovement = 0;
            longPressManager.AddTarget(() => handleLongPress(longPressManager, floorPlanGraph));
            floorplanView.AddGestureRecognizer(longPressManager);

			//the location manager handles the phone heading
            locationManager = new CLLocationManager();
            locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            locationManager.HeadingFilter = 1;
            locationManager.UpdatedHeading += HandleUpdatedHeading;
            locationManager.StartUpdatingHeading();

			//Another testing button

            returnButton.TouchUpInside += delegate{ returnToMenu(); };

            directionsButton.TouchUpInside += delegate {
                
                if (floor == 0) {
                    Console.Out.WriteLine("Changing to floor 1 ");
                    removePath();
                    floor = 1;
                    changeFloorPlanImage(floorplanImageView, floorplanFirstFloorNoGrid);
                    setStartPoint( 447.0f,  850.0f, this.floor,false);

                    wallCollImg = floorplanFirstFloorWallCol;

                    col = new Collision(floorPlanGraph, new StepDetector());

                    ((Collision)col).WallCol = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));
                    wallColTest = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));

                    pathView = new PathView (wallColTest, this);

                    col.SetLocation(447, 850);
                    col.PositionChanged += HandleStepsTaken;


                }
                else if (floor == 1) {
                    Console.Out.WriteLine("Changing to floor 0 ");
                    removePath();
                    floor = 0;
                    changeFloorPlanImage(floorplanImageView, floorplanImageNoGrid);

                    setStartPoint(486.0f, 980.0f,this.floor,false);

                    wallCollImg = floorplanWallCol;

                    col = new Collision(floorPlanGraph, new StepDetector());

                    ((Collision)col).WallCol = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));
                    wallColTest = new WallCollision ((x,y) => GetPixelColor(new PointF(x, y), wallCollImg));

                    pathView = new PathView (wallColTest, this);

                    col.SetLocation(486, 980);
                    col.PositionChanged += HandleStepsTaken;

                }
                else 
                    floor = 0;

            };

        }

       

        private int GetPixelColor(PointF myPoint, UIImage myImage)
        {
            var rawData = new byte[4];
            var handle = GCHandle.Alloc(rawData);
            int resultColor = 0;
            try
            {
                using (var colorSpace = CGColorSpace.CreateDeviceRGB())
                {
                    using (var context = new CGBitmapContext(rawData, 1, 1, 8, 4, colorSpace, CGImageAlphaInfo.PremultipliedLast))
                    {
                        context.DrawImage(new RectangleF(-myPoint.X, (float)(myPoint.Y - myImage.Size.Height), (float)myImage.Size.Width, (float)myImage.Size.Height), myImage.CGImage);
                        resultColor = (((int)rawData[0] & 0xFF) << 24) | //alpha
                            (((int)rawData[1] & 0xFF) << 16) | //red
                            (((int)rawData[2] & 0xFF) << 8) | //green
                            (((int)rawData[3] & 0xFF) << 0); //blue
                    }
                }
            }
            finally
            {
                handle.Free();
            }
            return resultColor;
        }

        private void returnToMenu() {
            NavigationController.PopViewController (true);
        }

        private void HandleUpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
        {
            var newRad = (float) ((e.NewHeading.TrueHeading*Math.PI/180f) + (45*Math.PI/180f));
            col.PassHeading(newRad);

            //floorplanImageView.Layer.AnchorPoint = new CGPoint (locationArrow.X/floorplanImageNoGrid.Size.Width, locationArrow.Y/floorplanImageNoGrid.Size.Height);
            locationArrow.lookAtHeading(newRad);
        }

        private void HandleStepsTaken(object s, PositionChangedHandlerEventArgs args)
        {
            GlobalStepCounter++;
            locationArrow.setLocation(args.newX, args.newY);


            /*if (GlobalStepCounter % 12 == 0 && pathDisplayed == true) {
                drawPathFromUser ((float)endX, (float)endY);
            }*/
        }


        private void floorplanLookAtHeading(float angle)
        {
            floorplanImageView.Transform = CGAffineTransform.MakeRotation(angle);
        }

        private void changeFloorPlanImage(UIImageView imageView, UIImage image)
        {
            imageView.Image = image;
            imageView.SizeToFit();
        }
            

        Dictionary<int, List<UndirEdge>> CurrentUserPath = new Dictionary<int, List<UndirEdge>>();

        private void findUserPath(){
            if (StartNavigationPosition == null || EndNavgiationPosition == null)
                return;




            //Get x and y of this nearest node
            var pathStart = this.pf.FloorGraphs[StartNavigationPosition.Floor].FindClosestNode(StartNavigationPosition.X,StartNavigationPosition.Y,6);

            //Get x and y of this node
            var pathEnd = this.pf.FloorGraphs[EndNavgiationPosition.Floor].FindClosestNode(EndNavgiationPosition.X,EndNavgiationPosition.Y,6);

            //Calculate path
            var path = this.pf.FindPath(new GraphLocatable(pathStart.X,pathStart.Y,StartNavigationPosition.Floor),new GraphLocatable(pathEnd.X,pathEnd.Y,EndNavgiationPosition.Floor));
            //Get path length
            CurrentUserPath = path;

        }

        private void drawUserPath(){

            if (!CurrentUserPath.ContainsKey (this.floor))
                return;

            var pathLength = CurrentUserPath[this.floor].Count();

            if (pathLength == 0)
                return;

            //Extract node along path
            var pathPoints = new List<CGPoint>();
                string test = "";
            foreach (var pathEdge in CurrentUserPath[this.floor]) {

                var startPoint = new Vector2 (pathEdge.Source);
                test += " ? " + startPoint.ToPointString ();
                pathPoints.Add (new CGPoint (startPoint.X, startPoint.Y));
            }
            removePath ();

            //Draw path on screen
            
            Console.WriteLine("Points in the draw path = {0}, Floor = {1} , Points : {2}",pathPoints.Count,this.floor,test);
            pathView.setPoints(pathPoints.ToArray());
            pathView.FLOOR = this.floor;

        }



        private void handleLongPress(UILongPressGestureRecognizer gesture, Graph g)
        {
            //Get x and y of press location
			var tapX = (float) gesture.LocationInView(floorplanImageView).X;
			var tapY = (float) gesture.LocationInView(floorplanImageView).Y;
            var node = this.pf.FloorGraphs [this.floor].FindClosestNode ((int)tapX, (int)tapY);
			// Create a new Alert Controller
            showContextMenu(tapX,tapY,this.floor);


			/*
			CGPoint point = new CGPoint (gesture.LocationInView (floorplanImageView).X, gesture.LocationInView (floorplanImageView).Y);

			UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
			if (presentationPopover!=null) {
				presentationPopover.SourceRect = new CGRect(point, new CGSize(0.1, 0.1));
				//presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			}
			*/      
		}

		private bool menuHidden = false;	
		private void handleAnimate(UIView topblurView, UIView bottomblurView, bool hide){
			var d = 90;
			if (hide != menuHidden) {
				if (hide == true && menuHidden == false) d = 90;
				else d = -90;
				UIView.Animate (0.4, 0, UIViewAnimationOptions.CurveEaseInOut , 
					() => {
						topblurView.Center = new CGPoint (topblurView.Center.X, topblurView.Center.Y - d);
						bottomblurView.Center = new CGPoint (bottomblurView.Center.X, bottomblurView.Center.Y + d);
						SearchBar.Center = new CGPoint (SearchBar.Center.X, SearchBar.Center.Y - d);
						returnButton.Center = new CGPoint (returnButton.Center.X, returnButton.Center.Y - d);
						directionsButton.Center = new CGPoint (directionsButton.Center.X, directionsButton.Center.Y + d);
					}, () => {
					topblurView.Center = topblurView.Center;
					bottomblurView.Center = bottomblurView.Center;
					SearchBar.Center = SearchBar.Center;
					returnButton.Center = returnButton.Center;
					directionsButton.Center = directionsButton.Center;
				});
				menuHidden = !menuHidden;
			}

		}

        public void showContextMenu(float locationX, float locationY, int roomFloor){
			UIAlertController actionSheetAlert = UIAlertController.Create("Options", null, UIAlertControllerStyle.Alert);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create("Cancel",UIAlertActionStyle.Cancel, null));
            actionSheetAlert.AddAction(UIAlertAction.Create("Set Start Point",UIAlertActionStyle.Default, (action) => {pathDisplayed = false;setStartPoint(locationX, locationY,roomFloor);}));
            actionSheetAlert.AddAction(UIAlertAction.Create("Set End Point",UIAlertActionStyle.Default, (action) => {pathDisplayed = true;setEndPoint(locationX, locationY,roomFloor);}));
            actionSheetAlert.AddAction(UIAlertAction.Create("Remove Path",UIAlertActionStyle.Default, (action) => removePath()));


			// Display alert
			this.PresentViewController(actionSheetAlert,true,null);
		}

        public void pushDirectionsPointsList(List<CGPoint> pl) {
            directionsPointList = pl;
        }

        private void removePath() {
            InvokeOnMainThread (() => {
                Console.Out.WriteLine ("DROPPING THE FUCKING SHIT");
                pathView.RemoveFromSuperview ();
                pathView.Dispose ();
                pathView = new PathView (wallColTest, this);
                pathView.ScaleFactor = floorplanView.ZoomScale;
                pathView.Frame = new CGRect (new CGPoint (0, 0), floorplanImageNoGrid.Size);
                floorplanImageView.AddSubview (pathView);
                pathDisplayed = false;
                directionCount = 0;
            });
        }

        public void displayAccelVal(float a) {
            count++;
            if (count > 400) {
                breakpointCheck (a);
            }
        }
        private void breakpointCheck (float a){

        }

        public void setStartPoint(nfloat x, nfloat y, int roomFloor,bool shouldPf = true) {
			locationArrow.setLocation ((float)x, (float)y);
			col.SetLocation ((float)x, (float)y);
            pathView.RemoveFromSuperview ();
            pathView = new PathView(wallColTest, this);
            pathView.ScaleFactor = floorplanView.ZoomScale;
            pathView.Frame = new CGRect(new CGPoint(0, 0), floorplanImageNoGrid.Size);
            floorplanImageView.AddSubview(pathView);
            SearchBar.ShowsCancelButton = false;
            SearchBar.ResignFirstResponder();
            directionCount = 0;
            StartNavigationPosition = new GraphLocatable ((float)x, (float)y, roomFloor);
            if (shouldPf)
                findUserPath();
            drawUserPath ();
		}

        public void setEndPoint(nfloat x, nfloat y, int roomFloor) {
            SearchBar.ShowsCancelButton = false;
            SearchBar.ResignFirstResponder();
            endX = x;
            endY = y;

            directionCount = 0;

            EndNavgiationPosition = new GraphLocatable ((float)x, (float)y, roomFloor);

            //if (pathDisplayed == true) {
            findUserPath();
            drawUserPath ();
           // }
		}
        int counter2 = 0;
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            counter2++;
            // Release any cached data, images, etc that aren't in use.		
        }

		partial void ReturnButton_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ();
		}
    }
}