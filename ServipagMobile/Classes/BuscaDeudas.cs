using System;
namespace ServipagMobile.Droid {
	public class BuscaDeudas {
		public string S;
		public string acepta_abono;
		public string acepta_casa_comercial;
		public string acepta_pago_min;
		public string acepta_prog;
		public string alias;
		public string boleta;
		public int cero;
		public string codigo_barra;
		public string codigo_tecno;
		public string codigo_tecno2;
		public string cuota;
		public string descrip_tipo_document;
		public string dias_vencimiento;
		public string direccion;
		public string direccion_factura;
		public string fecha_prog;
		public string fecha_venc;
		public string fecha_vencimiento;
		public string grafico;
		public int id_biller;
		public int id_estado_pago_solt;
		public int id_pago_solicitado;
		public string id_periodo_solicitado;
		public string id_secuencia_solicitado;
		public int id_servicio;
		public string identificador;
		public string imagen_logo;
		public string interes;
		public string logo_servicio;
		public string mensaje;
		public string mensaje_respuesta_usr;
		public string moneda;
		public int monto_minimo;
		public string monto_origen;
		public string monto_origen2;
		public string monto_original;
		public int monto_total;
		public string mostrar_cod_barra;
		public string mostrar_fecha_venc;
		public string multa;
		public string nombre_fantasia;
		public string pnd_prog;
		public string rubro;
		public string rut_biller;
		public string telefono;
		public string texto_facturador;
		public string tipo_cliente;
		public string valor_cambio;
		public string valor_cambio2;
		public string valor_uf;
		public string webpay;
		public bool isSelected;
		public bool hasSaldoAnterior;

		public BuscaDeudas() { }
		public BuscaDeudas(string S,
						string acepta_abono,
						string acepta_casa_comercial,
						string acepta_pago_min,
						string acepta_prog,
						string alias,
						string boleta,
						int cero,
						string codigo_barra,
						string codigo_tecno,
						string codigo_tecno2,
						string cuota,
						string descrip_tipo_document,
						string dias_vencimiento,
						string direccion,
						string direccion_factura,
						string fecha_prog,
						string fecha_venc,
						string fecha_vencimiento,
						string grafico,
						int id_biller,
						int id_estado_pago_solt,
						int id_pago_solicitado,
						string id_periodo_solicitado,
	                    string id_secuencia_solicitado,
						int id_servicio,
						string identificador,
						string imagen_logo,
						string interes,
						string logo_servicio,
						string mensaje,
						string mensaje_respuesta_usr,
						string moneda,
						int monto_minimo,
						string monto_origen,
						string monto_origen2,
						string monto_original,
						int monto_total,
						string mostrar_cod_barra,
						string mostrar_fecha_venc,
						string multa,
						string nombre_fantasia,
						string pnd_prog,
						string rubro,
						string rut_biller,
						string telefono,
						string texto_facturador,
						string tipo_cliente,
						string valor_cambio,
						string valor_cambio2,
						string valor_uf,
						string webpay) {

			this.S = S;
			this.acepta_abono = acepta_abono;
			this.acepta_casa_comercial = acepta_casa_comercial;
			this.acepta_pago_min = acepta_pago_min;
			this.acepta_prog = acepta_prog;
			this.alias = alias;
			this.boleta = boleta;
			this.cero = cero;
			this.codigo_barra = codigo_barra;
			this.codigo_tecno = codigo_tecno;
			this.codigo_tecno2 = codigo_tecno2;
			this.cuota = cuota;
			this.descrip_tipo_document = descrip_tipo_document;
			this.dias_vencimiento = dias_vencimiento;
			this.direccion = direccion;
			this.direccion_factura = direccion_factura;
			this.fecha_prog = fecha_prog;
			this.fecha_venc = fecha_venc;
			this.fecha_vencimiento = fecha_vencimiento;
			this.grafico = grafico;
			this.id_biller = id_biller;
			this.id_estado_pago_solt = id_estado_pago_solt;
			this.id_pago_solicitado = id_pago_solicitado;
			this.id_periodo_solicitado = id_periodo_solicitado;
			this.id_secuencia_solicitado = id_secuencia_solicitado;
			this.id_servicio = id_servicio;
			this.identificador = identificador;
			this.imagen_logo = imagen_logo;
			this.interes = interes;
			this.logo_servicio = logo_servicio;
			this.mensaje = mensaje;
			this.mensaje_respuesta_usr = mensaje_respuesta_usr;
			this.moneda = moneda;
			this.monto_minimo = monto_minimo;
			this.monto_origen = monto_origen;
			this.monto_origen2 = monto_origen2;
			this.monto_original = monto_original;
			this.monto_total = monto_total;
			this.mostrar_cod_barra = mostrar_cod_barra;
			this.mostrar_fecha_venc = mostrar_fecha_venc;
			this.multa = multa;
			this.nombre_fantasia = nombre_fantasia;
			this.pnd_prog = pnd_prog;
			this.rubro = rubro;
			this.rut_biller = rut_biller;
			this.telefono = telefono;
			this.texto_facturador = texto_facturador;
			this.tipo_cliente = tipo_cliente;
			this.valor_cambio = valor_cambio;
			this.valor_cambio2 = valor_cambio2;
			this.valor_uf = valor_uf;
			this.webpay = webpay;
			this.isSelected = true;
			this.hasSaldoAnterior = false;
		}
	}
}
