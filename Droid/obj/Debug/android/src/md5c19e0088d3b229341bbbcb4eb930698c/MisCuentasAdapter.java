package md5c19e0088d3b229341bbbcb4eb930698c;


public class MisCuentasAdapter
	extends com.daimajia.swipe.adapters.RecyclerSwipeAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemCount:()I:GetGetItemCountHandler\n" +
			"n_onBindViewHolder:(Landroid/support/v7/widget/RecyclerView$ViewHolder;I)V:GetOnBindViewHolder_Landroid_support_v7_widget_RecyclerView_ViewHolder_IHandler\n" +
			"n_onCreateViewHolder:(Landroid/view/ViewGroup;I)Landroid/support/v7/widget/RecyclerView$ViewHolder;:GetOnCreateViewHolder_Landroid_view_ViewGroup_IHandler\n" +
			"n_getSwipeLayoutResourceId:(I)I:GetGetSwipeLayoutResourceId_IHandler\n" +
			"";
		mono.android.Runtime.register ("ServipagMobile.Droid.MisCuentasAdapter, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MisCuentasAdapter.class, __md_methods);
	}


	public MisCuentasAdapter ()
	{
		super ();
		if (getClass () == MisCuentasAdapter.class)
			mono.android.TypeManager.Activate ("ServipagMobile.Droid.MisCuentasAdapter, ServipagMobile.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public int getItemCount ()
	{
		return n_getItemCount ();
	}

	private native int n_getItemCount ();


	public void onBindViewHolder (android.support.v7.widget.RecyclerView.ViewHolder p0, int p1)
	{
		n_onBindViewHolder (p0, p1);
	}

	private native void n_onBindViewHolder (android.support.v7.widget.RecyclerView.ViewHolder p0, int p1);


	public android.support.v7.widget.RecyclerView.ViewHolder onCreateViewHolder (android.view.ViewGroup p0, int p1)
	{
		return n_onCreateViewHolder (p0, p1);
	}

	private native android.support.v7.widget.RecyclerView.ViewHolder n_onCreateViewHolder (android.view.ViewGroup p0, int p1);


	public int getSwipeLayoutResourceId (int p0)
	{
		return n_getSwipeLayoutResourceId (p0);
	}

	private native int n_getSwipeLayoutResourceId (int p0);

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
