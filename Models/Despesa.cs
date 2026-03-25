using System;
using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.Models
{
    public class Despesa
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string Categoria { get; set; }
    }
}