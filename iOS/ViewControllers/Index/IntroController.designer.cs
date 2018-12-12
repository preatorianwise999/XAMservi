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

namespace ServipagMobile.iOS
{
    [Register ("IntroController")]
    partial class IntroController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonForgotPass { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonLogin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonPagoExpress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FieldPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FieldUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FrameInicioSesion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FramePagoExpress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        ServipagMobile.iOS.UISegmentControl SegmentControl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitlePagoExpress { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ButtonForgotPass != null) {
                ButtonForgotPass.Dispose ();
                ButtonForgotPass = null;
            }

            if (ButtonLogin != null) {
                ButtonLogin.Dispose ();
                ButtonLogin = null;
            }

            if (ButtonPagoExpress != null) {
                ButtonPagoExpress.Dispose ();
                ButtonPagoExpress = null;
            }

            if (ButtonSignUp != null) {
                ButtonSignUp.Dispose ();
                ButtonSignUp = null;
            }

            if (FieldPassword != null) {
                FieldPassword.Dispose ();
                FieldPassword = null;
            }

            if (FieldUser != null) {
                FieldUser.Dispose ();
                FieldUser = null;
            }

            if (FrameInicioSesion != null) {
                FrameInicioSesion.Dispose ();
                FrameInicioSesion = null;
            }

            if (FramePagoExpress != null) {
                FramePagoExpress.Dispose ();
                FramePagoExpress = null;
            }

            if (SegmentControl != null) {
                SegmentControl.Dispose ();
                SegmentControl = null;
            }

            if (TitlePagoExpress != null) {
                TitlePagoExpress.Dispose ();
                TitlePagoExpress = null;
            }
        }
    }
}