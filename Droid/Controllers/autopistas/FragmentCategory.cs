using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class FragmentCategory : Fragment {
		private string category;
		private int idImgCategory;

		private ImageView imgCategory;
		private TextView hintCategory;

		public FragmentCategory() { }

		public FragmentCategory(string category, int idImgCategory) {
			this.category = category;
			this.idImgCategory = idImgCategory;
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			return inflater.Inflate(Resource.Layout.RowCategoryPDU, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState) {
			base.OnViewCreated(view, savedInstanceState);

			imgCategory = view.FindViewById<ImageView>(Resource.Id.imgCategory);
			hintCategory = view.FindViewById<TextView>(Resource.Id.hintCategory);

			imgCategory.SetImageDrawable(Resources.GetDrawable(idImgCategory));
			hintCategory.Text = category;
		}
	}
}
