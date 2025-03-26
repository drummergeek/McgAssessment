using Microsoft.EntityFrameworkCore;

namespace McgAssessment.Data.EFInMemory;

/*
 * I've used Entity framework for the database in this file, however, this only because of time constraints.
 * I generally avoid code first database design, as data structures often don't follow code structure 1:1, especially
 * when dealing with large data sets. Data design is a very different discipline than object design. If I had the time
 * to properly design and implement the database, I would leverage SQL server, use stored procedures, normalized data,
 * and schema based security to limit the capabilities of the DAL to actually affect the data. This would ensure better
 * data integrity and leave the door open for better performance tuning.
 *
 * For now, I am using a very simplistic table structure. I did, however, separate the Patient and Document stores
 * into two separate layers, even though they are stored in the same database. This is because the document store
 * would best be served by a NoSQL implementation, or even file based storage method. SQL can be used as a document
 * store as well, easily enough, but logically these should be kept separate as far as the DAL is concerned so that
 * it could be easily spun off to another implementation if/when the application reaches scale.
 */

public class PatientDbContext : DbContext
{
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<PatientMedicalConditionEntity> PatientMedicalConditions { get; set; }
    public DbSet<PatientDocumentEntity> PatientDocuments { get; set; }

    public PatientDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseInMemoryDatabase("PatientDB");
    }
}