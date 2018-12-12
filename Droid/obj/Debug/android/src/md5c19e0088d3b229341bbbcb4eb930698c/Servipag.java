package md5c19e0088d3b229341bbbcb4eb930698c;


public class Servipag
	extends android.support.multidex.MultiDexApplication
	implements
		mono.android.IGCUserPeer,
		com.browser2app.khenshin.KhenshinApplication
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:()V:GetOnCreateHandler\n" +
			"n_getKhenshin:()Lcom/browser2app/khenshin/KhenshinInterface;:GetGetKhenshinHandler:Com.Browser2app.Khenshin.IKhenshinApplicationInvoker, Binding-khenshin\n" +
			"";
	}

	public Servipag ()
	{
		mono.MonoPackageManager.setContext (this);
	}


	public void onCreate ()
	{
		n_onCreate ();
	}

	private native void n_onCreate ();


	public com.browser2app.khenshin.KhenshinInterface getKhenshin ()
	{
		return n_getKhenshin ();
	}

	private native com.browser2app.khenshin.KhenshinInterface n_getKhenshin ();

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
