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

            menuPageMapButton.Layer.CornerRadius = 35;
            menuPageMapButton.Layer.BorderWidth = 2;
            menuPageMapButton.Layer.BorderColor = UIColor.White.CGColor;

            aboutUsButton.Layer.CornerRadius = 35;
            aboutUsButton.Layer.BorderWidth = 2;
            aboutUsButton.Layer.BorderColor = UIColor.White.CGColor;

            userGuideButton.Layer.CornerRadius = 35;
            userGuideButton.Layer.BorderWidth = 2;
            userGuideButton.Layer.BorderColor = UIColor.White.CGColor;

            menuPageMapButton.TouchUpInside += delegate(object sender, EventArgs e) {
                ViewController nextView = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                this.NavigationController.PushViewController(nextView, true);
            };


        }
	}
}
