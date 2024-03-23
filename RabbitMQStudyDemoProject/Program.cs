using MassTransit;
using RabbitMQStudyDemoProject;
using RabbitMQStudyDemoProject.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHostedService<RabbitBgWorker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotifyTransactionConsumer>();
    x.AddConsumer<CreateTransactionConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", c =>
        {
            //default username-password bo'lsa yozish shart emas
            c.Username("guest");
            c.Password("guest");
        });
        cfg.ReceiveEndpoint("NotifyTransactionQueue", e =>
        {
            e.ConfigureConsumer<NotifyTransactionConsumer>(context);
        });
        cfg.ReceiveEndpoint("CreateTransactionQueue", e =>
        {
            e.ConfigureConsumers(context);
        });
        cfg.ClearSerialization();
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

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
