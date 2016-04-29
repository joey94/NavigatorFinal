using System;
using UIKit;

namespace Navigator.iOS
{
    partial class menuController : UIViewController
    {
        private UIImage backImg;

        public menuController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            backImg = UIImage.FromBundle("Images/menuBackground.png");
            backImg = backImg.ApplyLightEffect();
            menuBackgroundImage.Image = backImg;
            menuBackgroundImage.SizeToFit();

            menuPageMapButton.TouchUpInside += delegate
            {
                var nextView = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                this.NavigationController.PushViewController(nextView, true);
            };
        }
    }
}