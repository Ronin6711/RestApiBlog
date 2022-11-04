using AutoMapper;
using RestApiBlog.Contracts.V1.Responses;
using RestApiBlog.Domain;

namespace RestApiBlog.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Games, opt =>
                    opt.MapFrom(src => src.Games.Select(r => new GameUsersResponse { Name = r.Game.Name, Image = r.Game.Image })));

            CreateMap<PublicProfile, PublicProfileResponse>()
                .ForMember(dest => dest.Games, opt => 
                    opt.MapFrom(src => src.Games.Select(r => new GameUsersResponse { Name = r.Game.Name, Image = r.Game.Image })));

            CreateMap<Game, GameResponse>();

        }
    }
}
