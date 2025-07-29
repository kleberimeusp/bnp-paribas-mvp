// MovimentoManualProfile.cs
using AutoMapper;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Request;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Mappings
{
    public class MovimentoManualProfile : Profile
    {
        public MovimentoManualProfile()
        {
            CreateMap<MovimentoManual, MovimentoManualResponse>();
            CreateMap<MovimentoManualRequest, MovimentoManual>();
        }
    }
}

