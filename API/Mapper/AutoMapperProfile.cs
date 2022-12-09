using API.Dtos;
using API.Entity;
using API.Extensions;
using AutoMapper;

namespace API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
             CreateMap<BusinessUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, 
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, BusinessUser>();
            CreateMap<RegisterDto, BusinessUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                    .FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                    .FirstOrDefault(x => x.IsMain).Url));
            CreateMap<DateTime,DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d,DateTimeKind.Utc));//this for trust information time
            CreateMap<DateTime?,DateTime?>().ConvertUsing(d => d.HasValue ? 
                        DateTime.SpecifyKind(d.Value, DateTimeKind.Utc): null);
        }
    }
}