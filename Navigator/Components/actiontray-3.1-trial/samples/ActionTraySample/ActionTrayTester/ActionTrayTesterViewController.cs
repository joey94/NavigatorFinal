using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Appracatappra.ActionComponents.ActionTray;
using MonoTouch.CoreGraphics;

namespace ActionTrayTester
{
	public partial class ActionTrayTesterViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		//Public variables
		public UIActionTrayManager trayManager;

		public ActionTrayTesterViewController ()
			: base (UserInterfaceIdiomIsPhone ? "ActionTrayTesterViewController_iPhone" : "ActionTrayTesterViewController_iPad", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Create a TrayManager to handle a collection of "palette"
			// trays. It will automatically close any open tray when 
			// another tray in this collection is opened.
			trayManager = new UIActionTrayManager ();

			// Automatically close the left tray when any tray
			// in the manager's collection is opened
			trayManager.TrayOpened += (tray) => {
				// Animate the tray being closed
				leftTray.CloseTray (true);

				// Are we on an iPhone?
				if (UserInterfaceIdiomIsPhone) {
					// Yes, close the right tray too
					rightTray.CloseTray (true);
				}
			};
			
			// Wireup the left side tray created in the .xib file and style
			// it to be a drag out tray.
			if (leftTray != null) {
				// Set tray type
				leftTray.orientation = UIActionTrayOrientation.Left;
				leftTray.tabLocation=UIActionTrayTabLocation.BottomOrRight;
				leftTray.frameType=UIActionTrayFrameType.EdgeOnly;
				leftTray.tabType=UIActionTrayTabType.IconAndTitle;

				// Style tray
				leftTray.appearance.background=UIColor.DarkGray;
				leftTray.icon=UIImage.FromFile ("Images/icon_calendar.png");
				leftTray.title="Events";
				leftTray.appearance.tabAlpha=0.8f;
				leftTray.CloseTray (false);

				// Respond to the tray being touched
				leftTray.Touched+= (tray) => {
					// Are we on an iPhone?
					if (UserInterfaceIdiomIsPhone) {
						//Yes, close this tray and aminate the closing
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

				// Respond to the tray being opened
				rightTray.Opened+= (tray) => {
					//Are we on an iPhone?
					if (UserInterfaceIdiomIsPhone) {
						//Yes, close this tray and aminate the closing
						leftTray.CloseTray (true);
					}

					// Tell any open palette trays to close
					trayManager.CloseAllTrays ();

					// Close document tray
					if (documentTray!=null) 
						documentTray.CloseTray (true);
				};
			}

			// Wireup the document tray created in the .xib file and style it
			// to be an auto closing popup. When the user selects something
			// from it's content area, it is automatically closed.
			if (documentTray != null) {
				// Set tray type
				documentTray.trayType=UIActionTrayType.AutoClosingPopup;
				documentTray.orientation = UIActionTrayOrientation.Bottom;
				documentTray.tabType=UIActionTrayTabType.GripAndTitle;

				// Style tray
				documentTray.tabWidth=125f;
				documentTray.title="Documents";
				documentTray.CloseTray (false);

				// Respond to the tray being opened
				documentTray.Opened += (tray) => {
					// Close left and right trays
					leftTray.CloseTray(true);
					rightTray.CloseTray(true);
				};
			}

			// Palette 1
			// Wireup the 1st palette from the .xib file and style it
			// to be an auto closing popup. When the user selects something
			// from it's content area, it is automatically closed.
			if (paletteTray != null) {
				// Are we on an iPhone?
				if (UserInterfaceIdiomIsPhone) {
					// Move the subview into view and attach it to the master view
					paletteTray.MoveTo (new PointF(0,0));
					View.AddSubview(paletteTray);

					// Adjust tab location
					paletteTray.tabLocation=UIActionTrayTabLocation.Custom;
					paletteTray.tabOffset=55f;

					//iPhone specific settings
					paletteTray.orientation=UIActionTrayOrientation.Right;
				} else {
					// iPad specific settings
					paletteTray.tabLocation=UIActionTrayTabLocation.TopOrLeft;
					paletteTray.orientation=UIActionTrayOrientation.Top;
				}

				// Set tray type
				paletteTray.trayType=UIActionTrayType.AutoClosingPopup;
				paletteTray.tabType=UIActionTrayTabType.IconAndTitle;
				paletteTray.CloseTray (false);

				// Style tray
				paletteTray.tabWidth=125f;
				paletteTray.icon=UIImage.FromFile ("Images/icon_palette.png");
				paletteTray.title="Palette";

				// Add this tray to the manager's collection
				trayManager.AddTray (paletteTray);
			}

			// Palette 2
			// Wireup the 2nd palette from the .xib file and style it
			// to be a popup tray. Touch it's dragTab to open and close it.
			if (propertyTray != null) {
				// Are we on an iPhone?
				if (UserInterfaceIdiomIsPhone) {
					// Move subview into view and attach it to the master view
					propertyTray.MoveTo(new PointF(0,170));
					View.AddSubview(propertyTray);

					// iPhone specific settings
					propertyTray.orientation=UIActionTrayOrientation.Right;
				} else {
					// iPad specific settings
					propertyTray.orientation=UIActionTrayOrientation.Top;
				}

				// Set tray type
				propertyTray.trayType=UIActionTrayType.Popup;
				propertyTray.tabLocation=UIActionTrayTabLocation.TopOrLeft;
				propertyTray.tabType=UIActionTrayTabType.IconAndTitle;
				propertyTray.CloseTray (false);
				
				// Style tray
				propertyTray.tabWidth=125f;
				propertyTray.icon=UIImage.FromFile ("Images/icon_measures.png");
				propertyTray.title="Properties";

				// Add this tray to the manager's collection
				trayManager.AddTray (propertyTray);
			}

			// Palette 3
			// Wireup the 3rd palette from the .xib file and style it 
			// to be an auto closing popup. When the user selects something
			// from it's content area, it is automatically closed.
			if (toolsTray != null) {
				// Are we on an iPhone?
				if (UserInterfaceIdiomIsPhone) {
					// Move the subview into view and attach it to the master view
					toolsTray.MoveTo (new PointF(0,0));
					View.AddSubview(toolsTray);

					// Adjust tab location
					toolsTray.tabLocation=UIActionTrayTabLocation.Custom;
					toolsTray.tabOffset=5f;

					// iPhone specific settings
					toolsTray.orientation=UIActionTrayOrientation.Right;
				} else {
					// iPad specific settings
					toolsTray.orientation=UIActionTrayOrientation.Top;
				}

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

				// Add this tray to the manager's collection
				trayManager.AddTray (toolsTray);
			}

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
	}
}

