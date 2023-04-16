using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

public class PatientMasterData
{

    public string PrimaryDoctor { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool Active { get; set; }
}

