using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Estudiante> estudiantes { get; set; }
    public DbSet<Profesor> profesores { get; set; }
    public DbSet<Clase> clases { get; set; }
    public DbSet<EstudianteClase> estudiante_clases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstudianteClase>()
            .HasKey(ec => new { ec.estudiante_id, ec.clase_id });

        modelBuilder.Entity<EstudianteClase>()
            .HasOne(ec => ec.estudiante)
            .WithMany(e => e.estudiante_clases)
            .HasForeignKey(ec => ec.estudiante_id);

        modelBuilder.Entity<EstudianteClase>()
            .HasOne(ec => ec.clase)
            .WithMany(c => c.estudiante_clases)
            .HasForeignKey(ec => ec.clase_id);
    }

    public DbSet<Usuario> usuarios { get; set; }

}
