package mono.uk.co.senab.photoview;


public class PhotoViewAttacher_OnMatrixChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		uk.co.senab.photoview.PhotoViewAttacher.OnMatrixChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMatrixChanged:(Landroid/graphics/RectF;)V:GetOnMatrixChanged_Landroid_graphics_RectF_Handler:UK.CO.Senab.Photoview.PhotoViewAttacher/IOnMatrixChangedListenerInvoker, PhotoViewBinding\n" +
			"";
		mono.android.Runtime.register ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnMatrixChangedListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PhotoViewAttacher_OnMatrixChangedListenerImplementor.class, __md_methods);
	}


	public PhotoViewAttacher_OnMatrixChangedListenerImplementor ()
	{
		super ();
		if (getClass () == PhotoViewAttacher_OnMatrixChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("UK.CO.Senab.Photoview.PhotoViewAttacher+IOnMatrixChangedListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onMatrixChanged (android.graphics.RectF p0)
	{
		n_onMatrixChanged (p0);
	}

	private native void n_onMatrixChanged (android.graphics.RectF p0);

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
