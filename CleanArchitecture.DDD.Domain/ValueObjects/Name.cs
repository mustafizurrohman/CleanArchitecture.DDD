﻿ using CleanArchitecture.DDD.Core.ExtensionMethods;
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

     public static Name Create(string firstName, string lastName)
     {
         return Create(firstName, string.Empty, lastName);
     }

     public static Name Create(string firstName, string middleName, string lastName)
     {
         var newName = new Name
         {
             Firstname = firstName,
             Middlename = middleName,
             Lastname = lastName
         };

         Validate(newName);

         return newName;
     }

     public override string ToString()
     {
         return (Firstname + " " + Middlename + " " + Lastname).RemoveConsequtiveSpaces();
     }
       
     public static Name Copy(Name name)
     {
         Validate(name);
         return new Name(name.Firstname, name.Middlename ?? string.Empty, name.Lastname);
     }

     private static void Validate(Name name)
     {
         var validationResult = new NameValidator().Validate(name);

         if (!validationResult.IsValid)
             throw new NameValidationException(validationResult.Errors);
     }

 }