using AutoMapper;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Application.Profiles
{
    public class DTOToDomainProfile : Profile
    {
        public DTOToDomainProfile()
        {
            CreateMap<Produto, Produto>();
            CreateMap<ProdutoCosif, ProdutoCosif>();
            CreateMap<MovimentoManual, MovimentoManual>();
        }
    }
}
