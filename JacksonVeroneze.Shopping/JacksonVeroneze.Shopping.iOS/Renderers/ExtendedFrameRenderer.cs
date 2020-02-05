using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Frame), typeof(JacksonVeroneze.Shopping.iOS.Renderers.ExtendedFrameRenderer))]

namespace JacksonVeroneze.Shopping.iOS.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e) {  
                    base.OnElementChanged(e);  
                    Layer.BorderColor = UIColor.White.CGColor;  
                    Layer.CornerRadius = 5;  
                    Layer.MasksToBounds = false;  
                    Layer.ShadowOffset = new CGSize(-1, 2);  
                    Layer.ShadowRadius = 2;  
                    Layer.ShadowOpacity = 0.3f;  
         }
    }
}