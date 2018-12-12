package md590d78cc84980ea1513896bb26e646dea;


public class IntEditTextPreference
	extends android.preference.EditTextPreference
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getPersistedString:(Ljava/lang/String;)Ljava/lang/String;:GetGetPersistedString_Ljava_lang_String_Handler\n" +
			"n_persistString:(Ljava/lang/String;)Z:GetPersistString_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("com.refractored.monodroidtoolkit.preferneces.IntEditTextPreference, com.refractored.monodroidtoolkit, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null", IntEditTextPreference.class, __md_methods);
	}


	public IntEditTextPreference (android.content.Context p0)
	{
		super (p0);
		if (getClass () == IntEditTextPreference.class)
			mono.android.TypeManager.Activate ("com.refractored.monodroidtoolkit.preferneces.IntEditTextPreference, com.refractored.monodroidtoolkit, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public IntEditTextPreference (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == IntEditTextPreference.class)
			mono.android.TypeManager.Activate ("com.refractored.monodroidtoolkit.preferneces.IntEditTextPreference, com.refractored.monodroidtoolkit, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public IntEditTextPreference (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == IntEditTextPreference.class)
			mono.android.TypeManager.Activate ("com.refractored.monodroidtoolkit.preferneces.IntEditTextPreference, com.refractored.monodroidtoolkit, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public IntEditTextPreference (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == IntEditTextPreference.class)
			mono.android.TypeManager.Activate ("com.refractored.monodroidtoolkit.preferneces.IntEditTextPreference, com.refractored.monodroidtoolkit, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public java.lang.String getPersistedString (java.lang.String p0)
	{
		return n_getPersistedString (p0);
	}

	private native java.lang.String n_getPersistedString (java.lang.String p0);


	public boolean persistString (java.lang.String p0)
	{
		return n_persistString (p0);
	}

	private native boolean n_persistString (java.lang.String p0);

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
