using System;
using CoreGraphics;
using UIKit;
using Navigator.Helpers;
using System.Collections.Generic;

namespace Navigator.iOS
{
    public class PathView : UIView
    {
        private nfloat _scaleFactor;
        private  CGPath path;
        private CGPoint[] pointsList;
        private bool pathSet = false;
        private WallCollision wallCol;
        private ViewController mainView;
        public int FLOOR;

        public PathView(WallCollision wc, ViewController v)
        {
            BackgroundColor = UIColor.Clear;

            path = new CGPath();
            wallCol = wc;
            mainView = v;

        }

        public nfloat ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                SetNeedsDisplay();
            }
        }

        public CGPoint getLatestPoint()
        {
            return path.CurrentPoint;
        }
         
        public void clear(){
            path = new CGPath ();
            pathSet = false;
        }
        public void setPoints(CGPoint[] points)
        {
            pathSet = true;
			path.AddLines(points);
            pointsList = points;
            //SetNeedsDisplay();
        }

        /*
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);



            //get graphics context
            using (var g = UIGraphics.GetCurrentContext())
            {
                //set up drawing attributes
                g.SetLineWidth(3/_scaleFactor);
                UIColor.Cyan.SetStroke ();
                g.SetShadow (new CGSize (1, 1), 10, UIColor.Blue.CGColor);
                //use a dashed line
                //g.SetLineDash(0, new[] {5, 2/_scaleFactor});

                //add geometry to graphics context and draw it
                g.AddPath(path);
                g.DrawPath(CGPathDrawingMode.Stroke);
            }
        }
        */

        public override void Draw (CGRect rect)
        {
            if (pathSet == true) {
                base.Draw (rect);

                using (var context = UIGraphics.GetCurrentContext ()) {
                
                    //set up drawing attributes
                    context.SetLineWidth (3 / _scaleFactor);
                    UIColor.Cyan.SetStroke ();
                    context.SetShadow (new CGSize (1, 1), 10, UIColor.Blue.CGColor);

                    var lineStart = pointsList[0];
                    var lineEnd = pointsList[0];

                    var line = new CGPoint[2];


                    foreach(var pathPoint in pointsList){
                        // If we can make a non obstructed path from our start to end , just continue
                        int originX = (int)lineStart.X;
                        int originY = (int)lineStart.Y;
                        int targetX = (int)pathPoint.X;
                        int targetY = (int)pathPoint.Y;
                        if(wallCol.IsValidStep(originX,originY,targetX,targetY))
                        {
                            // Our step is valid
                            lineEnd = pathPoint;

                        }else{
                            // We cannot perform this step, revert to last one
                            line[0] = lineStart;
                            line[1] = lineEnd;
                            context.AddLines(line);
                            context.StrokePath();
                            lineStart = lineEnd;
                        }
                    }

                    line[0] = lineStart;
                    line[1] = lineEnd;
                    context.AddLines(line);
                    context.StrokePath();
                    //directionPoints.Add (line[1]);

                    //mainView.pushDirectionsPointsList (directionPoints);
                    //context.AddPath(path);
                    //context.DrawPath(CGPathDrawingMode.Stroke);


              


                    /*
                    // Draw a quad curve with end points s,e and control point cp1
                    context.SetStrokeColor (1, 1, 1, 1);
                    s = new CGPoint (30, 300);
                    e = new CGPoint (270, 300);
                    cp1 = new CGPoint (150, 180);
                    context.MoveTo (s.X, s.Y);
                    context.AddQuadCurveToPoint (cp1.X, cp1.Y, e.X, e.Y);
                    context.AddQuadCurveToPoint (cp1.X + 100, cp1.Y + 100, e.X + 100, e.Y + 100);

                    context.StrokePath ();
                    */

                }
            }
        }
    }
}