using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LigaProDB.Models
{
    public class Jugador
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        public int? Edad { get; set; }
        public int EquipoId { get; set; }
        [ForeignKey("EquipoId")]
        public Equipo? Equipo { get; set; }
        [Display(Name = "N° Camiseta")]
        [Range(1, 99)]
        public int NumeroCamiseta { get; set; }
        [Range(0, 1000)]
        public int Goles { get; set; }
        [Range(0, 1000)]
        public int Asistencias { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sueldo { get; set; }
        [Required]
        public string Posicion { get; set; }

    }
}
