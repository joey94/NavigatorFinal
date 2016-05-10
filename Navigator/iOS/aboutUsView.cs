using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Navigator.iOS
{
	partial class aboutUsView : UIViewController
	{
        UIImage backImg;

		public aboutUsView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            backImg = UIImage.FromBundle("Images/menuBackground.png");
            backImg = UIImageEffects.ApplyLightEffect (backImg);
            aboutUsBackground.Image = backImg;
            aboutUsBackground.SizeToFit ();

            aboutUsReturnToMenu.Layer.CornerRadius = 25;
            aboutUsReturnToMenu.Layer.BorderWidth = 2;
            aboutUsReturnToMenu.Layer.BorderColor = UIColor.White.CGColor;


            aboutUsTitle.Layer.BorderWidth = 2;
            aboutUsTitle.Layer.BorderColor = UIColor.White.CGColor;




            aboutUsReturnToMenu.TouchUpInside += delegate { 
                NavigationController.PopViewController (true);
            };




        }

	}
}
