using BuscaCep.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace BuscaCep
{
    public partial class MainPage : ContentPage
    {
        BuscaCepViewModel ViewModel { get => ((BuscaCepViewModel)BindingContext); }

        public MainPage()
        {
            InitializeComponent();
        }
    }    
}