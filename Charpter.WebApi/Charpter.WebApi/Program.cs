using Charpter.WebApi.Contexts;
using Charpter.WebApi.Interfaces;
using Charpter.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1", Title = "CharpterWebApi"});
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultChallengeScheme = "JwtBearer";
    o.DefaultAuthenticateScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("bomba-nuclear-autorizacao")),
        ClockSkew = TimeSpan.FromMinutes(60),
        ValidIssuer = "charpter.webapi",
        ValidAudience = "charpter.webapi"
    };
});

builder.Services.AddScoped<CharpterContext, CharpterContext>();

builder.Services.AddTransient<LivroRepository, LivroRepository>();

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CharpterWebApi");
    c.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
 {
     endpoints.MapControllers();
 });

app.Run();
