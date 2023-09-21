 using Bogus;
 using CleanArchitecture.DDD.Core.ExtensionMethods;
 using CleanArchitecture.DDD.Domain.Exceptions;

 namespace CleanArchitecture.DDD.Domain.ValueObjects; 

 public record Name(string Firstname, string? Middlename, string Lastname) 
 {
     protected Name(string firstname, string lastname) : this(firstname, string.Empty, lastname)
     {
     }

     protected Name() : this(string.Empty, string.Empty, string.Empty)
     {
     }

     // ReSharper disable once UseDeconstructionOnParameter
     public Name(Name name)
     {
         Create(name.Firstname, name.Middlename ?? string.Empty, name.Lastname);
     }

     public static Name Create(string firstName, string lastName, bool validate = true)
     {
         return Create(firstName, string.Empty, lastName, validate);
     }

     public static Name Create(string firstName, string middleName, string lastName, bool validate = true)
     {
         var newName = new Name
         {
             Firstname = firstName,
             Middlename = middleName,
             Lastname = lastName
         };

         if (validate)
            Validate(newName);

         return newName;
     }

     public static Name CreateRandom()
     { 
         Faker faker = new();
         var name = Name.Create(faker.Name.FirstName(), faker.Name.LastName());

         return name;
     }

     public override string ToString()
     {
         return (Firstname + " " + Middlename + " " + Lastname).RemoveConsecutiveSpaces();
     }
       
     public static Name Copy(Name name, bool validate = true)
     {
         if (validate)
            Validate(name);

         // ReSharper disable once UseWithExpressionToCopyRecord
         return new Name(name.Firstname, name.Middlename ?? string.Empty, name.Lastname);
     }

     private static void Validate(Name name)
     {
         var validationResult = new NameValidator().Validate(name);

         if (!validationResult.IsValid)
             throw new NameValidationException(validationResult.Errors);
     }

 }