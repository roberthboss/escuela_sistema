using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
public class EstudianteClase
{
    [Display(Name = "Estudiante")]
    public int estudiante_id { get; set; }
    [Display(Name = "Estudiante")]
    public Estudiante? estudiante { get; set; }
    
    [Display(Name = "Clase")]
    public int clase_id { get; set; }
    [Display(Name = "Clase")]
    public Clase? clase { get; set; } 
}
