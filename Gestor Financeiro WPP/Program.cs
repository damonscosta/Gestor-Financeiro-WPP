using GestorFinanceiro.Application.Services;
using GestorFinanceiro.Domain.Interfaces;
using GestorFinanceiro.Infrastructure.Data;
using GestorFinanceiro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
//CONFIGURAÇÃO DO BANCO DE DADOS
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ============================================================
//INJEÇÃO DE DEPENDÊNCIA (LIGANDO AS CAMADAS DO DDD)
// Sempre que alguém pedir a Interface, entregue o Repositório real
// ============================================================
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<TransacaoAppService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioAppService>();

// Prepara a API para entender requisições HTTP (Controllers)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Uma rota de teste apenas para você abrir no navegador e ver se funcionou
app.MapGet("/", () => "🚀 API do Gestor Financeiro está rodando com sucesso!");

app.UseStaticFiles();
app.MapControllers();

app.Run();