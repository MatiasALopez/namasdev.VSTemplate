using AutoMapper;
using namasdev.Core.Validation;

namespace MyApp.Negocio
{
    public class NegocioBase<TRespositorio>
        where TRespositorio : class
    {
        private readonly TRespositorio _repositorio;
        private readonly IErroresNegocio _erroresNegocio;
        private readonly IMapper _mapper;

        public NegocioBase(TRespositorio repositorio, IErroresNegocio erroresNegocio, IMapper mapper)
        {
            Validador.ValidarArgumentRequeridoYThrow(repositorio, nameof(repositorio));
            Validador.ValidarArgumentRequeridoYThrow(erroresNegocio, nameof(erroresNegocio));
            Validador.ValidarArgumentRequeridoYThrow(mapper, nameof(mapper));

            _repositorio = repositorio;
            _erroresNegocio = erroresNegocio;
            _mapper = mapper;
        }

        protected TRespositorio Repositorio
        {
            get { return _repositorio; } 
        }

        protected IErroresNegocio ErroresNegocio 
        {
            get { return _erroresNegocio; }
        }

        protected IMapper Mapper 
        {
            get { return _mapper; }
        }
    }
}
