﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    internal class BuscaCepViewModel : ViewModelBase
    {
        public BuscaCepViewModel()
        {
        }

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

        private CepDto _ViaCepDto = null;

        public bool HasCep { get => !(_ViaCepDto is null); }

        public string Logradouro { get => _ViaCepDto?.logradouro; }

        public string Complemento { get => _ViaCepDto?.complemento; }

        public string Bairro { get => _ViaCepDto?.bairro; }

        public string Localidade { get => _ViaCepDto?.localidade; }

        public string UF { get => _ViaCepDto?.uf; }

        private Command _BuscarCommand;

        public Command BuscarCommand
            => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute(),
                () => BuscarCommandCanExecute()));

        private bool BuscarCommandCanExecute()
            => !string.IsNullOrWhiteSpace(CEP)
            && CEP.Length == 8
            && IsNotBusy;

        private async Task BuscarCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync($"https://viacep.com.br/ws/{CEP}/json/"))
                    {
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                        _ViaCepDto = JsonConvert.DeserializeObject<CepDto>(content);

                        if (_ViaCepDto.erro)
                            throw new InvalidOperationException();
                    }
                }
            }
            catch (Exception ex)
            {
                _ViaCepDto = null;

                await App.Current.MainPage.DisplayAlert("Ooops", "Algo de errado não deu certo", ex.Message);
            }
            finally
            {
                OnPropertyChanged(nameof(HasCep));
                OnPropertyChanged(nameof(Logradouro));
                OnPropertyChanged(nameof(Complemento));
                OnPropertyChanged(nameof(Bairro));
                OnPropertyChanged(nameof(Localidade));
                OnPropertyChanged(nameof(UF));

                IsBusy = false;

                BuscarCommand.ChangeCanExecute();
            }
        }
    }
}