using AutoMapper;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Application.Profiles
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<ProdutoCosif, ProdutoCosifDTO>().ReverseMap();
            CreateMap<MovimentoManual, MovimentoManualDTO>().ReverseMap();
        }
    }
}
