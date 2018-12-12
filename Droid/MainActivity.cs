using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Widget;
using Android.Views;
using Android.Content.PM;
using Com.Bumptech.Glide;
using Android.Util;
using Realms;
using System.Linq;
using Newtonsoft.Json.Linq;
using AndroidHUD;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Content;

namespace ServipagMobile.Droid {
	[Activity(Label = "Servipag")]
	public class MainActivity : AppCompatActivity {
		public NavigationView navigationView;
		public FragmentListaCuentas childFragment;
		public int sortSelected;
		public int sortSelectedPE;
		public List<MisCuentas> listAdd = new List<MisCuentas>();
		public List<SolicitaRecarga> listURPE = new List<SolicitaRecarga>();
		public List<PaseDiario> listaUltimosPDU;
		public List<PaseTardio> listaUltimosPDT;
		public List<MisCuentas> listaCuentasInscritas = new List<MisCuentas>();
		public UtilsAndroid utilsAndroid = new UtilsAndroid();
		public Utils utils = new Utils();
		public bool isCarroCompra = false;
		public bool isLogin = false;
		public string idFragment = "login";
		public string tabSelected = "pe";
		private DrawerLayout drawerLayout;
		private TextView welcomeText;
		private ImageView userImage;
		public RelativeLayout carroCompraLayout;
		private ImageButton carroCompra;
		public TextView badgeCount;
		public ImageView sortMyAccounts;
		public ImageView addAccount;

		private Android.Support.V7.App.AlertDialog mSortDialog;
		private Android.Support.V7.App.AlertDialog mSortDialogPE;
		private ImageView upperAlphabetic;
		private ImageView downerAlphabetic;
		private ImageView upperAlphabeticPE;
		private ImageView downerAlphabeticPE;
		private ImageView upperAlias;
		private ImageView downerAlias;
		private TextView version;
		private string idBiller;
		private string idServicio;

		private ActionBarDrawerToggle drawerToggle;

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			sortSelectedPE = RealmDB.GetInstance().realm.All<PersistentData>().First().sortTypePE;
			sortSelected = RealmDB.GetInstance().realm.All<PersistentData>().First().sortType;

			setValuesMain();
			getAccountsPE();

			if (savedInstanceState == null) {
				changeMainFragment(new FragmentLogin(SupportFragmentManager, this), idFragment);	
			} else {
				idFragment = savedInstanceState.GetString("idFragment");
				isLogin = savedInstanceState.GetBoolean("isLogin");
				switch(idFragment) {
					case "login":
						changeMainFragment(new FragmentLogin(SupportFragmentManager, this), idFragment);
					break;
					case "home":
						changeMainFragment(new FragmentListaCuentas(isLogin, listaCuentasInscritas), idFragment);
					break;
				}
			}
		}

		protected override void OnResume() {
			base.OnResume();

			if (utilsAndroid.getDeviceType(this) == "Mobile") {
				this.RequestedOrientation = ScreenOrientation.Portrait;
			}
		}

