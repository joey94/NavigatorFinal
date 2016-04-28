###About ActionTray###

`ActionTray` is a dockable, customizable, slide-out, tray view controller for iOS and Android that can be attached to the top, left, bottom or right sides of the screen. `ActionTray` supports three tray types:

- *Draggable* - The user can drag the tray out from the edged anywhere between its open and closed positions or double tap it to snap between open and closed.
- *Popup* - The tray will snap between its open and closed positions when its **Drag Tab** is touched
- *AutoClosingPopup* - Just like the **Popup** tray but it will also close if the user taps its content area 

###Use Alone or in Groups###

You can place individual `ActionTray`s along any edge of the screen or place several `ActionTray`s togehter and attach them to an `ActionTrayManager` to control them as a group and use them like palettes or menus. The `ActionTrayManager` provides events to respond to user interaction in any of the trays it controls and it automatically ensures that only one tray in the group is open at a time.

###Events###

`ActionTray` defines the following events that you can monitor and respond to:

- Touched
- Moved
- Released
- Opened
- Closed

###Appearance###

`ActionTray` is fully customizable with user definable appearances for every element of its UI. `ActionTray` supports the following **Drag Tab** types:

- *Plain* - An empty **Drag Tab**
- *GripOnly* - A 3 line grip in the **Drag Tab**
- *GripAndText* - A 3 line grip and a title in the **Drag Tab**
- *TitleOnly* - Only the title in the **Drag Tab**
- *IconOnly* - Only an icon in the **Drag Tab**
- *IconAndTitle* - An icon and title in the **Drag Tab**
- *CustomDrawn* - Allows for a totally custom drawn **Drag Tab**

You can also position where the **Drag Tab** appears on the `ActionTray` as one of the following:

- *TopOrLeft* - Appears on the top or left side of the `ActionTray` based on its orientation
- *Middle* - Appears in the middle of the `ActionTray`
- *BottomOrRight* - Appears on the bottom or right side of the `ActionTray` based on its orientation
- *Custom* - You can control the position of the **Drag Tab** by setting the *tabOffset* property of the `ActionTray`

###iOS 8 Ready###

`ActionTray` now supports the iOS 7 look and feel by calling the **Flatten** method of the `ActionTray's` _appearance_ property. On iOS 7 and greater devices, the look will be switched automatically.

###Features###

`ActionTray` includes a fully documented **API** with comments for every feature. The `ActionTray` user interface is drawn with vectors and is fully resolution indenpendant.

###iOS Example###

`ActionTray` was designed to make adding it to a project super easy. Start an iPad, iPhone or Universal project in Xamarin Studio and build the project. Next, double click the _MyProjectViewController.xib_ file to open it in Xcode. Insert a _UIView_ and place it along one of the edges of the main _UIView_ making it a large as it will be when fully opened by the user. Next change its _Class_ to _UIActionTray_ and add any other views or components that will be part of the tray.


```
using Appracatappra.ActionComponents.ActionTray;
...

public override void ViewDidLoad ()
{
	base.ViewDidLoad ();

	...
	
	// Wireup the left side tray created in the .xib file and style
	// it to be a drag out tray.
	if (leftTray != null) {
		// Set tray type
		leftTray.orientation = UIActionTrayOrientation.Left;
		leftTray.tabLocation=UIActionTrayTabLocation.BottomOrRight;
		leftTray.frameType=UIActionTrayFrameType.EdgeOnly;
		leftTray.tabType=UIActionTrayTabType.IconAndTitle;

		// Style tray
		leftTray.appearance.background=UIColor.LightGray;
		leftTray.appearance.frame=UIColor.DarkGray;
		leftTray.icon=UIImage.FromFile ("Images/icon_calendar.png");
		leftTray.title="Events";
		leftTray.appearance.tabAlpha=0.25f;
		leftTray.CloseTray (false);

		// Respond to the tray being touched
		leftTray.Touched+= (tray) => {
			// Are we on an iPhone?
			if (UserInterfaceIdiomIsPhone) {
				//Yes, close this tray and animate the closing
				rightTray.CloseTray (true);
			}

			// Tell any open palette trays to close
			trayManager.CloseAllTrays ();

			// Close document tray
			if (documentTray!=null) 
				documentTray.CloseTray (true);
		};
	}

	// Wireup the right tray created in the .xib file and style it
	// to be a popup tray. Touch it's dragTab to open and close it.
	if (rightTray != null) {
		// Are we on an iPhone?
		if (UserInterfaceIdiomIsPhone) {
			// Move the subview into view and attach it to the master view
			rightTray.MoveTo (new PointF(320f-rightTray.Frame.Width,0f));
			View.AddSubview(rightTray);

			// iPhone specific settings
			rightTray.tabLocation=UIActionTrayTabLocation.BottomOrRight;
		}

		// Set tray type
		rightTray.trayType=UIActionTrayType.Popup;
		rightTray.orientation=UIActionTrayOrientation.Right;
		rightTray.bringToFrontOnTouch=true;
		if (UserInterfaceIdiomIsPhone) rightTray.CloseTray(false);

		// Style the tray
		rightTray.appearance.background=UIColor.DarkGray;

		// Respond to the tray being opened
		rightTray.Opened+= (tray) => {
			//Are we on an iPhone?
			if (UserInterfaceIdiomIsPhone) {
				//Yes, close this tray and animate the closing
				leftTray.CloseTray (true);
			}

			// Tell any open palette trays to close
			trayManager.CloseAllTrays ();

			// Close document tray
			if (documentTray!=null) 
				documentTray.CloseTray (true);
		};
	}

	...

}
```

