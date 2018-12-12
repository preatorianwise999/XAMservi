using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace ServipagMobile.Droid {
	public class CategoryPDUAdapter : FragmentStatePagerAdapter{
		private List<Fragment> fragmentList = new List<Fragment>();
		private ImageView imgCategory;
		private TextView hintCategory;
		private FragmentManager mFragmentManager;

		public CategoryPDUAdapter(FragmentManager fm) :base(fm) {
			mFragmentManager = fm;
		}

		public override int Count {
			get {
				return fragmentList.Count;
			}
		}
		public override Fragment GetItem(int position) {
			return fragmentList[position];
		}

		/*public override Java.Lang.Object InstantiateItem(Android.Views.View container, int position) {
			LayoutInflater inflater = (LayoutInflater)this
				.(Context.LayoutInflaterService);
			
			View view = inflater.Inflate(Resource.Layout.RowCategoryPDU, null);

			imgCategory = view.FindViewById<ImageView>(Resource.Id.imgCategory);
			hintCategory = view.FindViewById<TextView>(Resource.Id.hintCategory);

			((ViewPager)container).AddView(view, 0);

			return view;
		}*/

		public void addFragment(Fragment fragment) {
			fragmentList.Add(fragment);
			NotifyDataSetChanged();
		}

		public void clearFragments() {
			fragmentList.Clear();
		}
	}
}
