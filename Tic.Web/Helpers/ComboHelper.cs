using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Tic.Shared.Enum;
using Tic.Web.Data;

namespace Tic.Web.Helpers
{
    public class ComboHelper :IComboHelper
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
    }
}
