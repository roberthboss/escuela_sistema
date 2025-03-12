using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Clase
{
    [Key]
    public int clase_id { get; set; }

    [Required]
    [Display(Name = "Clase")]
    public string nombre_clase { get; set; }

    [Display(Name = "Profesor")]
    public int profesor_id { get; set; }

    [ForeignKey(nameof(profesor_id))]
    public Profesor? profesor { get; set; }


    public List<EstudianteClase> estudiante_clases { get; set; } = new List<EstudianteClase>();
}
