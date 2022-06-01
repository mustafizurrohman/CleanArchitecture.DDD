namespace CleanArchitecture.DDD.Application.Exceptions;

public class UniqueAddressesNotAvailable : ApplicationException
{
    private readonly int _needed;
    private readonly int _have;

    public UniqueAddressesNotAvailable(int needed, int have)
    {
        _needed = needed;
        _have = have;
    }

    public override string Message => $"Sufficient unique addresses not available. Needed {_needed}, have {_have}. Please seed addresses first or try with a lower number of doctors.";
}