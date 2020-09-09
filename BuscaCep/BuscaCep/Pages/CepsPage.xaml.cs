using BuscaCep.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuscaCep.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CepsPage : ContentPage
    {
        private CepsViewModel ViewModel { get => (CepsViewModel)this.BindingContext; }

        public CepsPage()
        {
            InitializeComponent();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.RefreshCommand.Execute(false);
        }
    }
}