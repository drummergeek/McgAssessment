using McgAssessment.Data;
using McgAssessment.Data.EFInMemory;
using McgAssessment.Data.LocalFileSystem;
using McgAssessment.PatientService;
using McgAssessment.PatientService.Server;
using McgAssessment.Security;
using McgAssessment.Security.Simple;
using McgAssessment.UserService;
using McgAssessment.UserService.Client;

var builder = WebApplication.CreateBuilder(args);

// Bootstrap DI
builder.Services.AddSingleton<ISecurityProvider, SimpleSecurityProvider>();
builder.Services.AddSingleton<IUserTokenService, UserTokenServiceClient>();
builder.Services.AddSingleton<PatientDbContext>(); // Setup as singleton to simplify seeding initial data. Real EF implementation would be Scoped instead.
builder.Services.AddSingleton<IPatientDataStore, PatientDataStore>();
builder.Services.AddSingleton<IDocumentStore>(p => ActivatorUtilities.CreateInstance<DocumentFileSystemStore>(p, @".\storage\documents\"));
builder.Services.AddScoped<IPatientService, PatientServiceImpl>();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await SeedInitialData(app.Services.GetService<IPatientDataStore>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();



async Task SeedInitialData(IPatientDataStore patientDataStore)
{
    await patientDataStore.CreatePatientAsync(new Patient
    {
        Id = "P0001",
        FirstName = "John",
        LastName = "Doe",
        Email = "johndoe@gmail.com",
        DateOfBirth = new DateTime(1980, 9, 29),
        Address = "123 Main Street",
        City = "Seattle",
        State = "WA",
        Zip = "98322",
        Country = "USA",
        Phone = "+13335551212",
        Gender = "Male",
    });
    
    await patientDataStore.UpdatePatientMedicalConditionsAsync("P0001", new [] {"Coronary Artery Disease", "Hypertension"});
    await patientDataStore.AttachPatientDocumentAsync("P0001", "DOC00001", "Scan-20250204-JDoe.pdf",
        PatientDocumentType.CATScan);
    await patientDataStore.AttachPatientDocumentAsync("P0001", "DOC00002", "MRI-JDoe.pdf",
        PatientDocumentType.MRI);
    await patientDataStore.AttachPatientDocumentAsync("P0001", "DOC00003", "Notes-20250210.txt",
        PatientDocumentType.ProviderNote);
    
    await patientDataStore.CreatePatientAsync(new Patient
    {
        Id = "P0002",
        FirstName = "Jane",
        LastName = "Smith",
        Email = "janesmith@gmail.com",
        DateOfBirth = new DateTime(1976, 3, 12),
        Address = "123 Other Street",
        City = "Vancouver",
        State = "WA",
        Zip = "98321",
        Country = "USA",
        Phone = "+13335551314",
        Gender = "Female",
    });
    
    await patientDataStore.UpdatePatientMedicalConditionsAsync("P0002", new [] {"Arthritis", "Asthma"});
    await patientDataStore.AttachPatientDocumentAsync("P0002", "DOC00004", "Scan-20220114-JSmith.pdf",
        PatientDocumentType.CATScan);
    await patientDataStore.AttachPatientDocumentAsync("P0002", "DOC00005", "EKG-JSmith.pdf",
        PatientDocumentType.Other);
    await patientDataStore.AttachPatientDocumentAsync("P0002", "DOC00006", "Notes-20231012.txt",
        PatientDocumentType.ProviderNote);
}