using BuscaCep.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuscaCep.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CepPage : ContentPage
    {
        public CepPage(CepDto dto)
        {
            InitializeComponent();

            BindingContext = new CepViewModel(dto);
        }
    }
}