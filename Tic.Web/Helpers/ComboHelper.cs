using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;
using Tic.Shared.Enum;
using Tic.Web.Data;

namespace Tic.Web.Helpers
{
    public class ComboHelper : IComboHelper
    {
        private readonly DataContext _context;

        public ComboHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboUserType()
        {
            List<SelectListItem> list = Enum.GetValues(typeof(UserTypeDTO)).Cast<UserTypeDTO>().Select(c => new SelectListItem()
            {
                Text = c.ToString(),
                Value = c.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Tipo Usuario...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSpeedUpType()
        {
            List<SelectListItem> list = Enum.GetValues(typeof(SpeedUpType)).Cast<SpeedUpType>().Select(c => new SelectListItem()
            {
                Text = c.ToString(),
                Value = c.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Size...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSpeedDownType()
        {
            List<SelectListItem> list = Enum.GetValues(typeof(SpeedDownType)).Cast<SpeedDownType>().Select(c => new SelectListItem()
            {
                Text = c.ToString(),
                Value = c.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Size...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSoftPlan()
        {
            List<SelectListItem> list = _context.SoftPlans.Where(t => t.Activo == true)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.SoftPlanId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Plan...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCountry()
        {
            List<SelectListItem> list = _context.Countries
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.CountryId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Pais...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboState()
        {
            List<SelectListItem> list = _context.States
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.StateId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Estado...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboState(int idcountry)
        {
            List<SelectListItem> list = _context.States
                .Where(t => t.CountryId == idcountry)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.StateId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Estado...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCity()
        {
            List<SelectListItem> list = _context.Cities
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.CityId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Ciudad...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCity(int idstate)
        {
            List<SelectListItem> list = _context.Cities.Where(x => x.StateId == idstate)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.CityId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Ciudad...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboZone(int idCity)
        {
            List<SelectListItem> list = _context.Zones.Where(x => x.CityId == idCity)
                .Select(t => new SelectListItem
                {
                    Text = t.ZoneName,
                    Value = $"{t.ZoneId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Zona...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCorporate()
        {
            List<SelectListItem> list = _context.Corporates.Where(c => c.Activo == true)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.CorporateId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Corporacion...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTaxes(int idCorporate)
        {
            List<SelectListItem> list = _context.Taxes.Where(c => c.Active == true && c.CorporateId == idCorporate)
                .Select(t => new SelectListItem
                {
                    Text = t.TaxName,
                    Value = $"{t.TaxId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Impuesto...]",
                Value = "0"
            });

            return list;
        }


        public IEnumerable<SelectListItem> GetComboIpNetwork(int idCorporate)
        {
            List<SelectListItem> list = _context.IpNetworks.Where(c => c.Active == true && c.Assigned == false
            && c.CorporateId == idCorporate)
                .Select(t => new SelectListItem
                {
                    Text = t.Ip,
                    Value = $"{t.IpNetworkId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione IP...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboIpNetworkUp(int idCorporate, int idIpNetwork)
        {
            List<SelectListItem> list = _context.IpNetworks.Where(c => c.Active == true && c.Assigned == false
            && c.CorporateId == idCorporate || c.IpNetworkId == idIpNetwork)
                .Select(t => new SelectListItem
                {
                    Text = t.Ip,
                    Value = $"{t.IpNetworkId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione IP...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboMark(int idCorporate)
        {
            List<SelectListItem> list = _context.Marks.Where(c => c.Active == true && c.CorporateId == idCorporate)
                .Select(t => new SelectListItem
                {
                    Text = t.MarkName,
                    Value = $"{t.MarkId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Marca...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetCombomarkModel(int CorporateId)
        {
            List<SelectListItem> list = _context.MarkModels.Where(c => c.Active == true && c.CorporateId == CorporateId)
                .Select(t => new SelectListItem
                {
                    Text = t.MarkModelName,
                    Value = $"{t.MarkModelId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Modelo...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCatPlan(int idCorporate)
        {
            List<SelectListItem> list = _context.PlanCategories.Where(c => c.Active == true && c.CorporateId == idCorporate)
                .Select(t => new SelectListItem
                {
                    Text = t.PlanCategoryName,
                    Value = $"{t.PlanCategoryId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Categoria...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTimeInactive()
        {
            List<SelectListItem> list = _context.TicketInactives.Where(c => c.Activo == true).OrderBy(x => x.Orden)
                .Select(t => new SelectListItem
                {
                    Text = t.Tiempo,
                    Value = $"{t.TicketInactiveId}"
                })
            .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Tiempo Inactivo...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTimeRefresh()
        {
            List<SelectListItem> list = _context.TicketRefreshes.Where(c => c.Activo == true).OrderBy(x => x.Orden)
                .Select(t => new SelectListItem
                {
                    Text = t.Tiempo,
                    Value = $"{t.TicketRefreshId}"
                })
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Tiempo Refresh...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTimeTicket()
        {
            List<SelectListItem> list = _context.TicketTimes.Where(c => c.Activo == true).OrderBy(x => x.Orden)
                .Select(t => new SelectListItem
                {
                    Text = t.Tiempo,
                    Value = $"{t.TicketTimeId}"
                })
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Ticket Tiempo...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboServerActivos(int idCorporate)
        {
            List<SelectListItem> list = _context.Servers.Where(c => c.CorporateId == idCorporate && c.Active == true)
                .Select(t => new SelectListItem
                {
                    Text = t.ServerName,
                    Value = $"{t.ServerId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Servidor...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPlanOrdenes(int idCorporate, int idServer, int idCategory)
        {
            List<SelectListItem> list = _context.Plans.Where(c => c.CorporateId == idCorporate && c.Active == true &&
            c.ServerId == idServer && c.PlanCategoryId == idCategory)
                .Select(t => new SelectListItem
                {
                    Text = t.PlanName,
                    Value = $"{t.PlanId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Plan...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDocument(int idCorporate)
        {
            List<SelectListItem> list = _context.DocumentTypes.Where(c => c.Active == true && c.CorporateId == idCorporate)
                .Select(t => new SelectListItem
                {
                    Text = t.DocumentName,
                    Value = $"{t.DocumentTypeId}"
                })
            .OrderBy(t => t.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione Tipo...]",
                Value = "0"
            });

            return list;
        }


        //... Combo Cajeros
        //...
        public List<Cachier> GetComboCachier(int idCorporate)
        {
            var datos = _context.Cachiers
                .Where(c => c.Activo == true && c.CorporateId == idCorporate)
                .ToList();
            datos.Add(new Cachier
            {
                CachierId = 0,
                FullName = "[Debe Seleccionar un Cajero...]"
            });
            return datos.OrderBy(o => o.FullName).ToList();
        }

        //Sistema para Generacion automatica de Clave
        //Se pasa longitud de la clave y caracteres con la que puede hacer la clave
        public string GeneratePass(int longitud, string caracteres)
        {
            StringBuilder res = new();
            Random rnd = new();
            while (0 < longitud--)
            {
                res.Append(caracteres[rnd.Next(caracteres.Length)]);
            }
            return res.ToString();
        }

        public string GenerateTickets(int longitud, string caracteres)
        {
            StringBuilder res = new();
            Random rnd = new();
            while (0 < longitud--)
            {
                res.Append(caracteres[rnd.Next(caracteres.Length)]);
            }
            return res.ToString();
        }
    }
}
