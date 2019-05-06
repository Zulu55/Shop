// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FourWays.Classic.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField AmountText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider GenerositySlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TipLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountText != null) {
                AmountText.Dispose ();
                AmountText = null;
            }

            if (GenerositySlider != null) {
                GenerositySlider.Dispose ();
                GenerositySlider = null;
            }

            if (TipLabel != null) {
                TipLabel.Dispose ();
                TipLabel = null;
            }
        }
    }
}