using Microsoft.AspNetCore.Mvc.Rendering;
using Tic.Shared.EntitiesSoft;

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

        IEnumerable<SelectListItem> GetComboZone(int idCity);

        IEnumerable<SelectListItem> GetComboCorporate();

        IEnumerable<SelectListItem> GetComboTaxes(int idCorporate);

        IEnumerable<SelectListItem> GetComboIpNetwork(int idCorporate);

        IEnumerable<SelectListItem> GetComboIpNetworkUp(int idCorporate, int idIpNetwork);

        IEnumerable<SelectListItem> GetComboMark(int idCorporate);

        IEnumerable<SelectListItem> GetCombomarkModel(int CorporateId);

        IEnumerable<SelectListItem> GetComboCatPlan(int idCorporate);

        IEnumerable<SelectListItem> GetComboTimeInactive();

        IEnumerable<SelectListItem> GetComboTimeRefresh();

        IEnumerable<SelectListItem> GetComboTimeTicket();

        IEnumerable<SelectListItem> GetComboServerActivos(int idCorporate);

        IEnumerable<SelectListItem> GetComboPlanOrdenes(int idCorporate, int idServer, int idCategory);

        IEnumerable<SelectListItem> GetComboDocument(int idCorporate);

        //... Combo Cajeros
        //...
        List<Cachier> GetComboCachier(int idCorporate);






        //Sistema para Generacion automatica de Clave
        //Se pasa longitud de la clave y caracteres con la que puede hacer la clave
        string GeneratePass(int longitud, string caracteres);

        string GenerateTickets(int longitud, string caracteres);
    }
}
