##ActionTray Component##

###Getting Started with ActionTrays###

To use an `ActionTray` in your mobile application include the `ActionPack.iOS` or `ActionPack.Android` component and reference the following using statement in your C# code:

```
using Appracatappra.ActionComponents.ActionTray;
``` 

###Minimal Setup Required###

Whether created as a .xib file in Xcode for iOS or the designer for Android or built directly from C# code the following two properties **must** be set before the `ActionTray` is displayed:

```
// These values MUST be set in code before the view is displayed
tray.trayType = UIActionTrayType.Popup;
tray.orientation = UIActionTrayOrientation.Right;
```

Failure to set the above lines before display can result in an `ActionTray` that is drawn and/or behaves incorrectly.

If the tray is being created completely in C# code, set the above lines **after** you have set the tray's _Frame_ size and added the tray to the parent _View_ so that it can correctly calculate its open and closed positions.

###Working with ActionTrays in Android###

`ActionTray` was designed to make adding it to a project super easy. Start an Android project in Xamarin Studio, switch to the **Android Designer** and add a **RelativeLayout** to be the parent of the `ActionTray`. Add one or more _Views_, switch to the **Source** view and change their type to _Appracatappra.ActionComponents.ActionTray.UIActionTray_.

_Note: The `ActionTray` **MUST** be hosted inside a **RelativeView** or it will not work correctly! The `ActionTray` itself is a type of **RelativeLayout** so add any UI Components to the tray and position them within it using **RelativeLayout** metrics._

###Configuring an ActionTray###

Aside from the base, minimal setup above, there are several features that you can use to customize not only the look but the feel of your action trays. Here is an example in iOS for an `ActionTray` added to a .xib file:

```
// Set tray type
leftTray.orientation = UIActionTrayOrientation.Left;
leftTray.tabLocation=UIActionTrayTabLocation.BottomOrRight;
leftTray.frameType=UIActionTrayFrameType.EdgeOnly;
leftTray.tabType=UIActionTrayTabType.IconAndTitle;

// Style tray
leftTray.appearance.background=UIColor.LightGray;
leftTray.appearance.frame=UIColor.DarkGray;
leftTray.icon=UIActionImage.FromFile ("Images/icon_calendar.png");
leftTray.title="Events";
leftTray.CloseTray (false);
```

And the same code for Android for an `ActionTray` created in the designer:

```
// Gain Access to all views and controls in our layout
UIActionTray leftTray = FindViewById<UIActionTray> (Resource.Id.trayLeft);
...

// Setup the left side tray
leftTray.trayType = UIActionTrayType.Draggable;
leftTray.orientation = UIActionTrayOrientation.Left;
leftTray.tabLocation = UIActionTrayTabLocation.BottomOrRight;
leftTray.frameType = UIActionTrayFrameType.EdgeOnly;
leftTray.tabType = UIActionTrayTabType.IconAndTitle;

// Style tray
leftTray.appearance.background = Color.Gray;
leftTray.appearance.border = Color.Red;
leftTray.icon = Resource.Drawable.icon_calendar;
leftTray.title = "Events";
leftTray.appearance.tabAlpha=100;
leftTray.CloseTray (false);
```

###Responding to User Interaction###

`ActionTrays` define several events that can be responded to such as _Touched_, _Moved_, _Released_, _Opened_, _Closed_ or _CustomDrawDragTab_. The following is an example of handling an `ActionTray` being opened on either iOS or Android:

```
// Respond to the tray being opened
rightTray.Opened+= (tray) => {
	// Tell any open palette trays to close
	trayManager.CloseAllTrays ();
};
```

###Working with Groups of ActionTrays###

`ActionTray` provides a **UIActionTrayManager** to make working with groups of `ActionTrays` easier. The **UIActionTrayManager** ensures that only one tray in the group is open at any given time and provides events for handling the trays it controls as a group. The following is an example of building a **UIActionTrayManager** and responding to any tray being opened in the group for both iOS and Android:

```
// Create a TrayManager to handle a collection of "palette"
// trays. It will automatically close any open tray when 
// another tray in this collection is opened.
trayManager = new UIActionTrayManager ();

// Automatically close the left tray when any tray
// in the manager's collection is opened
trayManager.TrayOpened += (tray) => {
	// Animate the tray being closed
	leftTray.CloseTray (true);
};
```

Configure your `ActionTrays` as normal and use the following code to add them to the Tray Managers collection. The tray will be automatically close by the manager when added:

