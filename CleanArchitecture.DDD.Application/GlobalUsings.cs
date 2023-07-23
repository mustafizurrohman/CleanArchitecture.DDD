// Global using directives

global using Bogus;
global using CleanArchitecture.DDD.Application.ExtensionMethods;
global using CleanArchitecture.DDD.Core.ExtensionMethods;
global using CleanArchitecture.DDD.Domain.ValueObjects;
global using JetBrains.Annotations;

global using Polly;
global using MediatR;
global using Serilog;
global using Hangfire;
global using AutoMapper;
global using Ardalis.GuardClauses;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper.QueryableExtensions;

global using CleanArchitecture.DDD.Core.Polly;
global using CleanArchitecture.DDD.Core.Attributes;

global using CleanArchitecture.DDD.Application.MediatR.Queries;
global using CleanArchitecture.DDD.Application.MediatR.Commands;
global using CleanArchitecture.DDD.Application.Services;
global using CleanArchitecture.DDD.Application.ServicesAggregate;
global using CleanArchitecture.DDD.Application.DTO;

global using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
global using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

global using CleanArchitecture.DDD.Domain.DTOs;

global using CleanArchitecture.DDD.Application.DTO.Internal;
global using FluentValidation;
global using Microsoft.Extensions.DependencyInjection;
global using Scrutor;

global using CleanArchitecture.DDD.Core.Helpers;
