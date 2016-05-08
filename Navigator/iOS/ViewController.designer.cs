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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton directionsButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView floorplanView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton OptionsButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton returnButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchBar SearchBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SearchPredictionTable { get; set; }

		[Action ("ReturnButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ReturnButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (directionsButton != null) {
				directionsButton.Dispose ();
				directionsButton = null;
			}
			if (floorplanView != null) {
				floorplanView.Dispose ();
				floorplanView = null;
			}
			if (OptionsButton != null) {
				OptionsButton.Dispose ();
				OptionsButton = null;
			}
			if (returnButton != null) {
				returnButton.Dispose ();
				returnButton = null;
			}
			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}
			if (SearchPredictionTable != null) {
				SearchPredictionTable.Dispose ();
				SearchPredictionTable = null;
			}
		}
	}
}
