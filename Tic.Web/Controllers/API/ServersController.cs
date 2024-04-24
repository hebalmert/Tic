using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tic.Shared.ApiDTOs;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.API
{
    [Route("api/servers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="User")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ServersController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: api/Servers
        [HttpGet]
        public async Task<ActionResult<List<ServerIndexDTOs>>> GetServers()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listServer = (from sv in _context.Servers
                              join ip in _context.IpNetworks on sv.IpNetworkId equals ip.IpNetworkId
                              join mk in _context.Marks on sv.MarkId equals mk.MarkId
                              join md in _context.MarkModels on sv.MarkModelId equals md.MarkModelId
                              join zn in _context.Zones on sv.ZoneId equals zn.ZoneId
                              where sv.CorporateId == user.CorporateId
                              select new ServerIndexDTOs
                              {
                                  ServerId = sv.ServerId,
                                  ServerName = sv.ServerName,
                                  IpNetwork = ip.Ip!,
                                  Usuario = sv.Usuario,
                                  Clave = sv.Clave,
                                  ApiPort = sv.ApiPort,
                                  Zona = zn.ZoneName,
                                  Activo = sv.Active == true ? "On" : "Off",
                                  CorporateId = sv.CorporateId
                              }).OrderBy(x => x.ServerName).ToList();

            return listServer;
        }

        // GET: api/Servers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDTOs>> GetServer(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var Server = (from sv in _context.Servers
                              join ip in _context.IpNetworks on sv.IpNetworkId equals ip.IpNetworkId
                              join mk in _context.Marks on sv.MarkId equals mk.MarkId
                              join md in _context.MarkModels on sv.MarkModelId equals md.MarkModelId
                              join zn in _context.Zones on sv.ZoneId equals zn.ZoneId
                              where sv.CorporateId == user.CorporateId && sv.ServerId == id
                              select new ServerDTOs
                              {
                                  ServerId = sv.ServerId,
                                  ServerName = sv.ServerName,
                                  IpNetwork = ip.Ip!,
                                  Usuario = sv.Usuario,
                                  Clave = sv.Clave,
                                  ApiPort = sv.ApiPort,
                                  WanName = sv.WanName,
                                  Marka = mk.MarkName,
                                  MarkModelo = md.MarkModelName,
                                  Estado = zn.State!.Name,
                                  Ciudad = zn.City!.Name,
                                  Zona = zn.ZoneName,
                                  Activo = sv.Active == true ? "On" : "Off",
                                  CorporateId = sv.CorporateId
                              }).First();

            return Server!;
        }


        // PUT: api/Servers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer(int id, Server server)
        {
            if (id != server.ServerId)
            {
                return BadRequest();
            }

            _context.Entry(server).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Servers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Server>> PostServer(Server server)
        {
            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServer", new { id = server.ServerId }, server);
        }

        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(int id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServerExists(int id)
        {
            return _context.Servers.Any(e => e.ServerId == id);
        }
    }
}
