using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public sealed class Patient 
    : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid PatientID { get; set; }
    
    // Primitive Obsession
    public string Firstname { get; set; }

    public string Lastname { get; set; }

    // JSON Column
    public PatientMasterData MasterData { get; set; }

    [NotMapped]
    public string Fullname => (Firstname + " " + Lastname).RemoveConsecutiveSpaces();

}