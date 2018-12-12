using System;
namespace ServipagMobile {
	public class MediosPago {
		public string descripcion;
		public int forma_pago;
		public int id_banco;
		public string logo_banco;
		public string orden;
		public string url_banco;
		public string valor_parametro_banco;
		public string valor_popup;
        public string principalColor;
        public string navigationBarTextTint;
        public string darkerPrincipalColor;
        public string secondaryColor;
        public string mainButtonStyle;
        public string hideWebAddressInformationInForm;
        public string useBarCenteredLogoInForm;
        public string font;
        public string switcher;
		
        public MediosPago(string desc, int fp, int ib, string lb, string orden, string ub, string vpb, string vp, string principalColor, string navigationBarTextTint,
                         string darkerPrincipalColor, string secondaryColor, 
                          string mainButtonStyle, string hideWebAddressInformationInForm, 
                          string useBarCenteredLogoInForm, string font 
                         ,string switcher
                         ) {
			this.descripcion = desc;
			this.forma_pago = fp;
			this.id_banco = ib;
			this.logo_banco = lb;
			this.orden = orden;
			this.url_banco = ub;
			this.valor_parametro_banco = vpb;
			this.valor_popup = vp;
            this.principalColor = principalColor;
            this.navigationBarTextTint = navigationBarTextTint;
            this.darkerPrincipalColor = darkerPrincipalColor;
            this.secondaryColor = secondaryColor;
            this.mainButtonStyle = mainButtonStyle;
            this.hideWebAddressInformationInForm = hideWebAddressInformationInForm;
            this.useBarCenteredLogoInForm = useBarCenteredLogoInForm;
            this.font = font;
            this.switcher = switcher;
		}
	}
}
