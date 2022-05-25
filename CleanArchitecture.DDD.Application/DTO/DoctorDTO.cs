using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.DDD.Domain.ValueObjects;

namespace CleanArchitecture.DDD.Application.DTO;

public class DoctorDTO
{
    public Name Name { get; init; }

    public AddressDTO Address { get; init; }
    
}