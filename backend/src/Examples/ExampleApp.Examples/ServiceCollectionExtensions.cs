using System.Globalization;
using Azure.Core;
using ExampleApp.Examples.Configuration;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.DataAccess.Serialization;
using ExampleApp.Examples.Handlers.Identities;
using LeanCode.AuditLogs;
using LeanCode.AzureIdentity;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Model;
using LeanCode.Kratos.Client.Api;
using LeanCode.Kratos.Client.Client;
using LeanCode.Kratos.Client.Extensions;
using LeanCode.Kratos.Client.Model;
using LeanCode.Npgsql.ActiveDirectory;
using LeanCode.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static ExampleApp.Examples.Contracts.Auth;
#if Example
using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.DataAccess.Repositories;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ServiceProvider = ExampleApp.Examples.Domain.Booking.ServiceProvider;
#endif

namespace ExampleApp.Examples;

public static class ServiceCollectionExtensions { }
