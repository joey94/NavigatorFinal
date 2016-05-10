using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Navigator.iOS
{
	partial class userGuidePage : UIViewController
	{
        UIImage backImg;

        public userGuidePage (IntPtr handle) : base (handle)
		{
		}
            
        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            backImg = UIImage.FromBundle("Images/menuBackground.png");
            backImg = UIImageEffects.ApplyLightEffect (backImg);
            userGuideBackground.Image = backImg;
            userGuideBackground.SizeToFit ();

            userGuideReturnToMenu.Layer.CornerRadius = 25;
            userGuideReturnToMenu.Layer.BorderWidth = 2;
            userGuideReturnToMenu.Layer.BorderColor = UIColor.White.CGColor;


            userGuideTitleLabel.Layer.BorderWidth = 2;
            userGuideTitleLabel.Layer.BorderColor = UIColor.White.CGColor;

        


            userGuideReturnToMenu.TouchUpInside += delegate { 
                NavigationController.PopViewController (true);
            };




        }
	}
}
