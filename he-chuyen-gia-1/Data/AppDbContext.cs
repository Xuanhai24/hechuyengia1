using hechuyengia.Models;
using hechuyengia.Models.DiseaseDiagnose;
using Microsoft.EntityFrameworkCore;

namespace hechuyengia.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
        //public DbSet<Disease> Diseases => Set<Disease>();
        //public DbSet<Symptom> Symptoms => Set<Symptom>();
        //public DbSet<DiseaseSymptom> DiseaseSymptoms => Set<DiseaseSymptom>();
        //public DbSet<Rule> PrologRules => Set<Rule>();
        public DbSet<DiagnoseResult> DiagnoseResults => Set<DiagnoseResult>();
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // Email unique cho User
            mb.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique();

            // Disease - Symptom (N-N)
            //mb.Entity<DiseaseSymptom>()
            //  .HasKey(ds => new { ds.DiseaseId, ds.SymptomId });

            //mb.Entity<DiseaseSymptom>()
            //  .HasOne(ds => ds.Disease)
            //  .WithMany(d => d.DiseaseSymptoms)
            //  .HasForeignKey(ds => ds.DiseaseId);

            //mb.Entity<DiseaseSymptom>()
            //  .HasOne(ds => ds.Symptom)
            //  .WithMany(s => s.DiseaseSymptoms)
            //  .HasForeignKey(ds => ds.SymptomId);

            // User - Doctor (1-1), xoá User sẽ xoá luôn Doctor
            mb.Entity<Doctor>()
              .HasOne(d => d.User)
              .WithOne(u => u.Doctor)
              .HasForeignKey<Doctor>(d => d.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            // Diagnosis - Patient (1-N), xoá Patient xoá luôn chẩn đoán
            mb.Entity<Diagnosis>()
              .HasOne(d => d.Patient)
              .WithMany(p => p.Diagnoses)
              .HasForeignKey(d => d.PatientId)
              .OnDelete(DeleteBehavior.Cascade);

            // Diagnosis - Doctor (1-N), không cho xoá Doctor nếu còn chẩn đoán
            mb.Entity<Diagnosis>()
              .HasOne(d => d.Doctor)
              .WithMany(doc => doc.Diagnoses)
              .HasForeignKey(d => d.DoctorId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
