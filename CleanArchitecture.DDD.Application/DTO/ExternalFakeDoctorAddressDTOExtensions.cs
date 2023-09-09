namespace CleanArchitecture.DDD.Application.DTO;

public static class ExternalFakeDoctorAddressDTOExtensions
{
    public static ExternalFakeDoctorAddressDTO Invalidate(this ExternalFakeDoctorAddressDTO instance)
    {
        return new ExternalFakeDoctorAddressDTO()
        {
            EDCMExternalID = (new Faker()).Random.Number(20) % 2 == 0 ? Guid.Empty : instance.EDCMExternalID,
            Firstname = instance.Firstname.InvalidateStringRandomly(),
            Lastname = instance.Lastname.InvalidateStringRandomly(),
            StreetAddress = instance.StreetAddress,
            ZipCode = instance.ZipCode,
            City = instance.City,
            Country = instance.Country
        };
    }

    public static ExternalFakeDoctorAddressDTO UpdateAddress(this ExternalFakeDoctorAddressDTO instance)
    {
        var randomNumber = (new Faker()).Random.Number(5, 10);

        return new ExternalFakeDoctorAddressDTO()
        {
            EDCMExternalID = instance.EDCMExternalID,
            Firstname = instance.Firstname,
            Lastname = instance.Lastname,
            StreetAddress = instance.StreetAddress + randomNumber,
            ZipCode = instance.ZipCode + randomNumber,
            City = instance.City + randomNumber,
            Country = instance.Country + randomNumber
        };
    }

    private static string InvalidateStringRandomly(this string inputString)
    {
        var faker = new Faker();

        var index = faker.Random.Number(100, 200);

        return (index % faker.Random.Number(2, 6) == 0)
            ? inputString
            : InvalidateString(inputString);
    }

    private static string InvalidateString(string inputString)
    {
        var faker = new Faker();

        var invalidateWithSpecialCharacters = faker.Random.Number(10, 20) % 2 == 0;

        IReadOnlyList<char> invalidCharacters = new List<char>()
        {
            '*', '?', '§', '~', '#', '`', '´'
        }.AsReadOnly();

        if (invalidateWithSpecialCharacters)
        {
            var randomInvalidCharacters = Enumerable.Range(1, faker.Random.Number(1, invalidCharacters.Count))
                .Select(_ => faker.Random.ArrayElement(invalidCharacters.ToArray()))
                .Aggregate(string.Empty, (a, b) => a.ToString() + b.ToString());

            inputString += randomInvalidCharacters;
            inputString = inputString.Randomize();
        }
        else
        {
            inputString += $"{DateTime.Now}" + "   " + faker.Random.Number(1, 10);
            inputString = inputString.Randomize();
        }

        var invalidationType = faker.Random.Number(1, 100) % 3;

        switch (invalidationType)
        {
            case 0:
                inputString = " " + inputString;
                break;
            case 1:
                inputString += "  ";
                break;
            default:
            {
                inputString = inputString.Insert(faker.Random.Number(1, inputString.Length), "  ");
                break;
            }
        }

        return inputString.Randomize().Randomize();

    }
}