_NOTE: `ActionTray`s and the UIViews that they control can be completely created in C# code without using .xib or storyboard files._

###Android Example###

`ActionTray` was designed to make adding it to a project super easy. Start an Android project in Xamarin Studio, switch to the Android Designer and add a RelativeLayout to be the parent of the `ActionTab`. Add one or more _Views_, switch to the Source view and change their type to _Appracatappra.ActionComponents.ActionTray.UIActionTray_. Wire-up their functionality inside your Main Activity:

```
using Appracatappra.ActionComponents.ActionTray;
...

[Activity (Label = "ActionTrayTest.Android", MainLauncher = true)]
public class Activity1 : Activity
{
	#region Private Variables
	private UIActionTray leftTray, rightTray, toolsTray, propertyTray, paletteTray, documentTray;
	#endregion 

	#region Public Variables
	public UIActionTrayManager trayManager;
	#endregion

	#region Override Methods
	protected override void OnCreate (Bundle bundle)
	{
		base.OnCreate (bundle);

		// Set our view from the "main" layout resource
		SetContentView (Resource.Layout.Main);

		// Gain Access to all views and controls in our layout
		leftTray = FindViewById<UIActionTray> (Resource.Id.trayLeft);
		rightTray = FindViewById<UIActionTray> (Resource.Id.trayRight);
		toolsTray = FindViewById<UIActionTray> (Resource.Id.trayTools);
		propertyTray = FindViewById<UIActionTray> (Resource.Id.trayProperty);
		paletteTray = FindViewById<UIActionTray> (Resource.Id.trayPalette);
		documentTray = FindViewById<UIActionTray> (Resource.Id.trayDocuments);

		// Create a TrayManager to handle a collection of "palette"
		// trays. It will automatically close any open tray when 
		// another tray in this collection is opened.
		trayManager = new UIActionTrayManager ();

		// Automatically close the left and right trays when any tray
		// in the manager's collection is opened
		trayManager.TrayOpened += (tray) => {
			// Animate the trays being closed
			leftTray.CloseTray (true);
			rightTray.CloseTray (true);
		};

		// Setup the left side tray
		leftTray.trayType = UIActionTrayType.Draggable;
		leftTray.orientation = UIActionTrayOrientation.Left;
		leftTray.tabLocation = UIActionTrayTabLocation.BottomOrRight;
		leftTray.frameType = UIActionTrayFrameType.EdgeOnly;
		leftTray.tabType = UIActionTrayTabType.IconAndTitle;
		leftTray.bringToFrontOnTouch=true;

		// Style tray
		leftTray.appearance.background = Color.Gray;
		leftTray.appearance.border = Color.Red;
		leftTray.icon = Resource.Drawable.icon_calendar;
		leftTray.title = "Events";
		leftTray.appearance.tabAlpha=100;
		leftTray.CloseTray (false);

		// Respond to the left tray being touched
		leftTray.Touched+= (tray) => {
			//Yes, close this tray and animate the closing
			rightTray.CloseTray (true);
			
			// Tell any open palette trays to close
			trayManager.CloseAllTrays ();
			
			// Close document tray
			documentTray.CloseTray (true);
		};

		// Setup the right side tray
		rightTray.trayType = UIActionTrayType.Popup;
		rightTray.orientation = UIActionTrayOrientation.Right;
		rightTray.bringToFrontOnTouch = true;
		rightTray.CloseTray (false);

		// Respond to the tray being opened
		rightTray.Opened+= (tray) => {
			//Close this tray and animate the closing
			leftTray.CloseTray (true);
			
			// Tell any open palette trays to close
			trayManager.CloseAllTrays ();
			
			// Close document tray
			documentTray.CloseTray (true);
		};

		// Set tray type
		documentTray.trayType = UIActionTrayType.AutoClosingPopup;
		documentTray.orientation = UIActionTrayOrientation.Bottom;
		documentTray.tabType = UIActionTrayTabType.GripAndTitle;
		documentTray.bringToFrontOnTouch=true;
		
		// Style tray
		documentTray.tabWidth = 125;
		documentTray.appearance.background = Color.Gray;
		documentTray.title = "Documents";
		documentTray.CloseTray (false);
		
		// Respond to the tray being opened
		documentTray.Opened += (tray) => {
			// Close left and right trays
			leftTray.CloseTray(true);
			rightTray.CloseTray(true);
		};

		//--------------------------------------------------------------------------------------
		// Create three action tray's and use them as a collection via an ActionTrayManager
		//--------------------------------------------------------------------------------------

		//--------------------------------------------------------------------------------------
		// Palette 1
		// Set tray type
		paletteTray.trayType = UIActionTrayType.AutoClosingPopup;
		paletteTray.orientation = UIActionTrayOrientation.Top;
		paletteTray.tabLocation = UIActionTrayTabLocation.TopOrLeft;
		paletteTray.tabType = UIActionTrayTabType.IconAndTitle;
		paletteTray.CloseTray (false);
		
		// Style tray
		paletteTray.tabWidth = 125;
		paletteTray.appearance.background = Color.Gray;
		paletteTray.icon = Resource.Drawable.icon_palette;
		paletteTray.title="Palette";
		
		// Add this tray to the manager's collection
		trayManager.AddTray (paletteTray);

		//--------------------------------------------------------------------------------------
		// Palette 2
		// Setup property tray type
		propertyTray.trayType = UIActionTrayType.Popup;
		propertyTray.orientation = UIActionTrayOrientation.Top;
		propertyTray.tabLocation = UIActionTrayTabLocation.TopOrLeft;
		propertyTray.tabType = UIActionTrayTabType.IconAndTitle;
		propertyTray.CloseTray (false);
		
		// Style tray
		propertyTray.tabWidth = 125;
		propertyTray.appearance.background = Color.Rgb (38,38,38);
		propertyTray.icon=Resource.Drawable.icon_measures;
		propertyTray.title="Properties";
		
		// Add this tray to the manager's collection
		trayManager.AddTray (propertyTray);

		//--------------------------------------------------------------------------------------
		// Palette 3
		// Setup tools tray type
		toolsTray.trayType = UIActionTrayType.AutoClosingPopup;
		toolsTray.orientation = UIActionTrayOrientation.Top;
		toolsTray.tabType = UIActionTrayTabType.IconOnly;
		toolsTray.CloseTray (false);
		
		// Style tools tray
		toolsTray.tabWidth = 50;
		toolsTray.tabLocation = UIActionTrayTabLocation.BottomOrRight;
		toolsTray.appearance.background = Color.Rgb (38,38,38);
		toolsTray.icon = Resource.Drawable.icon_pencil;
		
		// Add this tray to the manager's collection
		trayManager.AddTray (toolsTray);
	}

	protected override void OnSaveInstanceState (Bundle outState)
	{
		//Save the state of all trays on the screen
		outState.PutString("leftTray",leftTray.SaveState);
		outState.PutString("rightTray",rightTray.SaveState);
		outState.PutString("documentTray",documentTray.SaveState);
		outState.PutString("paletteTray",paletteTray.SaveState);
		outState.PutString("propertyTray",propertyTray.SaveState);
		outState.PutString("toolsTray",toolsTray.SaveState);
		
		base.OnSaveInstanceState (outState);
	}

	protected override void OnRestoreInstanceState (Bundle savedInstanceState)
	{
		//Restore all trays to their previous states
		leftTray.RestoreState(savedInstanceState.GetString("leftTray"));
		rightTray.RestoreState(savedInstanceState.GetString("rightTray"));
		documentTray.RestoreState(savedInstanceState.GetString("documentTray"));
		paletteTray.RestoreState(savedInstanceState.GetString("paletteTray"));
		propertyTray.RestoreState(savedInstanceState.GetString("propertyTray"));
		toolsTray.RestoreState(savedInstanceState.GetString("toolsTray"));

		base.OnRestoreInstanceState (savedInstanceState);
	}
	#endregion 
}
```

###Known Issues###

There is a know issue with the _dragTab_ hit target size and some Android based phones that can make an `ActionTray` hard to open on these devices. This issue will be addressed in a future release and the ability to programmatically increase the _height_ of the _dragTab_ will be added. While `ActionTray` now supports Android Ice Cream Sandwich (API Level 15), it performs better on Android Jelly Bean (API Level 17).


##Trial Version##

The Trial version of `ActionTray` is fully functional however the background is watermarked. The fully licensed version removes this watermark.

_Screenshots created with [PlaceIt](http://placeit.breezi.com "PlaceIt by Breezi") and may contain simulated functionality not included in the ActionTray._
