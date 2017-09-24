using AutoMapper;
using WebApi2Book.Common.TypeMapping;
using WebApi2Book.Web.Api.Models;

namespace WebApi2Book.Web.Api.AutoMappingConfiguration
{
    public class UserEntityToUserAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, Models.User>()
                    .ForMember(opt => opt.Links, x => x.Ignore());

            });
        }
    }
}