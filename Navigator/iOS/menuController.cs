using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Navigator.iOS
{
	partial class menuController : UIViewController
	{
		public menuController (IntPtr handle) : base (handle)
		{
                   

		}

        UIImage backImg;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            backImg = UIImage.FromBundle("Images/menuBackground.png");
            backImg = UIImageEffects.ApplyLightEffect (backImg);
            menuBackgroundImage.Image = backImg;
            menuBackgroundImage.SizeToFit ();

            menuPageMapButton.TouchUpInside += delegate(object sender, EventArgs e) {
                ViewController nextView = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                this.NavigationController.PushViewController(nextView, true);
            };


        }
	}
}
