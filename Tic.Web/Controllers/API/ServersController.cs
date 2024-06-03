using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tic.Shared.ApiDTOs;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.API
{
    [Route("api/servers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, UserAux, Cachier")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;

        public ServersController(DataContext context, IUserHelper userHelper, IMapper mapper)
        {
            _context = context;
            _userHelper = userHelper;
            _mapper = mapper;
        }

        [HttpGet("servidor")]
        public async Task<ActionResult<List<ServerPicketDTOs>>> GetListServer()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listaServer = await _context.Servers.Where(x => x.CorporateId == user.CorporateId && x.Active == true)
                .OrderBy(x => x.ServerName)
                .ToListAsync();

            return Ok(listaServer);
        }

        [HttpGet("servidores/cachier")]
        public async Task<ActionResult<List<ServerPicketDTOs>>> GetListServerCachier()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var cajero = await _context.Cachiers.FirstOrDefaultAsync(x => x.UserName == user.UserName && x.Activo == true);
            if (cajero == null)
            {
                return BadRequest("Cajero no se Encuentra o no Esta Activo");
            }
            if (cajero.MultiServer == false)
            {
                var listaServer1 = await _context.Servers.Where(x => x.CorporateId == user.CorporateId && x.Active == true && x.ServerId == cajero.ServerId)
                    .OrderBy(x => x.ServerName)
                    .ToListAsync();
                return Ok(listaServer1);
            }
            else
            { 
                var listaServer = await _context.Servers.Where(x => x.CorporateId == user.CorporateId && x.Active == true)
                    .OrderBy(x => x.ServerName)
                    .ToListAsync();

            return Ok(listaServer);
            }
        }


        // GET: api/Servers
        [HttpGet("listaservidores")]
        public async Task<ActionResult<List<ServerIndexDTOs>>> GetServersIndex()
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
        [HttpGet("{id:int}")]
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

        [HttpGet("cargarServidor/{id:int}")]
        public async Task<ActionResult<ServerSaveDTOs>> GetCargarServidor(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var servidor = await _context.Servers.FirstOrDefaultAsync(x => x.ServerId == id);
            ServerSaveDTOs modelo = _mapper.Map<ServerSaveDTOs>(servidor);
            modelo.ClaveConfirm = servidor!.Clave;

            return Ok(modelo);
        }



        // PUT: api/Servers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer(int id, Server server)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }
            server.CorporateId = (int)user.CorporateId!;

            if (id != server.ServerId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(server);
                await _context.SaveChangesAsync();

                return Ok();

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
        }

        // POST: api/Servers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServerSaveDTOs>> PostServer(ServerSaveDTOs serverSave)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            Server modelo = _mapper.Map<Server>(serverSave);
            modelo.CorporateId = Convert.ToInt32(user.CorporateId);

            _context.Servers.Add(modelo);
            await _context.SaveChangesAsync();

            return Created();
        }

        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            try
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
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("REFERENCE"))
                {
                    return BadRequest("Existe Algun Registro Relacionado no se puede Eliminar");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool ServerExists(int id)
        {
            return _context.Servers.Any(e => e.ServerId == id);
        }
    }
}
