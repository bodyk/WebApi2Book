using AutoMapper;
using WebApi2Book.Common.TypeMapping;
using User = WebApi2Book.Web.Api.Models.User;

namespace WebApi2Book.Web.Api.AutoMappingConfiguration
{
    public class UserToUserEntityAutoMapperTypeConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, Data.Entities.User>()
                    .ForMember(opt => opt.Version, x => x.Ignore());

            });
        }
    }
}