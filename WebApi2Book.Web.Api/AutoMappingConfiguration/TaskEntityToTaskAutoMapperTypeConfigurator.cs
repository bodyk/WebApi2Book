using System.Collections.Generic;
using AutoMapper;
using WebApi2Book.Common.TypeMapping;
using WebApi2Book.Web.Api.Models;
using Task = WebApi2Book.Data.Entities.Task;

namespace WebApi2Book.Web.Api.AutoMappingConfiguration
{
    public class TaskEntityToTaskAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Task, Models.Task>()
                    .ForMember(opt => opt.Links, x => x.Ignore())
                    .ForMember(opt => opt.Assignees, x => x.ResolveUsing<IValueResolver<Task, Models.Task, List<User>>>());

            });
        }
    }
}