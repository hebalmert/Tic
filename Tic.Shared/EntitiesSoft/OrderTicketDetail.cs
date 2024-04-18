using System.ComponentModel.DataAnnotations;
using Tic.Shared.Entites;

namespace Tic.Shared.EntitiesSoft
{
    public class OrderTicketDetail
    {
        public int OrderTicketDetailId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Order Ticket")]
        public int OrderTicketId { get; set; }

        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Ticket #")]
        public int Control { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Creado")]
        public DateTime DateCreado { get; set; }


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Informacion del Servidor, Velocidad y PIN del Ticket
        [Required(ErrorMessage = "La {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Servidor")]
        public int ServerId { get; set; }

        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Velocidad")]
        public string? Velocidad { get; set; }

        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Tiket:")]
        public string? Usuario { get; set; }

        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Clave")]
        public string? Clave { get; set; }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Si fue venta anotamos si fue Administrador o Cajero
        [Display(Name = "Usuarios")]
        public bool UserSystem { get; set; }

        [Display(Name = "Cajero")]
        public bool UserCachier { get; set; }

        [Display(Name = "Usuario")]
        public int? ManagerId { get; set; }

        [Display(Name = "Cajero")]
        public int? CachierId { get; set; }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Se Marca si el ticket esta vendido  se Agrega Fecha de Venta
        [Display(Name = "Vendido")]
        public bool Vendido { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Venta")]
        public DateTime? DateVenta { get; set; }

        [Display(Name = "Venta General")]
        public bool SellTotal { get; set; }

        [Display(Name = "Venta Admin")]
        public bool SellOne { get; set; }

        [Display(Name = "Venta Cajero")]
        public bool SellOneCachier { get; set; }

        [Display(Name = "Venta Id")]
        public int? VentaId { get; set; }


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Se Marca si el ticket esta Anulado
        [Display(Name = "Anulado")]
        public bool Anulado { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Anulado")]
        public DateTime? DateAnulado { get; set; }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //Se Agrega el Index de Mikrotik para el Ticket.
        [MaxLength(25, ErrorMessage = "El Maximo de caracteres es {1}")]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [Display(Name = "Mk Id")]
        public string? MkId { get; set; }

        //Relacion de datos

        //...
        public int CorporateId { get; set; }

        public Corporate? Corporate { get; set; }


        public OrderTicket? OrderTickets { get; set; }

        public ICollection<SellOne>? SellOnes { get; set; }

        ////..
        //[Display(Name = "Venta Pack")]
        //public ICollection<SellPackDetail> SellPackDetails { get; set; }

        ////..
        //[Display(Name = "Cajero")]
        //public ICollection<SellOneCachier> SellOneCachiers { get; set; }

        ////..
        //[Display(Name = "Comisiones Cajero")]
        //public ICollection<CachierPorcent> CachierPorcents { get; set; }
    }
}
