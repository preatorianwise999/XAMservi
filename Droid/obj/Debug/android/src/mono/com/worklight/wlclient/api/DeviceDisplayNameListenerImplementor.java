package mono.com.worklight.wlclient.api;


public class DeviceDisplayNameListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.worklight.wlclient.api.DeviceDisplayNameListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFailure:(Lcom/worklight/wlclient/api/WLFailResponse;)V:GetOnFailure_Lcom_worklight_wlclient_api_WLFailResponse_Handler:Worklight.Android.IDeviceDisplayNameListenerInvoker, Worklight.Android\n" +
			"n_onSuccess:(Ljava/lang/String;)V:GetOnSuccess_Ljava_lang_String_Handler:Worklight.Android.IDeviceDisplayNameListenerInvoker, Worklight.Android\n" +
			"";
		mono.android.Runtime.register ("Worklight.Android.IDeviceDisplayNameListenerImplementor, Worklight.Android, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null", DeviceDisplayNameListenerImplementor.class, __md_methods);
	}


	public DeviceDisplayNameListenerImplementor ()
	{
		super ();
		if (getClass () == DeviceDisplayNameListenerImplementor.class)
			mono.android.TypeManager.Activate ("Worklight.Android.IDeviceDisplayNameListenerImplementor, Worklight.Android, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onFailure (com.worklight.wlclient.api.WLFailResponse p0)
	{
		n_onFailure (p0);
	}

	private native void n_onFailure (com.worklight.wlclient.api.WLFailResponse p0);


	public void onSuccess (java.lang.String p0)
	{
		n_onSuccess (p0);
	}

	private native void n_onSuccess (java.lang.String p0);

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
