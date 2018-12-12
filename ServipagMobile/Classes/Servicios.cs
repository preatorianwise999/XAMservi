using System;
namespace ServipagMobile {
	public class Servicios {
		// Servicios y Billers
		public string entidad;
		public string id;
		public string nombre;
		public string imagen_logo;
		//Billers
		public string descripcion_primaria_identificador;
		public string descripcion_secundaria_identificador;
		public string dias_vencimiento;
		public string ejemplo_identificador;
		public string id_servicio;
		public string imagen_boleta;
		public string nombre_servicio;
		public string id_cliente;
		public string alias_cuenta;

		public Servicios() { }
		public Servicios(string entidad, 
		                string id,
		                string nombre,
		                string imagenLogo,
		                string descPrimaria,
		                string descSecundaria,
		                string diasVencimiento,
		                string ejemploId,
		                string idServicio,
		                string imagenBoleta,
		                string nombreServicio) {
			this.entidad = entidad;
			this.id = id;
			this.nombre = nombre;
			this.imagen_logo = imagenLogo;
			this.descripcion_primaria_identificador = descPrimaria;
			this.descripcion_secundaria_identificador = descSecundaria;
			this.dias_vencimiento = diasVencimiento;
			this.ejemplo_identificador = ejemploId;
			this.id_servicio = idServicio;
			this.imagen_boleta = imagenBoleta;
			this.nombre_servicio = nombreServicio;
		}
	}
}
