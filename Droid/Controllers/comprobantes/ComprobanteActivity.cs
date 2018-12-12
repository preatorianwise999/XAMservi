using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServipagMobile.Droid {
	[Activity(Label = "ComprobanteActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ComprobanteActivity : AppCompatActivity {
		private Android.Support.V7.Widget.Toolbar toolbar;
		private Button closeComprobante;
		private RelativeLayout carroCompraLayout, downloadVoucherLayout;
		public ImageButton carroCompra, downloadVoucher;
		private TextView badgeCarro;
		public List<MisCuentas> listAdd = new List<MisCuentas>();
		private bool isPagoExpress;
		private string status;
		private string badgeCount;
		private string mensaje;
		private string idPago;
		private VoucherData voucherData;
		private string filePath;

		private int cantPasesVendidos;
		private PaseDiario pd;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Comprobante);

			isPagoExpress = Intent.GetBooleanExtra("isPagoExpress", false);
			badgeCount = Intent.GetStringExtra("badgeCount");
			status = Intent.GetStringExtra("status");
			mensaje = Intent.GetStringExtra("mensaje");
			idPago = Intent.GetStringExtra("idPago");

			cantPasesVendidos = Intent.GetIntExtra("cantPasesVendidos", 1);
			if (Intent.GetStringExtra("paseDiario") != null) {
				pd = JsonConvert.DeserializeObject<PaseDiario>(Intent.GetStringExtra("paseDiario"));
			}

			setValuesAgregarComprobante();
		}

		public override void OnBackPressed() {
			//base.OnBackPressed();
		}

		public void setValuesAgregarComprobante() {
			toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarClose);
			closeComprobante = FindViewById<Button>(Resource.Id.buttonClose);
			carroCompraLayout = FindViewById<RelativeLayout>(Resource.Id.carroCompraLayout);
			carroCompra = FindViewById<ImageButton>(Resource.Id.carroCompra);
			badgeCarro = FindViewById<TextView>(Resource.Id.badgeText);
			downloadVoucherLayout = FindViewById<RelativeLayout>(Resource.Id.downloadVoucherLayout);
			downloadVoucher = FindViewById<ImageButton>(Resource.Id.downloadVoucher);

			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayShowHomeEnabled(false);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			setFragmentComprobante();

			if (isPagoExpress) {
				badgeCarro.Text = badgeCount;
				carroCompraLayout.Visibility = Android.Views.ViewStates.Visible;
				carroCompra.Click += (sender, e) => {
					Intent intent = new Intent();
					intent.PutExtra("actionComprobanteAgregar", "mCarroCompra");
					SetResult(Result.Ok, intent);
					this.Finish();
				};
			}
		}

		private void setFragmentComprobante() {
			switch(status) {
				case "agregarSuccess":
					SupportActionBar.SetDisplayHomeAsUpEnabled(false);
					changeMainFragment(new FragmentComprobanteAgregar(isPagoExpress, true));
					closeComprobante.Click += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("actionComprobanteAgregar", "cerrarAgregar");
						SetResult(Result.Ok, intent);
						this.Finish();
					};
				break;
				case "agregarFail":
					closeComprobante.Visibility = Android.Views.ViewStates.Gone;
					SupportActionBar.SetDisplayHomeAsUpEnabled(true);
					changeMainFragment(new FragmentFallidoAgregar(mensaje, true));
				break;
				case "editarSuccess":
					SupportActionBar.SetDisplayHomeAsUpEnabled(false);
					changeMainFragment(new FragmentComprobanteAgregar(isPagoExpress, false));
					closeComprobante.Click += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("actionComprobanteEditar", "cerrarEditar");
						SetResult(Result.Ok, intent);
						this.Finish();
					};
				break;
				case "editarFail":
					closeComprobante.Visibility = Android.Views.ViewStates.Gone;
					SupportActionBar.SetDisplayHomeAsUpEnabled(true);
					changeMainFragment(new FragmentFallidoAgregar(mensaje, false));
				break;
				case "cClaveSuccess":
					SupportActionBar.SetDisplayHomeAsUpEnabled(false);
					changeMainFragment(new FragmentComprobanteCClave());
					closeComprobante.Click += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("valueCompleted", "success");
						SetResult(Result.Ok, intent);
						this.Finish();
					};
				break;
				case "cClaveFail":
					closeComprobante.Visibility = Android.Views.ViewStates.Gone;
					SupportActionBar.SetDisplayHomeAsUpEnabled(true);
					changeMainFragment(new FragmentFallidoCClave(mensaje));
				break;
				case "ocSuccess":
					SupportActionBar.SetDisplayHomeAsUpEnabled(false);
					changeMainFragment(new FragmentComprobanteOC(mensaje));
					closeComprobante.Click += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("type", "close");
						SetResult(Result.Ok, intent);
						Finish();
					};
				break;
				case "ocFail":
					closeComprobante.Visibility = Android.Views.ViewStates.Gone;
					SupportActionBar.SetDisplayHomeAsUpEnabled(true);
					changeMainFragment(new FragmentFallidoOC(mensaje));
				break;
				case "pagoCompletado":
					SupportActionBar.SetDisplayHomeAsUpEnabled(false);
					/*downloadVoucherLayout.Visibility = Android.Views.ViewStates.Visible;

					downloadVoucher.Click += (sender, e) => {
						downloadPdf();
					};*/
					getEstadoPago();
				break;
			}
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item) {
			if (item.ItemId == Android.Resource.Id.Home) {
				switch (status) {
					case "agregarFail":
						Intent intent = new Intent();
						intent.PutExtra("actionComprobanteAgregar", "noAction");
						SetResult(Result.Ok, intent);
						Finish();
					break;
					case "editarFail":
						Intent inte = new Intent();
						inte.PutExtra("actionComprobanteEditar", "noAction");
						SetResult(Result.Ok, inte);
						Finish();
					break;
					case "cClaveFail":
						Finish();
					break;
					case "ocFail":
						Intent i = new Intent();
						i.PutExtra("type", "back");
						SetResult(Result.Ok, i);
						Finish();
					break;
				}

			}

			return base.OnOptionsItemSelected(item);
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment) {
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.mainFragmentC, fragment);
			ft.Commit();
		}

		public async void getEstadoPago() {
			JObject parametros = new JObject();
			AndHUD.Shared.Show(this, null, -1, MaskType.Black);

			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			parametros.Add("tx", idPago);
			parametros.Add("firma", "");

			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("estadoPago", "estado_pago", "POST", parametros);

			if (response.Success) {
				Log.Debug("Error", response.State["Error"].ToString());
				Log.Debug("if", (response.State["Error"].ToString().Equals("0")).ToString());
				if (response.State["Error"].ToString().Equals("0")) {
					if (response.Response["retorno"].ToString().Equals("0")) {
						getObtenerComprobante();
					} else {
						Log.Debug("Success", "Aún no existe confirmación de pago.");
						AndHUD.Shared.Dismiss(this);
						changeMainFragment(new FragmentNoVoucher(false, idPago));
						closeComprobante.Click += (sender, e) => {
							this.Finish();
						};
					}
				} else {
					Log.Debug("Error", "Servicio no se pudo ejecutar");
					AndHUD.Shared.Dismiss(this);
					changeMainFragment(new FragmentNoVoucher(true, idPago));
					closeComprobante.Click += (sender, e) => {
						this.Finish();
					};
				}
			} else {
				Log.Debug("Error", "Error de servicio");
				AndHUD.Shared.Dismiss(this);
				changeMainFragment(new FragmentNoVoucher(true, idPago));
				closeComprobante.Click += (sender, e) => {
					this.Finish();
				};
			}

		}

		public async void getObtenerComprobante() {
			JObject parametros = new JObject();

			parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
			parametros.Add("canal", DeviceInformation.GetInstance().channel);
			parametros.Add("idPago", idPago);
			parametros.Add("firma", "");

			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("obtieneComprobante", "obtiene_comprobante", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					getVoucherData(response.Response);
					changeMainFragment(new FragmentVoucher(voucherData));
					closeComprobante.Click += (sender, e) => {
						Intent intent = new Intent();
						intent.PutExtra("recargas", "closeRecargas");
						SetResult(Result.Ok, intent);
						this.Finish();
					};
				} else {
					Log.Debug("Error", "Información no disponible");
					changeMainFragment(new FragmentNoVoucher());
					closeComprobante.Click += (sender, e) => {
						this.Finish();
					};
				}
			} else {
				Log.Debug("Error", "Error de servicio");
				changeMainFragment(new FragmentNoVoucher());
				closeComprobante.Click += (sender, e) => {
					this.Finish();
				};
			}
			AndHUD.Shared.Dismiss(this);
		}

		private void getVoucherData(JObject response) {
			var vData = response["ObtieneComprobante"];
			var lDVoucher = vData["detalle_comprobante"];
			VoucherInfo vi = new VoucherInfo();
			voucherData = new VoucherData();
			voucherData.voucherDetail = new List<VoucherInfo>();

			for (int i = 0; i < lDVoucher.Count(); i++) {
				vi.authCode = lDVoucher[i]["cod_autorizacion"].ToString();
				vi.company = lDVoucher[i]["empresa"].ToString();
				vi.idAccount = lDVoucher[i]["identificador"].ToString();
				vi.amount = lDVoucher[i]["monto"].ToString();
				vi.alias = lDVoucher[i]["nombre"].ToString();

				voucherData.voucherDetail.Add(vi);
			}

			voucherData.date = vData["fecha"].ToString();
			voucherData.paymentType = vData["forma_de_pago"].ToString();
			voucherData.hour = vData["hora"].ToString();
			voucherData.amount = vData["monto_total"].ToString();
			voucherData.nRequest = vData["n_consulta"].ToString();
			voucherData.clientName = vData["usuario"].ToString();

		}

		private void downloadPdf() {
			View view = Window.DecorView;
			view.DrawingCacheEnabled = true;
			view.BuildDrawingCache();

			Bitmap bitmap = view.DrawingCache;
			Rect frame = new Rect();
			Window.DecorView.GetWindowVisibleDisplayFrame(frame);

			int statusBarHeight = frame.Top;
			int width = WindowManager.DefaultDisplay.Width;
			int height = WindowManager.DefaultDisplay.Height;

			Bitmap screenshot = Bitmap.CreateBitmap(bitmap, 0, statusBarHeight, width, height - statusBarHeight);
			view.DestroyDrawingCache();
			Log.Debug("Screenshot", "Tomada de forma exitosa");

			saveBitmap(screenshot);
		}

		private void saveBitmap(Bitmap bitmap) {
			var sdCardPath = Environment.ExternalStorageDirectory.AbsolutePath;
			filePath = System.IO.Path.Combine(sdCardPath, "test.png");
			var stream = new FileStream(filePath, FileMode.Create);
			bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
			stream.Close();

			CustomAlertDialog alert = new CustomAlertDialog(this,
			                                                "Descarga Boleta",
			                                                "Su boleta ha sido descargada exitosamente",
			                                                "Ver",
			                                                "Cerrar", showImage, null);
			alert.showDialog();
		}

		private void showImage() {
			Intent intent = new Intent();
			intent.SetAction(Intent.ActionView);
			intent.SetDataAndType(Uri.FromFile(new Java.IO.File(filePath)), "image/*");
			StartActivity(intent);
		}
	}
}
