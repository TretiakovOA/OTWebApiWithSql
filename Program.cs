using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Repositories;
using OTWebApiWithSql.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICreditApplicationRepository, CreditApplicationRepository>();
builder.Services.AddScoped<ICreditProductRepository, CreditProductRepository>();
builder.Services.AddScoped<ICallRepository, CallRepository>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<ICallAssignmentRepository, CallAssignmentRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICreditApplicationService, CreditApplicationService>();
builder.Services.AddScoped<ICreditProductService, CreditProductService>();
builder.Services.AddScoped<ICallService, CallService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
