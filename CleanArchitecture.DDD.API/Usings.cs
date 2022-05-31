global using Serilog;
global using CleanArchitecture.DDD.API.ExtensionMethods;
global using CleanArchitecture.DDD.Core;
global using CleanArchitecture.DDD.Domain.ValueObjects;
global using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.EntityFrameworkCore;
global using MediatR;

global using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
global using Microsoft.AspNetCore.Mvc;
global using AutoMapper.QueryableExtensions;
global using CleanArchitecture.DDD.Domain.DTOs;

global using System.Diagnostics;
global using System.Net.Mime;
global using Ardalis.GuardClauses;
global using Bogus;

global using AutoMapper;
global using CleanArchitecture.DDD.Application.MediatR.Commands;
global using CleanArchitecture.DDD.Application.Services;