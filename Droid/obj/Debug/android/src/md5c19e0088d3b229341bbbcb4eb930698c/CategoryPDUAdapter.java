package md5c19e0088d3b229341bbbcb4eb930698c;


public class CategoryPDUAdapter
	extends android.support.v4.app.FragmentStatePagerAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItem:(I)Landroid/support/v4/app/Fragment;:GetGetItem_IHandler\n" +
			"";
		mono.android.Runtime.register ("ServipagMobile.Droid.CategoryPDUAdapter, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CategoryPDUAdapter.class, __md_methods);
	}


	public CategoryPDUAdapter (android.support.v4.app.FragmentManager p0)
	{
		super (p0);
		if (getClass () == CategoryPDUAdapter.class)
			mono.android.TypeManager.Activate ("ServipagMobile.Droid.CategoryPDUAdapter, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentManager, Xamarin.Android.Support.Fragment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public android.support.v4.app.Fragment getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native android.support.v4.app.Fragment n_getItem (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
