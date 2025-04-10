using OpendataApi_LCMR.DTO;
using OpendataApi_LCMR.Models;
using AutoMapper;

namespace OpendataApi_LCMR.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Revenue, RevenueDto>()
          .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // 忽略未匹配
        }
    }
}
