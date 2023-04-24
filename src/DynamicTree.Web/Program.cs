using System.Reflection;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using NLog;
using NLog.Web;
using DynamicTree.Persistence;
using DynamicTree.SharedKernel;
using DynamicTree.SharedKernel.Exceptions.Filter;
using FluentValidation;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using IContainer = DryIoc.IContainer;
using Container = DryIoc.Container;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DynamicTree.Application;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.Contains("DynamicTree") ?? false).ToArray();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Host.UseServiceProviderFactory(new DryIocServiceProviderFactory(CreateMyPreConfiguredContainer()));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCors(options => options.AddPolicy("AllowAll",
        policyBuilder => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);

    AddAutoMapper(builder, assemblies);
    builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies));
    AddValidation(builder.Services, assemblies);

    builder.Services.AddSharedKernel();
    builder.Services.AddControllers(options => options.Filters.Add(new ApiExceptionFilter()))
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = false } };
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
#if DEBUG
            options.SerializerSettings.Formatting = Formatting.Indented;
#endif
        })
        .AddControllersAsServices();

    builder.Services.AddRazorPages();
    //builder.Services.AddSpaStaticFiles(configuration => configuration.RootPath = "wwwroot");
    builder.Services.AddSwaggerDocument();

    var app = builder.Build();

    app.UseCors("AllowAll");

    if (builder.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseRouting();
    app.UseStaticFiles();

    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();
    app.MapControllers();

    app.UseOpenApi();
    app.UseSwaggerUi3();

    app.Run();
}
catch (Exception e)
{
    logger?.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

static IContainer CreateMyPreConfiguredContainer() =>
    new Container(rules =>
        rules.With(propertiesAndFields: request =>
                request.ServiceType.Name.EndsWith("Controller")
                    ? PropertiesAndFields.Properties()(request)
                    : null)
            .WithAutoConcreteTypeResolution()
            .With(FactoryMethod.ConstructorWithResolvableArguments));

void AddAutoMapper(WebApplicationBuilder builder, Assembly[] assemblies)
{
    builder.Services.AddAutoMapper((serviceProvider, autoMapper) =>
    {
        autoMapper.AddCollectionMappers();
        autoMapper.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);

        var allTypes = assemblies.Where(a => !a.IsDynamic)
            .SelectMany(a => a.DefinedTypes)
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract)
            .ToArray();

        foreach (var type in allTypes)
            autoMapper.AddProfile(type.AsType());
    }, assemblies);

    var sp = builder.Services.CreateServiceProvider();

    using var scope = sp.CreateScope();

    logger?.Info("Start of AutoMapper warm-up...");
    _ = scope.ServiceProvider.GetRequiredService<IMapper>();
    logger?.Info("End of AutoMapper warm-up");
}

static void AddValidation(IServiceCollection services, Assembly[] assemblies)
{
    services.AddFluentValidationClientsideAdapters();
    services.AddValidatorsFromAssemblies(assemblies);
}