```
// Add this tray to the manager's collection
trayManager.AddTray (paletteTray);
```

###Custom Drawing the DragTab###

`ActionTrays` provide several types of built in _dragTabs_ and many properties for controlling their appearance. Sometimes, however, that isn't enough. That's why `ActionTrays` allow you to custom draw the _dragTab_ by hand. Here is an example in iOS:

```
// Set tray type
toolsTray.trayType=UIActionTrayType.AutoClosingPopup;
toolsTray.tabType=UIActionTrayTabType.IconOnly;
toolsTray.tabType=UIActionTrayTabType.CustomDrawn;
toolsTray.CloseTray (false);
		
// Style tray
toolsTray.tabWidth=50f;
toolsTray.appearance.background=UIColor.FromRGB (38,38,38);

// Custom draw the tray's drag tab
toolsTray.CustomDrawDragTab+= (tray, rect) => {
	// Mix background color
	UIColor tabColor;
			
	if (tray.frameType==UIActionTrayFrameType.None) {
		tabColor=tray.appearance.background.ColorWithAlpha (tray.appearance.tabAlpha);
	} else {
		tabColor=tray.appearance.frame.ColorWithAlpha (tray.appearance.tabAlpha);
	}

	// Save current context
	var context = UIGraphics.GetCurrentContext();

	// Draw tab in the given bounds
	var bodyPath = UIBezierPath.FromRect(rect);
	tabColor.SetFill();
	bodyPath.Fill();

	// Draw icon
	var icon=UIImage.FromFile ("Images/icon_pencil.png");
	var y=rect.GetMinY()+5f;
	var tabIconRect = new RectangleF(rect.GetMinX() + 1, y, 30, 30);
	var tabIconPath = UIBezierPath.FromRect(tabIconRect);
	context.SaveState();
	tabIconPath.AddClip();
	icon.Draw(new RectangleF((float)Math.Floor(tabIconRect.GetMinX() + 1f), (float)Math.Floor(y + 0.5f), icon.Size.Width, icon.Size.Height),CGBlendMode.Normal,tray.appearance.tabAlpha);
	context.RestoreState();
};
```

Here is the same example in Android:

```
// Setup tools tray type
toolsTray.trayType = UIActionTrayType.AutoClosingPopup;
toolsTray.orientation = UIActionTrayOrientation.Top;
toolsTray.tabType = UIActionTrayTabType.IconOnly;
toolsTray.CloseTray (false);
		
// Style tools tray
toolsTray.tabWidth = 50;
toolsTray.tabLocation = UIActionTrayTabLocation.BottomOrRight;
toolsTray.appearance.background = Color.Rgb (38,38,38);
toolsTray.tabType = UIActionTrayTabType.CustomDrawn;
toolsTray.icon = Resource.Drawable.icon_pencil;

// Custom draw this tab
toolsTray.CustomDrawDragTab += (tray, canvas, rect) => {
	//Draw background
	var body= new ShapeDrawable(new RectShape());
	body.Paint.Color=tray.appearance.background;
	body.SetBounds (rect.Left, rect.Top, rect.Right, rect.Bottom);
	body.Draw (canvas);

	//Define icon paint
	var iPaint=new Paint();
	iPaint.Alpha=tray.appearance.tabAlpha;

	//Load bitmap
	var bitmap=BitmapFactory.DecodeResource(Resources,tray.icon);
			
	//Draw image
	canvas.DrawBitmap (bitmap, rect.Left+1, rect.Top+5, iPaint);
};
```

###Maintain State on Android###

Since views are destroyed and recreated on Android devices for events such as screen rotation, `ActionTrays` provide methods for saving and restoring their state. Here is an example:

```
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
```

##Examples##

For full examples of using `ActionTrays` in your mobile application please see the _APTest.iOS_ example app included with this component. `ActionTrays` are used throughout the app.

The _APTest.iOS_ app not only includes this documentation but it also includes the full API documentation for the entire `ActionPack` component suite.

See the API documentation for `ActionTray` for a complete list of features and their usage.


##Trial Version##

The Trial version of `ActionTray` is fully functional however the background is watermarked. The fully licensed version removes this watermark.

## Other Resources

* [Action Components](http://appracatappra.com/actioncomponents)
* [Android API Documentation](http://appracatappra.com/api/android/)
* [iOS API Documentation](http://appracatappra.com/api/ios/)
* [Appracatappra Support Forum](http://appracatappra.com/forum/index.php)
* [Support](http://appracatappra.com/apis/component-support)