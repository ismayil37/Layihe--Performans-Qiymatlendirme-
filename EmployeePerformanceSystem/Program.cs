using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. Servisləri əlavə et
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS icazəsi
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// --- STATİK FAYLLAR ÜÇÜN ƏLAVƏLƏR ---
// Bu sətir brauzer açılan kimi avtomatik index.html faylını axtarır
app.UseDefaultFiles();
// Bu sətir wwwroot qovluğundakı faylları (html, js, css) xidmətə verir
app.UseStaticFiles();
// ------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();