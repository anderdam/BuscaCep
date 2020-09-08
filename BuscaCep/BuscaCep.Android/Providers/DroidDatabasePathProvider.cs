using BuscaCep.Droid.Providers;
using BuscaCep.Providers;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidDatabasePathProvider))]

namespace BuscaCep.Droid.Providers
{
    internal class DroidDatabasePathProvider : IDatabasePathProvider
    {
        public DroidDatabasePathProvider()
        {
        }

        public string GetPath()
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "BuscaCep.db3");
    }
}