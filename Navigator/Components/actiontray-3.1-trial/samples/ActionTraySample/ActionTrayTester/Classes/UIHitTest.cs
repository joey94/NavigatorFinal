using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace ActionTrayTester
{
	[Register("UIHitTest")]
	public class UIHitTest : UIView
	{
		public UIHitTest() : base() {
			
		}
		
		public UIHitTest(NSCoder coder): base(coder){
			
		}
		
		public UIHitTest(NSObjectFlag flag): base(flag){
			
		}
		
		public UIHitTest(RectangleF bounds): base(bounds){
			
		}
		
		public UIHitTest(IntPtr ptr): base(ptr){
			
		}

		public override UIView HitTest (PointF point, UIEvent uievent)
		{
			UIView view,wasHit;
			PointF pt;

			//Itterate through views backwards until you find the one
			//to send the event to
			for (int n=Subviews.Length-1; n>=0; --n) {
				view=Subviews[n];

				pt=this.ConvertPointToView (point,view);
				wasHit=view.HitTest (pt,uievent);
				if (wasHit!=null) {
					return wasHit;
				}
			}

			//Return to default behavior
			return base.HitTest (point, uievent);
		}

	}
}

