using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //Modelos de la parte Administrador
        public DbSet<City> Cities => Set<City>();

        public DbSet<Country> Countries => Set<Country>();

        public DbSet<State> States => Set<State>();

        public DbSet<SoftPlan> SoftPlans => Set<SoftPlan>();

        public DbSet<Manager> Managers => Set<Manager>();

        public DbSet<Corporate> Corporates => Set<Corporate>();

        public DbSet<TicketInactive> TicketInactives => Set<TicketInactive>();

        public DbSet<TicketRefresh> TicketRefreshes => Set<TicketRefresh>();

        public DbSet<TicketTime> TicketTimes => Set<TicketTime>();


        //Modelos de la parte de Los Usuarios

        public DbSet<Register> Registers => Set<Register>();

        public DbSet<Tax> Taxes => Set<Tax>();

        public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();

        public DbSet<ChainCode> ChainCodes => Set<ChainCode>();

        public DbSet<Zone> Zones => Set<Zone>();

        public DbSet<Mark> Marks => Set<Mark>();

        public DbSet<MarkModel> MarkModels => Set<MarkModel>();

        public DbSet<IpNetwork> IpNetworks => Set<IpNetwork>();

        public DbSet<Server> Servers => Set<Server>();

        public DbSet<PlanCategory> PlanCategories => Set<PlanCategory>();

        public DbSet<Plan> Plans => Set<Plan>();

        public DbSet<OrderTicket> OrderTickets => Set<OrderTicket>();

        public DbSet<OrderTicketDetail> OrderTicketDetails => Set<OrderTicketDetail>();

        public DbSet<Cachier> Cachiers => Set<Cachier>();

        public DbSet<SellOne> SellOnes => Set<SellOne>();

        public DbSet<SellPack> SellPacks => Set<SellPack>();

        public DbSet<SellPackDetail> SellPackDetails => Set<SellPackDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Para tomar los calores de ConfigEntities
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            //De esta manera podemos crear un regla general para un tipo de Dato
            //sera automatico.
            //configurationBuilder.Properties<string>().HaveMaxLength(50);
        }
    }
}
