using Microsoft.EntityFrameworkCore;
using primorye.Models;

namespace primorye.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<SolutionToTheCase> Solutions => Set<SolutionToTheCase>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Variant> Variants => Set<Variant>();
        public DbSet<TournamentTable> TournamentTables => Set<TournamentTable>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Login).HasColumnName("login");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.TeamId).HasColumnName("id_team");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Finance).HasColumnName("finance");
                entity.Property(e => e.SocialPoints).HasColumnName("social_points");
                entity.Property(e => e.Progress).HasColumnName("progress");
                entity.Property(e => e.PlaceInTheTable).HasColumnName("place_in_the_table");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("incidents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Difficulty).HasColumnName("difficulty");
                entity.Property(e => e.CityId).HasColumnName("id_city");
            });

            modelBuilder.Entity<SolutionToTheCase>(entity =>
            {
                entity.ToTable("solution_to_the_case");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.PublicOpinion).HasColumnName("public_opinion");
                entity.Property(e => e.Progress).HasColumnName("progress");
                entity.Property(e => e.IdIncidents).HasColumnName("id_incidents");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("questions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Text).HasColumnName("question_text");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.DifficultyLevel).HasColumnName("difficulty_level");
                entity.Property(e => e.CityId).HasColumnName("id_city");
            });

            modelBuilder.Entity<Variant>(entity =>
            {
                entity.ToTable("variants");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.IsCorrect).HasColumnName("correct");
                entity.Property(e => e.QuestionId).HasColumnName("id_question");
            });

            modelBuilder.Entity<TournamentTable>(entity =>
            {
                entity.ToTable("tournament_table");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TeamId).HasColumnName("id_team");
                entity.Property(e => e.CityId).HasColumnName("id_city");
            });
        }
    }
}