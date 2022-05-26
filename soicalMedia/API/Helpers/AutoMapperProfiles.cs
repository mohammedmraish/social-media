using AutoMapper;
using social_media.Entity;
using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Extensions;

namespace soicalMedia.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDtos>()
                 .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                      src.Photos.FirstOrDefault(x => x.IsMain).Url))
                 .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                      src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();


            CreateMap<Massage, MassageDto>()
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                         src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                         src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url));


            
        }
    }
}
