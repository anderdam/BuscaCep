using BuscaCep.Droid.Providers;
using BuscaCep.Providers;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDatabasePathProvider))]

namespace BuscaCep.Droid.Providers
{
    internal class IosDatabasePathProvider : IDatabasePathProvider
    {
        public IosDatabasePathProvider()
        {
        }

        public string GetPath()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", "Databases");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, "BuscaCep.db3");
        }
    }
}