using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    class BuscaCepViewModel : ViewModelBase
    {
        private string _CEP;
        public string CEP 
        { 
            get => _CEP;
            set
            {
                _CEP = value;
                OnPropertyChanged();
                BuscarCommand.ChangeCanExecute();
            }
        }

        private string _Logradouro;
        public string Logradouro
        {
            get => _Logradouro;
            set
            {
                _Logradouro = value;
                OnPropertyChanged();
            }
        }

        private string _Complemento;
        public string Complemento
        {
            get => _Complemento;
            set
            {
                _Complemento = value;
                OnPropertyChanged();
            }
        }

        private string _Bairro;
        public string Bairro
        {
            get => _Bairro;
            set
            {
                _Bairro = value;
                OnPropertyChanged();
            }
        }

        private string _Localidade;
        public string Localidade
        {
            get => _Localidade;
            set
            {
                _Localidade = value;
                OnPropertyChanged();
            }
        }

        private string _UF;
        public string UF
        {
            get => _UF;
            set
            {
                _UF = value;
                OnPropertyChanged();
            }
        }

        private Command _BuscarCommand;
        public Command BuscarCommand
            => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute(),
                () => BuscarCommandCanExecute()));

        private bool BuscarCommandCanExecute()
            => !string.IsNullOrWhiteSpace(CEP)
            && CEP.Length == 8;

        private async Task BuscarCommandExecute()
        {
            try
            {                
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync($"https://viacep.com.br/ws/{CEP}/json/"))
                    {
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                        var retorno = JsonConvert.DeserializeObject<ViaCepDto>(content);

                        if (retorno.erro)
                            throw new InvalidOperationException();

                        Logradouro = retorno.logradouro;
                        Complemento = retorno.complemento;
                        Bairro = retorno.bairro;
                        Localidade = retorno.localidade;
                        UF = retorno.uf;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ooops", "Algo de errado não deu certo", ex.Message);
            }
        }
    }
}
