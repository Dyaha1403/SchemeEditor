using Microsoft.EntityFrameworkCore;
using SchemeEditor.Entities;


namespace SchemeEditor
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SchemeDTO> Schemes { get; set; }
        public DbSet<CanvasItemDTO> CanvasItems { get; set; }
        public DbSet<ConnectorDTO> Connectors { get; set; }
        public DbSet<ControlDTO> Controls { get; set; }
        public DbSet<ConnectionDTO> Connections { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SchemeEditor.db");
        }
    }
}
