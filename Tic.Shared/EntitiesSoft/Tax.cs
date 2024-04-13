﻿using Tic.Shared.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic.Shared.EntitiesSoft
{
    public class Tax
    {
        public int TaxId { get; set; }

        [MaxLength(50, ErrorMessage = "El Maximo de caracteres es {0}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Impuesto")]
        public string TaxName { get; set; } = null!;

        [Range(0, 99, ErrorMessage = "EL Valor del {0} debe estar entre {1} y {2}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Tasa")]
        public decimal Rate { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }


        //A que Corporacion Pertenece
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }

    }
}
