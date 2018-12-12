package mono.uk.co.senab.photoview;


public class PhotoViewAttacher_OnViewTapListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		uk.co.senab.photoview.PhotoViewAttacher.OnViewTapListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onViewTap:(Landroid/view/View;FF)V:GetOnViewTap_Landroid_view_View_FFHandler:UK.CO.Senab.Photoview.PhotoViewAttacher/IOnViewTapListenerInvoker, PhotoViewBinding\n" +
			"";
		mono.android.Runtime.register ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnViewTapListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PhotoViewAttacher_OnViewTapListenerImplementor.class, __md_methods);
	}


	public PhotoViewAttacher_OnViewTapListenerImplementor ()
	{
		super ();
		if (getClass () == PhotoViewAttacher_OnViewTapListenerImplementor.class)
			mono.android.TypeManager.Activate ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnViewTapListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onViewTap (android.view.View p0, float p1, float p2)
	{
		n_onViewTap (p0, p1, p2);
	}

	private native void n_onViewTap (android.view.View p0, float p1, float p2);

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
