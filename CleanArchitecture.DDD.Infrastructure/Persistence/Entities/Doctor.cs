using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.DDD.Domain.ValueObjects;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public sealed class Doctor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int DoctorID { get; set; }

    public Firstname Firstname { get; set; } 

}