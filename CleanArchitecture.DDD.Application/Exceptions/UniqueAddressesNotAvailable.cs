namespace CleanArchitecture.DDD.Application.Exceptions;

public class UniqueAddressesNotAvailable : ApplicationException
{
    private readonly int _needed;
    private readonly int _have;
    private readonly int _moreNeeded;

    public UniqueAddressesNotAvailable(int needed, int have)
    {
        _needed = needed;
        _have = have;
        _moreNeeded = _needed - _have;
    }

    public override string Message => $"Sufficient unique addresses not available. Needed {_needed}, have {_have}. " +
                                      $"Please seed at-least {_moreNeeded} addresses first or try with a maximum of {_have} number of doctors.";
}