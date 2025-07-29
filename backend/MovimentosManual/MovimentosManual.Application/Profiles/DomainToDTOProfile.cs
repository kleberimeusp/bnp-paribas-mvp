using AutoMapper;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Application.Profiles
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<Produto, ProdutoResponse>();
            CreateMap<ProdutoCosif, ProdutoCosifResponse>();
            CreateMap<MovimentoManual, MovimentoManualResponse>();
        }
    }
}
