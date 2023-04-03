using EMS.SqlServer.Seed.IdentitySeed.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Univ_Manage.Infrastructure.Models.Univ;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.Enums;
using Univ_Manage.SqlServer.Seed.IdentitySeed.Services;

namespace Univ_Manage.SqlServer.Seed
{
    public static class DataSeed
    {
        public static IServiceCollection Services { get; set; }

        private static void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            Services.AddScoped<IUserCreationService, UserCreationService>();
            Services.AddScoped<IRoleCreationService, RoleCreationService>();

        }

        public async static Task EnsureDatabaseExistAsync(Univ_ManageDBContext context)
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        public async static Task<bool> SeedDataAsync(IServiceCollection services)
        {
            ConfigureServices(services);

            var provider = Services.BuildServiceProvider();
            var context = provider.GetService<Univ_ManageDBContext>();

            await EnsureDatabaseExistAsync(context);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!context.Users.Any())
                    {
                        await SeedIdentityAsync(provider);
                    }
                    if (!context.Years.Any())
                    {
                        await SeedYearAsync(context);
                    }
                    if (!context.Semesters.Any())
                    {
                        await SeedSemesterAsync(context);
                    }
                    if (!context.Departments.Any())
                    {
                        await SeedDepartmentsAsync(context);
                    }
                    if (!context.Subjects.Any())
                    {
                        await SeedSubjectsAsync(context);
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();

                    return false;
                }
            }
        }
        private async static Task SeedIdentityAsync(ServiceProvider serviceProvider)
        {
            var userService = serviceProvider.GetService<IUserCreationService>();
            var roleService = serviceProvider.GetService<IRoleCreationService>();

            await roleService.CreateRolesAsync();

            await userService.CreateUsersAsync();

        }
        private async static Task SeedYearAsync(Univ_ManageDBContext context)
        {
            var Years = Enum.GetNames(typeof(YearEnum));
            foreach (var year in Years)
            {
                var yearEntity = await context.Years.FirstOrDefaultAsync(e => e.Name == year.ToString());
                if (yearEntity is null)
                {
                    await context.AddAsync(new YearSet()
                    {
                        Name = year.ToString()
                    });
                }
                await context.SaveChangesAsync();
            }

        }
        private async static Task SeedSemesterAsync(Univ_ManageDBContext context)
        {
            var Years = context.Years.ToList();
            var Semesters = Enum.GetNames(typeof(SemesterEnum));
            foreach (var semester in Semesters)
            {
                var semesterEntity = await context.Semesters.FirstOrDefaultAsync(e => e.Name == semester.ToString());
                if (semesterEntity is null)
                {
                    await context.AddAsync(new SemesterSet()
                    {
                        Name = semester.ToString(),
                        YearId = Array.FindIndex(Semesters, row => row.ToString() == semester.ToString())<8?Years[Array.FindIndex(Semesters, row => row.ToString() == semester.ToString())/2].Id:3
                    });
                }
                await context.SaveChangesAsync();
            }

        }
        private async static Task SeedDepartmentsAsync(Univ_ManageDBContext context)
        {
            var Departments = Enum.GetNames(typeof(DepartmentEnum));
            foreach (var department in Departments)
            {
                var departmentEntity = await context.Departments.FirstOrDefaultAsync(e => e.Name == department.ToString());
                if (departmentEntity is null)
                {
                    await context.AddAsync(new DepartmetSet()
                    {
                        Name = department.ToString(),
                        Description= "Descriptions"
                    });
                }
                await context.SaveChangesAsync();
            }
        }
        private async static Task SeedSubjectsAsync(Univ_ManageDBContext context)
        {
            var Semesters = context.Semesters.ToList();
            var Departments = context.Departments.ToList();
            var subjects = new List<SubjectSet>()
            {
                new SubjectSet()
                {
                    Name = "الاقتصاد الرياضي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الأسواق المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"

                },
                new SubjectSet()
                {
                    Name = "التكامل الاقتصادي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاديات العمل",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التسويق الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد المالي والنقدي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد الزراعي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "استخدام الحاسوب في الاقتصاد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد الاسلامي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاديات الخدمات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاديات المشروعات الصغيرة والمتوسطة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد الصناعي و البيئة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد السوري",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اصول مراجعة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة التكاليف الزراعية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة الدولية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة التكاليف المعيارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظرية المحاسبة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة المالية الخاصة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة الادارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة النفط",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اصول مراجعة 1",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "استخدام الحاسوب في المحاسبة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة المتقدمة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم معلومات ادارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة استراتيجية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تنمية ادارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة المواد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة مؤسسات خدمية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "استخدام الحاسوب في الادارة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظرية القرارات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الرقابة الادارية المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التسويق الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة الجودة الشاملة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة المؤسسات المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة محافظ استثمارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "العلاقات المالية والنقدية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "السياسات النقدية و المصرفية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاسواق المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "العمليات المصرفية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم المعلومات  المصرفية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "استخدام الحاسوب في الاقتصاد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التمويل والاستثمار",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مؤسسات مالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاد مالي و نقدي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مصارف اسلامية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تسويق سياحي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة استراتيجية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة علاقات عامة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التسويق الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التسويق الالكتروني",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات تسويقية باللغة الانكليزية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تسويق الخدمات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اتصالات تسويقية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة المبيعات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم معلومات تسويقية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "بحوث التسويق",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظرية القرارات الادارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الضبط الاحصائي للجودة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تقنية المعلومات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم قواعد بيانات 2",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "منهجية البحث العلمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "أمن و حماية المعلومات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مشروع تخرج 2-4",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التجارة الاكترونية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم دعم القرارات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد القياسي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تحليل و تصميم نظم المعلومات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "النظم الخبيرة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FourthYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التخطيط الاستراتيجي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التنمية الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"

                },
                new SubjectSet()
                {
                    Name = "أسس البرمجة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الاقتصاد القياسي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الحسابات الاقتصادية القومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "القانون الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الادارة المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاديات الدول العربية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة التكاليف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "علم السكان",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاد الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التمويل الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="اقتصاد").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "النظام المحاسبي الموحد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة المتوسطة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "أسس البرمجة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة المنشآت المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تحليل القوائم المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الأسواق المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة الضريبية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المحاسبة الحكومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الحسابات الاقتصادية القومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة منشآت خدمية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة التكاليف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="محاسبة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة المشروعات الصغيرة و المتوسطة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "السلوك التنظيمي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "أسس البرمجة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "بحوث العمليات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الحسابات الاقتصادية القومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظريات الادارة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الادارة المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة التكاليف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الادارة العامة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة الأعمال الدولية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة التسويق",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="إدارة").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التأمين و الضمان الاجتماعي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التسويق المصرفي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الإدارة المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة المنشآت المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الحسابات الاقتصادية القومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التشريعات المالية و المصرفية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "استخدام الحاسوب في العمليات المصرفية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة التكاليف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التجارة الالكترونية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "ادارة المخاطر",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التمويل الدولي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="معاملات_مالية_ومصرفية").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "إدارة التسويق",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التوزيع",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "إدارة المواد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "سياسات المنتج",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "سياسة التسعير",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الإدارة المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تطبيقات حاسوبية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة التكاليف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "سلوك المستهلك",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "بحوث العمليات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات ادارية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="تسويق").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تحليل القوائم المالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "دراسات الجدوى الاقتصادية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم ادارة قواعد البيانات 1",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم معلومات إدارية 1",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم معلومات إدارية 2",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "تصميم و إدارة مواقع الانترنت",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "معالجة البيانات و تحليلها",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "أسس البرمجة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "إدارة الشبكات الحاسوبية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "لغة برمجة حديثة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "بحوث العمليات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "هيكلة البيانات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="نظم_المعلومات").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="ThirdYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اللغة الأجنبية 1",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المدخل إلى علم الحاسوب",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اللغة العربية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الرياضيات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الثقافة الوطنية القومية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اللغة الأجنبية 2",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المدخل إلى علم الاقتصاد",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مبادئ إدارة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مبادئ المحاسبة 1",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مبادئ الإحصاء",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التحليل الاقتصادي الجزئي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="FirstYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة شركات الأموال",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الإحصاء التطبيقي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "إدارة العمليات",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "التحليل الاقتصادي الكلي",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اللغة الأجنبية 3",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "الرياضيات الاقتصادية والمالية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "محاسبة شركات الأشخاص",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اللغة الأجنبية 4",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "النقود و المصارف",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "إدارة الموارد البشرية",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "المدخل إلى القانون التجاري",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "اقتصاديات المالية العامة",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "مبادئ تسويق",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearFirstsemester").Id,
                    Description = "Descreption"
                },
                new SubjectSet()
                {
                    Name = "نظم تشغيل حاسوب",
                    DepartmentId = Departments.FirstOrDefault(d=>d.Name=="عام").Id,
                    SemesterId = Semesters.FirstOrDefault(s=>s.Name=="SecondYearSecondsemester").Id,
                    Description = "Descreption"
                }
            };
            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
        }
    }
}