		protected override void OnSaveInstanceState(Bundle outState) {
			base.OnSaveInstanceState(outState);
			outState.PutString("idFragment", idFragment);
			outState.PutBoolean("isLogin", isLogin);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			switch(requestCode) {
				case 0://Registro
					if (data.GetStringExtra("registro") == "complete") {
						JObject parametros = new JObject();
						AndHUD.Shared.Show(this, null, -1, MaskType.Black);

						parametros.Add("canal", DeviceInformation.GetInstance().channel);
						parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().idTransaccion);
						parametros.Add("firma", "asd");
						parametros.Add("idUsuario", utils.getUserId(data.GetStringExtra("user")));
						parametros.Add("password", data.GetStringExtra("pass"));

						loginUser(parametros, data);
					}
				break;
				case 1://Agregar
					switch(data.GetStringExtra("actionAgregar")) {
						case "noAction":
							listAdd = JsonConvert.DeserializeObject<List<MisCuentas>>(data.GetStringExtra("listAdd"));
							badgeCount.Text = listAdd.Count.ToString();
						break;
						case "noServices":
							CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", "Existen problemas para obtener la lista de servicios. Inténtelo más tarde.", "Aceptar", "", null, null);
							alert.showDialog();
						break;
						case "mCarroCompra":
							carroCompraLayout.Visibility = ViewStates.Gone;
							sortMyAccounts.Visibility = ViewStates.Visible;
							addAccount.Visibility = ViewStates.Visible;

							isCarroCompra = true;
							isLogin = false;
							listAdd = JsonConvert.DeserializeObject<List<MisCuentas>>(data.GetStringExtra("listAdd"));
							badgeCount.Text = listAdd.Count.ToString();

							childFragment = new FragmentListaCuentas(isLogin, listAdd);
							changeMainFragment(childFragment, "mCarroCompra");
						break;
						case "reloadList":
							JObject parametros = new JObject();
							AndHUD.Shared.Show(this, null, -1, MaskType.Black);

							Log.Debug("canal", DeviceInformation.GetInstance().channel);
							Log.Debug("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
							Log.Debug("idUsuario", UserData.GetInstance().rut);
							Log.Debug("firma", "");

							parametros.Add("canal", DeviceInformation.GetInstance().channel);
							parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
							parametros.Add("idUsuario", UserData.GetInstance().rut);
							parametros.Add("firma", "");

							cuentasInscritas(parametros, true);
						break;
						case "openLastPDU":
							listaUltimosPDU = new List<PaseDiario>();
							listaUltimosPDT = new List<PaseTardio>();
							idBiller = data.GetStringExtra("idBiller");
							idServicio = data.GetStringExtra("idServicio");

							foreach (PaseDiario pdp in RealmDB.GetInstance().realm.All<PaseDiario>()) {
								var pd = new PaseDiario();
								pd.nombre_fantasia = pdp.nombre_fantasia;
								pd.fecha_vencimiento = pdp.fecha_vencimiento;
								pd.identificador = pdp.identificador;
								pd.monto_total = pdp.monto_total;
								pd.isSelected = pdp.isSelected;

								pd.isPDU = pdp.isPDU;
								pd.tipoPDU = pdp.tipoPDU;
								pd.patente = pdp.patente;
								pd.categoria = pdp.categoria;
								pd.fecha_circulacion = pdp.fecha_circulacion;
								pd.idBiller = pdp.idBiller;
								pd.idServicio = pdp.idServicio;
								listaUltimosPDU.Add(pd);
							}

							foreach (PaseTardio ptp in RealmDB.GetInstance().realm.All<PaseTardio>()) {
								var pt = new PaseTardio();
								pt.nombre_fantasia = ptp.nombre_fantasia;
								pt.fecha_vencimiento = ptp.fecha_vencimiento;
								pt.identificador = ptp.identificador;
								pt.monto_total = ptp.monto_total;
								pt.isSelected = ptp.isSelected;

								pt.isPDU = ptp.isPDU;
								pt.tipoPDU = ptp.tipoPDU;
								pt.patente = ptp.patente;
								pt.categoria = ptp.categoria;
								pt.fecha_circulacion = ptp.fecha_circulacion;
								pt.idBiller = ptp.idBiller;
								pt.idServicio = ptp.idServicio;
								listaUltimosPDT.Add(pt);
							}

							if (listaUltimosPDU.Count > 0) {
								listaUltimosPDU[0].isSelected = true;
							}
							if (listaUltimosPDT.Count > 0) {
								listaUltimosPDT[0].isSelected = true;
							}

							if (data.GetStringExtra("idBiller") == "886") {
								if (listaUltimosPDU.Count == 0) {
									Intent i = new Intent(this, typeof(PDUActivity));

									i.PutExtra("isLogin", isLogin);
									i.PutExtra("badgeCount", badgeCount.Text);
									i.PutExtra("idBiller", data.GetStringExtra("idBiller"));
									i.PutExtra("idServicio", data.GetStringExtra("idServicio"));
									i.PutExtra("isUPDU", false);

									StartActivityForResult(i, 11);
								} else {
									changeMainFragment(new FragmentUltimosPDU(isLogin,
																	  listAdd.Count.ToString(),
																	  data.GetStringExtra("idBiller"),
																	  data.GetStringExtra("idServicio"),
																	  this), "uPDU");
								}
							} else if (data.GetStringExtra("idBiller") == "964"){
								if (listaUltimosPDT.Count == 0) {
									Intent i = new Intent(this, typeof(PDUActivity));

									i.PutExtra("isLogin", isLogin);
									i.PutExtra("badgeCount", badgeCount.Text);
									i.PutExtra("idBiller", data.GetStringExtra("idBiller"));
									i.PutExtra("idServicio", data.GetStringExtra("idServicio"));
									i.PutExtra("isUPDU", false);

									StartActivityForResult(i, 11);
								} else {
									changeMainFragment(new FragmentUltimosPDU(isLogin,
																			  listAdd.Count.ToString(),
																			  data.GetStringExtra("idBiller"),
																			  data.GetStringExtra("idServicio"),
																			  this), "uPDU");
								}
							}
				 			
						break;
					}
				break;
				case 2: //Pago
					switch (data.GetStringExtra("pago")) {
						case "noAction":
							
						break;
					}
				break;
				case 11:
					if (data.GetStringExtra("pdu") == "abreCarroCompra") {
						carroCompraLayout.Visibility = ViewStates.Gone;
						sortMyAccounts.Visibility = ViewStates.Visible;
						addAccount.Visibility = ViewStates.Visible;

						isCarroCompra = true;
						isLogin = false;
						childFragment = new FragmentListaCuentas(isLogin, listAdd);
						changeMainFragment(childFragment, "mCarroCompra");
					} else if (data.GetStringExtra("pdu") == "reloadUPDU") {
						listaUltimosPDU = new List<PaseDiario>();
						listaUltimosPDT = new List<PaseTardio>();

						foreach (PaseDiario pdp in RealmDB.GetInstance().realm.All<PaseDiario>()) {
							var pd = new PaseDiario();
							pd.nombre_fantasia = pdp.nombre_fantasia;
							pd.fecha_vencimiento = pdp.fecha_vencimiento;
							pd.identificador = pdp.identificador;
							pd.monto_total = pdp.monto_total;
							pd.isSelected = pdp.isSelected;

							pd.isPDU = pdp.isPDU;
							pd.tipoPDU = pdp.tipoPDU;
							pd.patente = pdp.patente;
							pd.categoria = pdp.categoria;
							pd.fecha_circulacion = pdp.fecha_circulacion;
							pd.idBiller = pdp.idBiller;
							pd.idServicio = pdp.idServicio;
							listaUltimosPDU.Add(pd);
						}

						foreach (PaseTardio ptp in RealmDB.GetInstance().realm.All<PaseTardio>()) {
							var pt = new PaseTardio();
							pt.nombre_fantasia = ptp.nombre_fantasia;
							pt.fecha_vencimiento = ptp.fecha_vencimiento;
							pt.identificador = ptp.identificador;
							pt.monto_total = ptp.monto_total;
							pt.isSelected = ptp.isSelected;

							pt.isPDU = ptp.isPDU;
							pt.tipoPDU = ptp.tipoPDU;
							pt.patente = ptp.patente;
							pt.categoria = ptp.categoria;
							pt.fecha_circulacion = ptp.fecha_circulacion;
							pt.idBiller = ptp.idBiller;
							pt.idServicio = ptp.idServicio;
							listaUltimosPDT.Add(pt);
						}

						if (listaUltimosPDU.Count > 0) {
							listaUltimosPDU[0].isSelected = true;
						}
						if (listaUltimosPDT.Count > 0) {
							listaUltimosPDT[0].isSelected = true;
						}

						changeMainFragment(new FragmentUltimosPDU(isLogin,
																	  listAdd.Count.ToString(),
																	  idBiller,
																	  idServicio,
																	  this), "uPDU");
					} else if (data.GetStringExtra("pdu") == "noAction") {
						if (isLogin) {
							if (idBiller == "886") {
								if (listaUltimosPDU.Count == 0) {
									Intent intent = new Intent(this, typeof(AgregarActivity));
									intent.PutExtra("isPagoExpress", false);
									intent.PutExtra("autopista", true);
									intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
									StartActivityForResult(intent, 1);
								}
							} else if (idBiller == "964") {
								if (listaUltimosPDT.Count == 0) {
									Intent intent = new Intent(this, typeof(AgregarActivity));
									intent.PutExtra("isPagoExpress", false);
									intent.PutExtra("autopista", true);
									intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
									StartActivityForResult(intent, 1);
								}
							}
						} else {
							if (idBiller == "886") {
								if (listaUltimosPDU.Count == 0) {
									Intent intent = new Intent(this, typeof(AgregarActivity));
									intent.PutExtra("isPagoExpress", true);
									intent.PutExtra("autopista", true);
									intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
									StartActivityForResult(intent, 1);
								}
							} else if (idBiller == "964") {
								if (listaUltimosPDT.Count == 0) {
									Intent intent = new Intent(this, typeof(AgregarActivity));
									intent.PutExtra("isPagoExpress", true);
									intent.PutExtra("autopista", true);
									intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
									StartActivityForResult(intent, 1);
								}
							}
						}
					}
				break;
				case 22:
					if (data.GetStringExtra("actionEditar").Equals("reloadList")) {
						JObject parametros = new JObject();
						AndHUD.Shared.Show(this, null, -1, MaskType.Black);

						Log.Debug("canal", DeviceInformation.GetInstance().channel);
						Log.Debug("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
						Log.Debug("idUsuario", UserData.GetInstance().rut);
						Log.Debug("firma", "");

						parametros.Add("canal", DeviceInformation.GetInstance().channel);
						parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
						parametros.Add("idUsuario", UserData.GetInstance().rut);
						parametros.Add("firma", "");

						cuentasInscritas(parametros, true);
					}
				break;
			}
		}

		public void setValuesMain() {
			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMenu);
			PackageInfo pInfo = PackageManager.GetPackageInfo(PackageName, 0);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(false);

			navigationView = FindViewById<NavigationView>(Resource.Id.navView);
			navigationView.InflateMenu(Resource.Menu.MenuExpress);
			navigationView.ItemIconTintList = null;

			navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

			drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.menu_open_drawer, Resource.String.menu_close_drawer);
			drawerLayout.AddDrawerListener(drawerToggle);
			drawerToggle.SyncState();

