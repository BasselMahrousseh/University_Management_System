using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Core.Repositories.Security;
using Univ_Manage.Core.Repositories.Supervision;
using Univ_Manage.Core.Repositories.Univ;
using Univ_Manage.Infrastructure.Models.Security;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SqlServer.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<Univ_ManageDBContext>(x =>
                      x.UseSqlServer(builder.Configuration.GetValue<string>("Local:ConnectionString")
                      , sqlDbOptions => sqlDbOptions.MigrationsAssembly("Univ_Manage.Infrastructure")));
builder.Services.AddIdentity<UserSet, RoleSet>()
          .AddEntityFrameworkStores<Univ_ManageDBContext>()
          .AddDefaultTokenProviders();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<ISchedulingRepository, SchedulingRepository>();
builder.Services.AddTransient<IExamRepository, ExamRepository>();
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<ISemesterRepository, SemesterRepository>();
builder.Services.AddTransient<IDepartmentRepository,DepartmentRepository>();
await DataSeed.SeedDataAsync(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=LogIn}/{id?}");

app.Run();
