using System;
using Android.Util;
using Android.Widget;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using Java.Lang;

namespace ServipagMobile.Droid {
	public class ImageInterface : Java.Lang.Object, IRequestListener {
		private MediosPago medioPago;
		private int position;
		private ImageView image;
		private TextView name;
		private Action<MediosPago, TextView, ImageView, int> callback;

		public ImageInterface(MediosPago medioPago, int position, ImageView image, TextView name, Action<MediosPago, TextView, ImageView, int>callback) {
			this.medioPago = medioPago;
			this.position = position;
			this.image = image;
			this.name = name;
			this.callback = callback;
		}

		public bool OnException(Java.Lang.Exception p0, Java.Lang.Object p1, ITarget p2, bool p3) {
			Log.Error(" Image", "Exception has occurrs on: " + medioPago.descripcion);
			Log.Error(" Image", "Exception: " + p0.ToString());

			callback(medioPago, name, image, position);
			return false;
		}

		public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, bool p3, bool p4) {
			Log.Debug(" Image", "Image load success");
			return false;
		}
	}
}
