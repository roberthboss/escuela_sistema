using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Profesor
{
    [Key]
    public int profesor_id { get; set; }

    [Required]
    [Display(Name = "Nombre")]
    public string nombre { get; set; }

    [Required]
    [Display(Name = "Apellido")]
    public string apellido { get; set; }

    [Required]
    [Display(Name = "Especialidad")]
    public string especialidad { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Correo electrónico")]
    public string email { get; set; }

    // Relación con clases (un profesor puede dar muchas clases)
    public List<Clase> clases { get; set; } = new List<Clase>();
}
