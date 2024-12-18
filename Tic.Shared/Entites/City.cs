﻿using System.ComponentModel.DataAnnotations;
using Tic.Shared.EntitiesSoft;

namespace Tic.Shared.Entites
{
    public class City
    {
        public int CityId { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int StateId { get; set; }

        public State? State { get; set; }

        //Relacioens en doble via
        public ICollection<Corporate>? Corporates { get; set; }

        public ICollection<Zone>? Zones { get; set; }
    }
}
