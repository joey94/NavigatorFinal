// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ActionTrayTester
{
	[Register ("ActionTrayTesterViewController")]
	partial class ActionTrayTesterViewController
	{
		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray leftTray { get; set; }

		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray rightTray { get; set; }

		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray paletteTray { get; set; }

		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray propertyTray { get; set; }

		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray toolsTray { get; set; }

		[Outlet]
		Appracatappra.ActionComponents.ActionTray.UIActionTray documentTray { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (leftTray != null) {
				leftTray.Dispose ();
				leftTray = null;
			}

			if (rightTray != null) {
				rightTray.Dispose ();
				rightTray = null;
			}

			if (paletteTray != null) {
				paletteTray.Dispose ();
				paletteTray = null;
			}

			if (propertyTray != null) {
				propertyTray.Dispose ();
				propertyTray = null;
			}

			if (toolsTray != null) {
				toolsTray.Dispose ();
				toolsTray = null;
			}

			if (documentTray != null) {
				documentTray.Dispose ();
				documentTray = null;
			}
		}
	}
}
