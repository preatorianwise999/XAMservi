package mono.com.worklight.wlclient;


public class WLHybridHttpListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.worklight.wlclient.WLHybridHttpListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onException:(Ljava/lang/Exception;)V:GetOnException_Ljava_lang_Exception_Handler:Worklight.Android.IWLHybridHttpListenerInvoker, Worklight.Android\n" +
			"n_onResponse:(Lcom/squareup/okhttp/Response;)V:GetOnResponse_Lcom_squareup_okhttp_Response_Handler:Worklight.Android.IWLHybridHttpListenerInvoker, Worklight.Android\n" +
			"";
		mono.android.Runtime.register ("Worklight.Android.IWLHybridHttpListenerImplementor, Worklight.Android, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null", WLHybridHttpListenerImplementor.class, __md_methods);
	}


	public WLHybridHttpListenerImplementor ()
	{
		super ();
		if (getClass () == WLHybridHttpListenerImplementor.class)
			mono.android.TypeManager.Activate ("Worklight.Android.IWLHybridHttpListenerImplementor, Worklight.Android, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onException (java.lang.Exception p0)
	{
		n_onException (p0);
	}

	private native void n_onException (java.lang.Exception p0);


	public void onResponse (com.squareup.okhttp.Response p0)
	{
		n_onResponse (p0);
	}

	private native void n_onResponse (com.squareup.okhttp.Response p0);

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
