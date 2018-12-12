package md5c19e0088d3b229341bbbcb4eb930698c;


public class ServiciosRecargaViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ServipagMobile.Droid.ServiciosRecargaViewHolder, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ServiciosRecargaViewHolder.class, __md_methods);
	}


	public ServiciosRecargaViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == ServiciosRecargaViewHolder.class)
			mono.android.TypeManager.Activate ("ServipagMobile.Droid.ServiciosRecargaViewHolder, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

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