			carroCompraLayout = FindViewById<RelativeLayout>(Resource.Id.carroCompraLayout);
			carroCompra = FindViewById<ImageButton>(Resource.Id.carroCompra);
			badgeCount = FindViewById<TextView>(Resource.Id.badgeText);
			sortMyAccounts = FindViewById<ImageView>(Resource.Id.sortMyAccounts);
			addAccount = FindViewById<ImageView>(Resource.Id.addAccount);
			version = FindViewById<TextView>(Resource.Id.version);
			version.Text = "Versión " + pInfo.VersionName;


			carroCompra.Click += (sender, e) => {
				carroCompraLayout.Visibility = ViewStates.Gone;
				sortMyAccounts.Visibility = ViewStates.Visible;
				addAccount.Visibility = ViewStates.Visible;

				isCarroCompra = true;
				isLogin = false;
				childFragment = new FragmentListaCuentas(isLogin, listAdd);
				changeMainFragment(childFragment, "mCarroCompra");
			};
			sortMyAccounts.Click += (sender, e) => {
				if (isCarroCompra) {
					showSortDialogPE();
				} else {
					showSortDialog();
				}
			};

			addAccount.Click += (sender, e) => {
				childFragment.openAddService(isCarroCompra);
			};
		}

		private void getAccountsPE() {
			foreach (MisCuentas ape in RealmDB.GetInstance().realm.All<MisCuentas>()) {
				listAdd.Add(ape);
			}

			badgeCount.Text = listAdd.Count.ToString();
		}

		private void showSortDialog() {
			Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
			builder.SetView(LayoutInflater.Inflate(Resource.Layout.SortDialog, null));
			mSortDialog = builder.Create();
			mSortDialog.Show();

			var width = Resources.DisplayMetrics.WidthPixels;
			int actionBarHeight = 0;
			TypedValue tv = new TypedValue();
			if (Theme.ResolveAttribute(Resource.Attribute.actionBarSize, tv, true)) {
				actionBarHeight = TypedValue.ComplexToDimensionPixelSize(tv.Data, Resources.DisplayMetrics);
			}
			WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
			lp.CopyFrom(mSortDialog.Window.Attributes);

			lp.Width = width * 5/6;
			lp.Height = ViewGroup.LayoutParams.WrapContent;
			lp.Gravity = GravityFlags.Top | GravityFlags.End;
			lp.Y = actionBarHeight-50;
			mSortDialog.Window.Attributes = lp;

			upperAlphabetic = mSortDialog.FindViewById<ImageView>(Resource.Id.upperAlphabetic);
			downerAlphabetic = mSortDialog.FindViewById<ImageView>(Resource.Id.downerAlphabetic);
			upperAlias = mSortDialog.FindViewById<ImageView>(Resource.Id.upperAlias);
			downerAlias = mSortDialog.FindViewById<ImageView>(Resource.Id.downerAlias);

			setSortSelected();

			upperAlphabetic.Click += (sender, e) => {
				onUpperAlphaClick();
			};

			downerAlphabetic.Click += (sender, e) => {
				onDownerAlphaClick();
			};

			upperAlias.Click += (sender, e) => {
				onUpperAliasClick();
			};

			downerAlias.Click += (sender, e) => {
				onDownerAliasClick();
			};
		}

		private void showSortDialogPE() {
			Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
			builder.SetView(LayoutInflater.Inflate(Resource.Layout.SortDialogPE, null));
			mSortDialogPE = builder.Create();
			mSortDialogPE.Show();

			var width = Resources.DisplayMetrics.WidthPixels;
			int actionBarHeight = 0;
			TypedValue tv = new TypedValue();
			if (Theme.ResolveAttribute(Resource.Attribute.actionBarSize, tv, true)) {
				actionBarHeight = TypedValue.ComplexToDimensionPixelSize(tv.Data, Resources.DisplayMetrics);
			}
			WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
			lp.CopyFrom(mSortDialogPE.Window.Attributes);

			lp.Width = width * 5 / 6;
			lp.Height = ViewGroup.LayoutParams.WrapContent;
			lp.Gravity = GravityFlags.Top | GravityFlags.End;
			lp.Y = actionBarHeight - 50;
			mSortDialogPE.Window.Attributes = lp;

			upperAlphabeticPE = mSortDialogPE.FindViewById<ImageView>(Resource.Id.upperAlphabeticPE);
			downerAlphabeticPE = mSortDialogPE.FindViewById<ImageView>(Resource.Id.downerAlphabeticPE);

			if (isCarroCompra) {
				setSortSelectedPE();
			} else {
				setSortSelected();	
			}

			upperAlphabeticPE.Click += (sender, e) => {
				onUpperAlphaClick();
			};

			downerAlphabeticPE.Click += (sender, e) => {
				onDownerAlphaClick();
			};
		}

		private void setSortSelected() {
			switch (sortSelected) {
				case 0:
					upperAlphabetic.SetImageResource(Resource.Drawable.mayor_color);
				break;
				case 1:
					downerAlphabetic.SetImageResource(Resource.Drawable.menor_color);
				break;
				case 2:
					upperAlias.SetImageResource(Resource.Drawable.mayor_color);
				break;
				case 3:
					downerAlias.SetImageResource(Resource.Drawable.menor_color);
				break;
			}
		}

		private void setSortSelectedPE() {
			switch (sortSelectedPE) {
				case 0:
					upperAlphabeticPE.SetImageResource(Resource.Drawable.mayor_color);
				break;
				case 1:
					downerAlphabeticPE.SetImageResource(Resource.Drawable.menor_color);
				break;
			}
		}

		private void onUpperAlphaClick() {
			childFragment.sortNombreCuenta(false);

			if (isCarroCompra) {
				upperAlphabeticPE.SetImageResource(Resource.Drawable.mayor_color);
				downerAlphabeticPE.SetImageResource(Resource.Drawable.menor_blanco);

				var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortTypePE == sortSelectedPE).First();
				sortSelectedPE = 0;
				RealmDB.GetInstance().realm.Write(() => pData.sortTypePE = sortSelectedPE);
				mSortDialogPE.Hide();
			} else {
				upperAlphabetic.SetImageResource(Resource.Drawable.mayor_color);
				downerAlphabetic.SetImageResource(Resource.Drawable.menor_blanco);
				upperAlias.SetImageResource(Resource.Drawable.mayor_blanco);
				downerAlias.SetImageResource(Resource.Drawable.menor_blanco);

				var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortType == sortSelected).First();
				sortSelected = 0;
				RealmDB.GetInstance().realm.Write(() => pData.sortType = sortSelected);
				mSortDialog.Hide();
			}
		}

		private void onDownerAlphaClick() {
			childFragment.sortNombreCuenta(true);

			if (isCarroCompra) {
				upperAlphabeticPE.SetImageResource(Resource.Drawable.mayor_blanco);
				downerAlphabeticPE.SetImageResource(Resource.Drawable.menor_color);
				var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortTypePE == sortSelectedPE).First();
				sortSelectedPE = 1;
				RealmDB.GetInstance().realm.Write(() => pData.sortTypePE = sortSelectedPE);
				mSortDialogPE.Hide();
			} else {
				upperAlphabetic.SetImageResource(Resource.Drawable.mayor_blanco);
				downerAlphabetic.SetImageResource(Resource.Drawable.menor_color);
				upperAlias.SetImageResource(Resource.Drawable.mayor_blanco);
				downerAlias.SetImageResource(Resource.Drawable.menor_blanco);

				var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortType == sortSelected).First();
				sortSelected = 1;
				RealmDB.GetInstance().realm.Write(() => pData.sortType = sortSelected);
				mSortDialog.Hide();
			}
		}

		private void onUpperAliasClick() {
			childFragment.sortAliasCuenta(false);

			upperAlphabetic.SetImageResource(Resource.Drawable.mayor_blanco);
			downerAlphabetic.SetImageResource(Resource.Drawable.menor_blanco);

			upperAlias.SetImageResource(Resource.Drawable.mayor_color);
			downerAlias.SetImageResource(Resource.Drawable.menor_blanco);

			mSortDialog.Hide();

			var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortType == sortSelected).First();
			sortSelected = 2;
			RealmDB.GetInstance().realm.Write(() => pData.sortType = sortSelected);
		}

		private void onDownerAliasClick() {
			childFragment.sortAliasCuenta(true);
			upperAlphabetic.SetImageResource(Resource.Drawable.mayor_blanco);
			downerAlphabetic.SetImageResource(Resource.Drawable.menor_blanco);

			upperAlias.SetImageResource(Resource.Drawable.mayor_blanco);
			downerAlias.SetImageResource(Resource.Drawable.menor_color);

			mSortDialog.Hide();

			var pData = RealmDB.GetInstance().realm.All<PersistentData>().Where(d => d.sortType == sortSelected).First();
			sortSelected = 3;
			RealmDB.GetInstance().realm.Write(() => pData.sortType = sortSelected);
		}

		void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e) {
			switch (e.MenuItem.ItemId) {
				case (Resource.Id.misCuentas):
					if (idFragment != "home") {
						isLogin = true;
						changeMainFragment(new FragmentListaCuentas(isLogin, listaCuentasInscritas), idFragment);
					}
					break;
				case (Resource.Id.autopistas):
					if (isLogin) {
						Intent intent = new Intent(this, typeof(AgregarActivity));
						intent.PutExtra("isPagoExpress", false);
						intent.PutExtra("autopista", true);
						intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
						StartActivityForResult(intent, 1);
					} else {
						Intent intent = new Intent(this, typeof(AgregarActivity));
						intent.PutExtra("isPagoExpress", true);
						intent.PutExtra("autopista", true);
						intent.PutExtra("listAdd", JsonConvert.SerializeObject(listAdd));
						StartActivityForResult(intent, 1);
					}
				break;
				case (Resource.Id.recargas):
					if (isLogin) {
						JObject parametros = new JObject();
						AndHUD.Shared.Show(this, null, -1, MaskType.Black);

						parametros.Add("canal", DeviceInformation.GetInstance().channel);
						parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
						parametros.Add("firma", "");
						parametros.Add("idUsuario", UserData.GetInstance().rut);

						getUltimasRecargas(parametros);
					} else {
						carroCompraLayout.Visibility = ViewStates.Visible;
						sortMyAccounts.Visibility = ViewStates.Gone;
						addAccount.Visibility = ViewStates.Gone;
						idFragment = "recargas";
						
						if (RealmDB.GetInstance().realm.All<SolicitaRecargaPE>().Count() == 0) {
							changeMainFragment(new FragmentUltimasRecargas(listURPE, isLogin, badgeCount.Text, this), idFragment);
						} else {
							listURPE.Clear();
							foreach (SolicitaRecargaPE srpe in RealmDB.GetInstance().realm.All<SolicitaRecargaPE>()) {
								var sr = new SolicitaRecarga();
								sr.id_periodo_solicitado = srpe.id_periodo_solicitado;
								sr.id_pago_solicitado = srpe.id_pago_solicitado;
								sr.nombreBiller = srpe.nombreBiller;
								sr.acepta_abono = srpe.acepta_abono;
								sr.acepta_pago_min = srpe.acepta_pago_min;
								sr.boleta = srpe.boleta;
								sr.direccion_factura = srpe.direccion_factura;
								sr.fecha_vencimiento = srpe.fecha_vencimiento;
								sr.id_biller = srpe.id_biller;
								sr.id_servicio = srpe.id_servicio;
								sr.identificador = srpe.identificador;
								sr.monto_minimo = srpe.monto_minimo;
								sr.monto_total = srpe.monto_total;
								sr.texto_facturador = srpe.texto_facturador;
								sr.rut = srpe.rut;
								sr.email = srpe.email;
								sr.isSelected = false;
								sr.logoEmpresa = srpe.logoEmpresa;
								listURPE.Add(sr);
							}
							listURPE[0].isSelected = true;
							changeMainFragment(new FragmentUltimasRecargas(listURPE, isLogin, badgeCount.Text, this), idFragment);
						}
					}
					
					break;
				case (Resource.Id.comoFunciona):
					Toast.MakeText(this, "Cómo funciona!", ToastLength.Short).Show();
					break;
				case (Resource.Id.cerrarSesion):
					Toast.MakeText(this, "Su sesión ha finalizado correctamente.", ToastLength.Short).Show();
					carroCompraLayout.Visibility = ViewStates.Visible;
					sortMyAccounts.Visibility = ViewStates.Gone;
					addAccount.Visibility = ViewStates.Gone;
					idFragment = "login";
					isLogin = false;

					changeMainFragment(new FragmentLogin(SupportFragmentManager, this), idFragment);

					View header = this.navigationView.GetHeaderView(0);
					this.navigationView.RemoveHeaderView(header);
					this.navigationView.Menu.Clear();
					this.navigationView.InflateMenu(Resource.Menu.MenuExpress);
					break;
				case (Resource.Id.inicio):
					carroCompraLayout.Visibility = ViewStates.Visible;
					sortMyAccounts.Visibility = ViewStates.Gone;
					addAccount.Visibility = ViewStates.Gone;
					idFragment = "login";
					badgeCount.Text = listAdd.Count.ToString();

					changeMainFragment(new FragmentLogin(SupportFragmentManager, this), idFragment);
				break;
			}
			// Close drawer
			drawerLayout.CloseDrawers();
		}

		public async void getUltimasRecargas(JObject parametros) {
			var respuesta = await MyClass.WorklightClient.UnprotectedInvokeAsync("ultimasRecargas", "ultimas_recargas", "POST", parametros);

			if (respuesta.Success) {
				if ((int)respuesta.State["Error"] == 0) {
					carroCompraLayout.Visibility = ViewStates.Gone;
					sortMyAccounts.Visibility = ViewStates.Gone;
					addAccount.Visibility = ViewStates.Gone;
					idFragment = "recargas";

					changeMainFragment(new FragmentUltimasRecargas(setListUltimasRecargas(respuesta.Response), isLogin, "", this), idFragment);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", respuesta.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", respuesta.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}
			AndHUD.Shared.Dismiss(this);
		}

		public void changeMainFragment(Android.Support.V4.App.Fragment fragment, string idFragment) {
			var ft = SupportFragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.defaultFrame, fragment, idFragment);
			ft.Commit();
		}

		public void changeMenu(string name) {
			navigationView.Menu.Clear();
			navigationView.InflateMenu(Resource.Menu.MenuUser);
			navigationView.InflateHeaderView(Resource.Layout.HeaderMenu);

			var nView = navigationView.GetHeaderView(0);
			welcomeText = nView.FindViewById<TextView>(Resource.Id.welcomeText);
			welcomeText.Text = "¡Bienvenido " + name + "!";

			userImage = nView.FindViewById<ImageView>(Resource.Id.userImage);
			Glide.With(this)
			 .Load("https://www.smashingmagazine.com/wp-content/uploads/2015/06/10-dithering-opt.jpg")
			 .Transform(new CircleTransform(this))
			 .Into(userImage);

			userImage.Click += (sender, e) => {
				JObject parametros = new JObject();
				AndHUD.Shared.Show(this, null, -1, MaskType.Black);

				parametros.Add("canal", DeviceInformation.GetInstance().channel);
				parametros.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
				parametros.Add("idUsuario", UserData.GetInstance().rut);
				parametros.Add("firma", "");

				listadoDatosUsuario(parametros);

			};
		}

		public async void loginUser(JObject parametros, Android.Content.Intent data) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("login", "login", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					UserData.GetInstance(response.Response["DatosUsuarioLogin"]["Nombre"].ToString(),
					                     utils.getUserId(data.GetStringExtra("user")), data.GetStringExtra("user"),
										 "", "", response.Response["DatosUsuarioLogin"]["Email"].ToString(), "", "");

					var pData = RealmDB.GetInstance().realm.All<PersistentData>().First();
					RealmDB.GetInstance().realm.Write(() => pData.cookie = response.Response["DatosUsuarioLogin"]["Cookie"].ToString());

					parametros["sesion"] = response.Response["DatosUsuarioLogin"]["Cookie"].ToString();
					cuentasInscritas(parametros, false);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();

					AndHUD.Shared.Dismiss(this);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();

				AndHUD.Shared.Dismiss(this);
			}
		}

		public async void cuentasInscritas(JObject parametros, bool isReload) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("cuentasInscritas", "cuentas_inscritas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					if (isReload) {
						childFragment.adapter.reloadList(setListCuentasInscritas(response.Response));
						childFragment.adapter.refreshAdapter();
					} else {
						carroCompraLayout.Visibility = ViewStates.Gone;
						sortMyAccounts.Visibility = ViewStates.Visible;
						addAccount.Visibility = ViewStates.Visible;

						isCarroCompra = false;
						isLogin = true;
						idFragment = "home";


						childFragment = new FragmentListaCuentas(isLogin, setListCuentasInscritas(response.Response));
						changeMainFragment(childFragment, idFragment);
						changeMenu(UserData.GetInstance().nombre.Split(' ')[1]);
					}
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();
			}

			AndHUD.Shared.Dismiss(this);
		}

		public async void listadoDatosUsuario(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("listaInformacionCliente", "lista_informacion_cliente", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					UserData.GetInstance().nombre = response.Response["ObtieneDatosCliente"]["nombres"] + " " +
										 response.Response["ObtieneDatosCliente"]["apellidos"];
					UserData.GetInstance().region = response.Response["ObtieneDatosCliente"]["region"].ToString();
					UserData.GetInstance().comuna = response.Response["ObtieneDatosCliente"]["comuna"].ToString();
					UserData.GetInstance().correo = response.Response["ObtieneDatosCliente"]["email"].ToString();
					UserData.GetInstance().cumpleanos = response.Response["ObtieneDatosCliente"]["fecha_nacimiento"].ToString();

					JObject param = new JObject();
					param.Add("sesion", RealmDB.GetInstance().realm.All<PersistentData>().First().cookie);
					param.Add("idRegion", response.Response["ObtieneDatosCliente"]["region"].ToString());

					listadoComunas(param);
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();

					AndHUD.Shared.Dismiss(this);
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();

				AndHUD.Shared.Dismiss(this);
			}
		}

		public async void listadoComunas(JObject parametros) {
			var response = await MyClass.WorklightClient.UnprotectedInvokeAsync("obtieneComunas", "obtiene_comunas", "POST", parametros);

			if (response.Success) {
				if ((int)response.State["Error"] == 0) {
					ListadoComuna.GetInstance(setListComunas(response.Response));

					carroCompraLayout.Visibility = ViewStates.Gone;
					sortMyAccounts.Visibility = ViewStates.Gone;
					addAccount.Visibility = ViewStates.Gone;

					idFragment = "misDatos";
					changeMainFragment(new FragmentMisDatos(SupportFragmentManager), idFragment);
					drawerLayout.CloseDrawers();
				} else {
					CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.State["Mensaje"].ToString(), "Aceptar", "", null, null);
					alert.showDialog();
				}
			} else {
				CustomAlertDialog alert = new CustomAlertDialog(this, "¡Oops!", response.Message, "Aceptar", "", null, null);
				alert.showDialog();

				AndHUD.Shared.Dismiss(this);
			}
			AndHUD.Shared.Dismiss(this);
		}

		private List<MisCuentas> setListCuentasInscritas(JObject response) {
			listaCuentasInscritas.Clear(); 
			var listCI = response["CuentasInscritas"];
			for (var i = 0; i < listCI.Count(); i++) {
				Log.Debug("ASD", listCI[i]["alias"].ToString());
				listaCuentasInscritas.Add(new MisCuentas(listCI[i]["alias"].ToString(),
									   listCI[i]["biller"].ToString(),
									   (int)listCI[i]["id_biller"],
									   (int)listCI[i]["id_servicio"],
									   listCI[i]["identificador"].ToString(),
									   listCI[i]["imagen_biller"].ToString(),
									   listCI[i]["imagen_servicio"].ToString(),
									   (bool)listCI[i]["modificable"],
										listCI[i]["servicio"].ToString()));
			}

			return listaCuentasInscritas;
		}

		private List<Recargas> setListUltimasRecargas(JObject response) {
			var listaRecargas = new List<Recargas>();
			var listR = response["ObtieneUltimasRecargas"];

			for (var i = 0; i < listR.Count(); i++) {
				Recargas r = new Recargas();
				r.user_id = listR[i]["user_id"].ToString();
				r.id_pago = (int)listR[i]["id_pago"];
				r.id_servicio = (int)listR[i]["id_servicio"];
				r.descripcion_servicio = listR[i]["descripción_servicio"].ToString();
				r.codigo_identificacion = listR[i]["codigo_identificacion"].ToString();
				r.id_biller = (int)listR[i]["id_biller"];
				r.nombre_biller = listR[i]["nombre_biller"].ToString();
				r.monto_pago = (int)listR[i]["monto_pago"];
				r.alias = listR[i]["alias"].ToString();
				listaRecargas.Add(r);
			}
			if (listaRecargas.Count > 0) {
				listaRecargas[0].isSelected = true;
			}

			return listaRecargas;
		}

		private List<RegionComuna> setListComunas(JObject response) {
			List<RegionComuna> list = new List<RegionComuna>();

			var listComunas = response["ListadoComunas"];

			for (var i = 0; i < listComunas.Count(); i++) {
				list.Add(new RegionComuna(
					listComunas[i]["id_comuna"].ToString(), listComunas[i]["nombre_comuna"].ToString()));
			}

			return list;
		}
	}
}