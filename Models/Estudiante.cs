using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Estudiante
{
    [Key]
    public int estudiante_id { get; set; }

    [Required]
    [Display(Name = "Nombre")]
    public string nombre { get; set; }

    [Required]
    [Display(Name = "Apellido")]
    public string apellido { get; set; }

    [Required]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime fecha_nacimiento { get; set; }

    [Required]
    [Display(Name = "Grado")]
    public string grado { get; set; }

    // Relación muchos a muchos con clases
    public List<EstudianteClase> estudiante_clases { get; set; } = new List<EstudianteClase>();
}
