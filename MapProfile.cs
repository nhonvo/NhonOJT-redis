using AutoMapper;

namespace NhonOJT_redis
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}