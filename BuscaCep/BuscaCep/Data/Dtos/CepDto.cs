using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BuscaCep
{
    [Table("Cep")]
    public sealed class CepDto
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _cep;

        public string cep
        {
            get => _cep;
            set => _cep = string.IsNullOrWhiteSpace(value) ? value : Regex.Replace(value, @"[^\d]", string.Empty);
        }

        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public bool erro { get; set; } = false;

        [Ignore]
        public string Detalhes { get => $"{logradouro}, {bairro}, {localidade}/{uf}"; }
    }
}