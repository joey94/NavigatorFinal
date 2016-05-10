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
	[Register ("userGuidePage")]
	partial class userGuidePage
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labelBackground { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView userGuideAppTitle { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView userGuideBackground { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton userGuideReturnToMenu { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel userGuideTitleLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (labelBackground != null) {
				labelBackground.Dispose ();
				labelBackground = null;
			}
			if (userGuideAppTitle != null) {
				userGuideAppTitle.Dispose ();
				userGuideAppTitle = null;
			}
			if (userGuideBackground != null) {
				userGuideBackground.Dispose ();
				userGuideBackground = null;
			}
			if (userGuideReturnToMenu != null) {
				userGuideReturnToMenu.Dispose ();
				userGuideReturnToMenu = null;
			}
			if (userGuideTitleLabel != null) {
				userGuideTitleLabel.Dispose ();
				userGuideTitleLabel = null;
			}
		}
	}
}
