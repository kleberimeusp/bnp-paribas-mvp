using AutoMapper;
using MovimentosManual.Application.Models.Request;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Mapping;

public class ProdutoCosifProfile : Profile
{
    public ProdutoCosifProfile()
    {
        CreateMap<ProdutoCosif, ProdutoCosifResponse>();
        CreateMap<ProdutoCosifRequest, ProdutoCosif>();
    }
}
