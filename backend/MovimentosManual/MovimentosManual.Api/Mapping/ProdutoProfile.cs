using AutoMapper;
using MovimentosManual.Application.Models.Request;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Mapping;

public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<Produto, ProdutoResponse>();
        CreateMap<ProdutoRequest, Produto>();
    }
}
