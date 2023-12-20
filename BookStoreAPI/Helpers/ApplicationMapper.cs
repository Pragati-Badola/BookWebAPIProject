using AutoMapper;
using BookStoreAPI.DataTransferObjects;
using BookStoreAPI.Models;
using System.Data;

namespace BookStoreAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<IDataRecord, BookDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GetInt32(src.GetOrdinal(nameof(Book.Id)))))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal(nameof(Book.Title)))))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.GetString(src.GetOrdinal(nameof(Book.Description)))));
        }
    }
}
