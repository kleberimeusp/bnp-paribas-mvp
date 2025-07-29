using AutoMapper;
using MovimentosManual.Application.Models.Request;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Mapping;

public class CosifProfile : Profile
{
    public CosifProfile()
    {
        CreateMap<Cosif, CosifResponse>();
        CreateMap<CosifRequest, Cosif>();
    }
}
