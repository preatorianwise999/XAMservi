package mono.uk.co.senab.photoview;


public class PhotoViewAttacher_OnPhotoTapListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		uk.co.senab.photoview.PhotoViewAttacher.OnPhotoTapListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPhotoTap:(Landroid/view/View;FF)V:GetOnPhotoTap_Landroid_view_View_FFHandler:UK.CO.Senab.Photoview.PhotoViewAttacher/IOnPhotoTapListenerInvoker, PhotoViewBinding\n" +
			"";
		mono.android.Runtime.register ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnPhotoTapListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PhotoViewAttacher_OnPhotoTapListenerImplementor.class, __md_methods);
	}


	public PhotoViewAttacher_OnPhotoTapListenerImplementor ()
	{
		super ();
		if (getClass () == PhotoViewAttacher_OnPhotoTapListenerImplementor.class)
			mono.android.TypeManager.Activate ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnPhotoTapListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPhotoTap (android.view.View p0, float p1, float p2)
	{
		n_onPhotoTap (p0, p1, p2);
	}

	private native void n_onPhotoTap (android.view.View p0, float p1, float p2);

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
