using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Com.Bumptech.Glide;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	public class ServiciosRecargaAdapter : RecyclerView.Adapter {
		private List<ServiciosRecarga> listSR;
		private RecargasActivity ra;
		private int itemCount;
		private string urlImage;
		private bool isLogin;

		public ServiciosRecargaAdapter(List<ServiciosRecarga> listSR, RecargasActivity ra, bool isLogin) {
			this.listSR = listSR;
			this.ra = ra;
			this.isLogin = isLogin;
			this.urlImage = "https://www.servipag.com/PortalWS/Content/images";
			decimal d = (decimal)listSR.Count / 3;
			itemCount = (int)Math.Ceiling(d);
		}

		public override int ItemCount {
			get {
				return itemCount;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RowVistaMP, parent, false);
			ServiciosRecargaViewHolder vh = new ServiciosRecargaViewHolder(itemView, ra.Resources.DisplayMetrics.WidthPixels);
			return vh;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			ServiciosRecargaViewHolder vh = holder as ServiciosRecargaViewHolder;

			for (int i = position * 3; i < listSR.Count; i++) {
				if ((int)(i / 3) != position) {
					break;
				}

				int mod = i % 3;
				switch (mod) {
					case 0:
					IImageRecargas interfaceMPOne = new IImageRecargas(listSR[i], ((position * 3) + 0), vh.imgMPOne, vh.nameMPOne, hideName);
					Glide.With(ra)
					.Load(urlImage + "/" + listSR[i].logo)
					.Listener(interfaceMPOne)
					.Into(vh.imgMPOne);

					vh.imgMPOne.Click += (sender, e) => {
						openIngresaDatos(listSR[(position * 3) + 0]);
					};
					break;
					case 1:
					IImageRecargas interfaceMPTwo = new IImageRecargas(listSR[i], ((position * 3) + 1), vh.imgMPTwo, vh.nameMPTwo, hideName);
					Glide.With(ra)
					.Load(urlImage + "/" + listSR[i].logo)
					.Listener(interfaceMPTwo)
					 .Into(vh.imgMPTwo);

					vh.imgMPTwo.Click += (sender, e) => {
						openIngresaDatos(listSR[(position * 3) + 1]);
					};
					break;
					case 2:
					IImageRecargas interfaceMPThree = new IImageRecargas(listSR[i], ((position * 3) + 2), vh.imgMPThree, vh.nameMPThree, hideName);
					Glide.With(ra)
					.Load(urlImage + "/" + listSR[i].logo)
					.Listener(interfaceMPThree)
					 .Into(vh.imgMPThree);

					vh.imgMPThree.Click += (sender, e) => {
						openIngresaDatos(listSR[(position * 3) + 2]);
					};
					break;
				}
			}
		}

		private void hideName(ServiciosRecarga servicioRecarga, TextView name, ImageView image, int position) {
			image.Visibility = ViewStates.Gone;
			name.Visibility = ViewStates.Visible;

			name.Text = servicioRecarga.nombre;

			name.Click += (sender, e) => {
				openIngresaDatos(servicioRecarga);
			};
		}

		public void openIngresaDatos(ServiciosRecarga servicioRecarga) {
			JObject parametros = new JObject();
			AndHUD.Shared.Show(ra, null, -1, MaskType.Black);

			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			parametros.Add("idServicio", servicioRecarga.id_servicio);
			parametros.Add("idBiller", servicioRecarga.id_biller);

			getMontoRecarga(parametros, servicioRecarga);
		}

		public async void getMontoRecarga(JObject parametros, ServiciosRecarga servicioRecarga) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("montoRecarga", "monto_recarga", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					ra.servicioRecarga = servicioRecarga;
					ra.listMontoRecarga = setListMontoRecarga(respuesta.Response);
					ra.changeMainFragment(new FragmentIngresaDatosRecarga(
						servicioRecarga,ra.listMontoRecarga , ra, isLogin),
					                      ra.Resources.GetString(Resource.String.recargas_title_id_recargas));
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(ra, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(ra);
		}

		private List<MontoRecarga> setListMontoRecarga(JObject response) {
			var listMontoRecarga = new List<MontoRecarga>();
			var listMR = response["ObtieneMontoRecarga"];

			for (var i = 0; i < listMR.Count(); i++) {
				var mRecarga = new MontoRecarga();

				mRecarga.tipo_tramo = listMR[i]["tipo_tramo"].ToString();
				mRecarga.valor1 = (int)listMR[i]["valor1"];
				mRecarga.valor2 = (int)listMR[i]["valor2"];

				listMontoRecarga.Add(mRecarga);
			}

			return listMontoRecarga;
		}
	}
}
