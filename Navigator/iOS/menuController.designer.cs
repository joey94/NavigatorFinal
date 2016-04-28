// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Navigator.iOS
{
	[Register ("menuController")]
	partial class menuController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton aboutUsButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView menuBackgroundImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton menuPageMapButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView menuTitle { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton userGuideButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (aboutUsButton != null) {
				aboutUsButton.Dispose ();
				aboutUsButton = null;
			}
			if (menuBackgroundImage != null) {
				menuBackgroundImage.Dispose ();
				menuBackgroundImage = null;
			}
			if (menuPageMapButton != null) {
				menuPageMapButton.Dispose ();
				menuPageMapButton = null;
			}
			if (menuTitle != null) {
				menuTitle.Dispose ();
				menuTitle = null;
			}
			if (userGuideButton != null) {
				userGuideButton.Dispose ();
				userGuideButton = null;
			}
		}
	}
}
