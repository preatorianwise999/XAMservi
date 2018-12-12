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
    [Register ("MenuController")]
    partial class MenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView MenuIniciaSesion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView MenuPagoExpress { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MenuIniciaSesion != null) {
                MenuIniciaSesion.Dispose ();
                MenuIniciaSesion = null;
            }

            if (MenuPagoExpress != null) {
                MenuPagoExpress.Dispose ();
                MenuPagoExpress = null;
            }
        }
    }
}