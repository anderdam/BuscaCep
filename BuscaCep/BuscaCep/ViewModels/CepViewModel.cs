using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    internal class CepViewModel : ViewModelBase
    {
        private readonly CepDto _CepDto;

        public CepViewModel(CepDto cepDto)
        {
            _CepDto = cepDto;
        }

        public bool HasCep { get => !(_CepDto is null); }

        public string Cep { get => _CepDto?.cep; }

        public string Logradouro { get => _CepDto?.logradouro; }

        public string Complemento { get => _CepDto?.complemento; }

        public string Bairro { get => _CepDto?.bairro; }

        public string Localidade { get => _CepDto?.localidade; }

        public string UF { get => _CepDto?.uf; }
    }
}