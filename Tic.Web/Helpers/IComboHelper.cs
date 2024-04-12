using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tic.Web.Helpers
{
    public interface IComboHelper
    {
        //Combos apartir de los ENUM
        IEnumerable<SelectListItem> GetComboUserType();

        IEnumerable<SelectListItem> GetComboSpeedUpType();

        IEnumerable<SelectListItem> GetComboSpeedDownType();

        IEnumerable<SelectListItem> GetComboSoftPlan();

        IEnumerable<SelectListItem> GetComboCountry();

        IEnumerable<SelectListItem> GetComboState();

        IEnumerable<SelectListItem> GetComboState(int idcountry);

        IEnumerable<SelectListItem> GetComboCity();

        IEnumerable<SelectListItem> GetComboCity(int idstate);

        IEnumerable<SelectListItem> GetComboCorporate();


        //Sistema para Generacion automatica de Clave
        //Se pasa longitud de la clave y caracteres con la que puede hacer la clave
        string GeneratePass(int longitud, string caracteres);
    }
}
