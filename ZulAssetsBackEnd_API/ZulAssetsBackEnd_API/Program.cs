#region Usings

using ZulAssetsBackEnd_API.Services;
using ZulAssetsBackEnd_API.Settings;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

#endregion

#region Declarations

var builder = WebApplication.CreateBuilder(args);


#endregion

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Cross origin Access

builder.Services.AddCors(options =>
{
    options.AddPolicy("defaultcorspolicy",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

#endregion

#region NewtonSoft JSON

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

#endregion

builder.Services.AddControllers();

#region Connection b/w Interface & Class

builder.Services.AddTransient<IMailService, MailService>();

#endregion

#region Swagger Documentation & JWT Authentication

builder.Services.AddSwaggerGen(SwaggerGen =>
{

    #region JWT Bearer Token In Swagger

    SwaggerGen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    SwaggerGen.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
               Reference = new OpenApiReference
               {
                   Type=ReferenceType.SecurityScheme,
                   Id = "Bearer"
               }
            },
            new string[]{}
        }
    });

    #endregion

    #region Swagger Documentation

    SwaggerGen.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "ZulAssets API Project",
            Description = "ZulAssets API Project for Android Application",
            Version = "v1",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Abdul Kabeer Bhatti",
                Email = "abdul.kabeer@zultec.com",
            },
        });

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

    //Error throughing due to no data in xml file
    SwaggerGen.IncludeXmlComments(filePath);

    #endregion

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = JWTBuilder.GetValidationParameters();
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if(context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token=Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger(c => 
{
    c.SerializeAsV2 = true;
});

#region Swagger URL Change

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZulAssets API Project");
    c.RoutePrefix = string.Empty;

    c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
    {
        ["activated"] = false
    };

});

#endregion

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("defaultcorspolicy");

#region JWT Authentication & Authorization

app.UseAuthentication();
app.UseAuthorization();

#endregion

app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute(name:"default",pattern:"");
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});

app.Run();
