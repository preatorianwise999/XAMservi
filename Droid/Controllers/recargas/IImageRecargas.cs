using System;
using Android.Util;
using Android.Widget;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using Java.Lang;

namespace ServipagMobile.Droid {
	public class IImageRecargas : Java.Lang.Object, IRequestListener
	{
		private ServiciosRecarga sRecarga;
		private int position;
		private ImageView image;
		private TextView name;
		private Action<ServiciosRecarga, TextView, ImageView, int> callback;

		public IImageRecargas(ServiciosRecarga sRecarga, int position, ImageView image, TextView name, Action<ServiciosRecarga, TextView, ImageView, int> callback) {
			this.sRecarga = sRecarga;
			this.position = position;
			this.image = image;
			this.name = name;
			this.callback = callback;
		}

		public bool OnException(Java.Lang.Exception p0, Java.Lang.Object p1, ITarget p2, bool p3) {
			Log.Error("Glide Image", "Exception has occurrs on: " + sRecarga.nombre);
			Log.Error("Glide Image", "Exception: " + p0);

			callback(sRecarga, name, image, position);
			return false;
		}

		public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, bool p3, bool p4) {
			Log.Debug("Glide Image", "Image load success");
			return false;
		}
	}
}
