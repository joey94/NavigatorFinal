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
	[Register ("aboutUsView")]
	partial class aboutUsView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView aboutUsBackground { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton aboutUsReturnToMenu { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel aboutUsTitle { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (aboutUsBackground != null) {
				aboutUsBackground.Dispose ();
				aboutUsBackground = null;
			}
			if (aboutUsReturnToMenu != null) {
				aboutUsReturnToMenu.Dispose ();
				aboutUsReturnToMenu = null;
			}
			if (aboutUsTitle != null) {
				aboutUsTitle.Dispose ();
				aboutUsTitle = null;
			}
		}
	}
}
