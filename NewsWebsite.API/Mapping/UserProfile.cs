using AutoMapper;
using NewsWebsite.API.Dtos;
using NewsWebsite.API.Models;

namespace NewsWebsite.API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<NewsDto, News>();
            CreateMap<News, NewsViewDto>().ForMember(dst=>dst.AuthorName,opt=>opt.MapFrom(p=>p.Author!.Name));
        }
    }
}
