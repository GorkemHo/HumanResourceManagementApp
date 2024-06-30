using Autofac;
using AutoMapper;
using Ik_Bitirme.Application.Mapper;
using Ik_Bitirme.Application.Services.AdminService;
using Ik_Bitirme.Application.Services.AdvanceServices;
using Ik_Bitirme.Application.Services.CompanyServices;
using Ik_Bitirme.Application.Services.DepartmentService;
using Ik_Bitirme.Application.Services.DirectorService;
using Ik_Bitirme.Application.Services.DirectorServices;
using Ik_Bitirme.Application.Services.EmailServices;
using Ik_Bitirme.Application.Services.EmployeeServices;
using Ik_Bitirme.Application.Services.ExpenseRequestServices;
using Ik_Bitirme.Application.Services.JobService;
using Ik_Bitirme.Application.Services.JobServices;
using Ik_Bitirme.Application.Services.LeaveRequestService;
using Ik_Bitirme.Application.Services.UserService;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Ioc
{
    public class DependencyResolver:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppUserRepo>().As<IAppUserRepo>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.RegisterType<AdminRepo>().As<IAdminRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerLifetimeScope();

            builder.RegisterType<AdvanceRequestRepo>().As<IAdvanceRequestRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AdvanceService>().As<IAdvanceService>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyRepo>().As<ICompanyRepo>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentRepo>().As<IDepartmentRepo>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();

            builder.RegisterType<DirectorRepo>().As<IDirectorRepo>().InstancePerLifetimeScope();
            builder.RegisterType<DirectorService>().As<IDirectorService>().InstancePerLifetimeScope();

            builder.RegisterType<EmployeeRepo>().As<IEmployeeRepo>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().InstancePerLifetimeScope();

            builder.RegisterType<ExpenseRequestRepo>().As<IExpenseRequestRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ExpenseRequestService>().As<IExpenseRequestService>().InstancePerLifetimeScope();

            builder.RegisterType<JobRepo>().As<IJobRepo>().InstancePerLifetimeScope();
            builder.RegisterType<JobService>().As<IJobService>().InstancePerLifetimeScope();

            builder.RegisterType<LeaveRequestRepo>().As<ILeaveRequestRepo>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveRequestService>().As<ILeaveRequestService>().InstancePerLifetimeScope();


            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();


            builder.Register(context => new MapperConfiguration(config =>
            {
                // Register Mapper Profile
                config.AddProfile<Mapping>();
                config.AllowNullCollections = true;
                config.AddGlobalIgnore("Item");
            })).AsSelf().SingleInstance();

            builder.Register(c => {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);

            })
                .As<IMapper>()
                .InstancePerLifetimeScope();

            //silme
            base.Load(builder);
        }
    }
}
