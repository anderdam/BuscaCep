﻿using BuscaCep.Data;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    internal class CepsViewModel : ViewModelBase
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

                if (MobileDatabaseService.Current.Get<CepDto>(lbda => lbda.cep.Equals(Regex.Replace(CEP, @"[^\d]", string.Empty))).Any())
                {
                    await App.Current.MainPage.DisplayAlert("Ooops", "O CEP já foi pesquisado", "Ok");
                    return;
                }

                IsBusy = true;

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync($"https://viacep.com.br/ws/{CEP}/json/"))
                    {
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                        var cepDto = JsonConvert.DeserializeObject<CepDto>(content);

                        if (cepDto.erro)
                            throw new InvalidOperationException();

                        MobileDatabaseService.Current.Save(cepDto);

                        RefreshCommand.Execute(true);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ooops", "Algo de errado não deu certo", ex.Message);
            }
            finally
            {
                IsBusy = false;

                BuscarCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<CepDto> Ceps { get; private set; } = new ObservableCollection<CepDto>();

        private Command _RefreshCommand;

        public Command RefreshCommand
            => _RefreshCommand ?? (_RefreshCommand = new Command<bool>(async
                (args) => await RefreshCommandExecute(args),
                (args) => RefreshCommandCanExecute()));

        private bool RefreshCommandCanExecute()
            => IsNotBusy;

        private async Task RefreshCommandExecute(bool force = false)
        {
            try
            {
                if (!force && IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.ChangeCanExecute();

                Ceps.Clear();

                await Task.Factory.StartNew(() =>
                {
                    foreach (var cep in MobileDatabaseService.Current.Get<CepDto>())
                    {
                        Ceps.Add(cep);
                    }
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ooops", "Algo de errado não deu certo", ex.Message);
            }
            finally
            {
                IsBusy = false;

                BuscarCommand.ChangeCanExecute();
            }
        }
    }
}