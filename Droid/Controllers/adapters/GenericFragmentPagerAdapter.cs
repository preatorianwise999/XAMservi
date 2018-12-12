using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;

namespace ServipagMobile.Droid {
	public class GenericFragmentPagerAdapter : FragmentStatePagerAdapter {
		private List<Fragment> fragmentList = new List<Fragment>();
		private List<string> titleList = new List<string>();
		private FragmentManager mFragmentManager;
		private FragmentTransaction mCurTransaction;

		public GenericFragmentPagerAdapter(FragmentManager fm) :base(fm) {
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

		public override ICharSequence GetPageTitleFormatted(int position) {
			return new String(titleList[position].ToLower());
		}

		public void addFragment(Fragment fragment, string title) {
			fragmentList.Add(fragment);
			titleList.Add(title);
		}
	}
}
