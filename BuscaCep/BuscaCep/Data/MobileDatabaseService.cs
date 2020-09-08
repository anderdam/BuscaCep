using BuscaCep.Providers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BuscaCep.Data
{
    sealed class MobileDatabaseService
    {
        private static Lazy<MobileDatabaseService> _Lazy = new Lazy<MobileDatabaseService>(() => new MobileDatabaseService());
        private readonly SQLiteConnection _SQLiteConnection;

        public static MobileDatabaseService  Current { get => _Lazy.Value;}

        private MobileDatabaseService()
        {
            var path = DependencyService.Get<IDatabasePathProvider>().GetPath();

            _SQLiteConnection = new SQLiteConnection(path);
            _SQLiteConnection.CreateTable<ViaCepDto>();
        }
    }
}
