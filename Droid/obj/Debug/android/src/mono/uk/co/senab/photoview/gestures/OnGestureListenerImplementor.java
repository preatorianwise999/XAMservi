package mono.uk.co.senab.photoview.gestures;


public class OnGestureListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		uk.co.senab.photoview.gestures.OnGestureListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrag:(FF)V:GetOnDrag_FFHandler:UK.CO.Senab.Photoview.Gestures.IOnGestureListenerInvoker, PhotoViewBinding\n" +
			"n_onFling:(FFFF)V:GetOnFling_FFFFHandler:UK.CO.Senab.Photoview.Gestures.IOnGestureListenerInvoker, PhotoViewBinding\n" +
			"n_onScale:(FFF)V:GetOnScale_FFFHandler:UK.CO.Senab.Photoview.Gestures.IOnGestureListenerInvoker, PhotoViewBinding\n" +
			"";
		mono.android.Runtime.register ("UK.CO.Senab.Photoview.Gestures.IOnGestureListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnGestureListenerImplementor.class, __md_methods);
	}


	public OnGestureListenerImplementor ()
	{
		super ();
		if (getClass () == OnGestureListenerImplementor.class)
			mono.android.TypeManager.Activate ("UK.CO.Senab.Photoview.Gestures.IOnGestureListenerImplementor, PhotoViewBinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDrag (float p0, float p1)
	{
		n_onDrag (p0, p1);
	}

	private native void n_onDrag (float p0, float p1);


	public void onFling (float p0, float p1, float p2, float p3)
	{
		n_onFling (p0, p1, p2, p3);
	}

	private native void n_onFling (float p0, float p1, float p2, float p3);


	public void onScale (float p0, float p1, float p2)
	{
		n_onScale (p0, p1, p2);
	}

	private native void n_onScale (float p0, float p1, float p2);

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
