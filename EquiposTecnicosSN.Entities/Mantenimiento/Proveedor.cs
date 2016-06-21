﻿using EquiposTecnicosSN.Entities.Equipos.Info;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquiposTecnicosSN.Entities.Mantenimiento
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        [Required]
        public int ProveedorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public ProveedorTipo Tipo { get; set; }

        [DisplayName("Dirección")]
        [StringLength(255)]
        public string Direccion { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(50)]
        public string Telefono { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        [DisplayName("Email")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Servicios { get; set; }
    }
}
