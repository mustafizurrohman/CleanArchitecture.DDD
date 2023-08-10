﻿using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using System.Threading;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

public class PatientMasterData
{
    public string PrimaryDoctor { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool Active { get; set; }

    private static PatientMasterData Create(string primaryDoctor, DateTime dateOfBirth, bool active)
    {
        return new PatientMasterData()
        {
            PrimaryDoctor = primaryDoctor,
            DateOfBirth = dateOfBirth,
            Active = active
        };
    }

    public static PatientMasterData CreateRandom(DomainDbContext context)
    {
        var primaryDoctor = context
            .Doctors
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .FirstOrDefault() ?? string.Empty;

        var active = DateTime.Now.Ticks % 2 == 0;
        
        return Create(primaryDoctor, RandomDay(), active);
    }

    public static List<PatientMasterData> CreateRandom(DomainDbContext context, int num)
    {
        var primaryDoctors = context
            .Doctors
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .Take(num)
            .ToList();

        var patientMasterDataList = new List<PatientMasterData>();

        foreach (var primaryDoctor in primaryDoctors)
        {
            var active = DateTime.Now.Ticks % 2 == 0;

            patientMasterDataList.Add(Create(primaryDoctor, RandomDay(), active));
        }

        return patientMasterDataList;

    }

    private static DateTime RandomDay()
    {
        var now = DateTime.Now;
        var start = now.AddYears(-100);
        var range = (now - start).Days;
        return start.AddDays(new Random().Next(range));
    }
}

