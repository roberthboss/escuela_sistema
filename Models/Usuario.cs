using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int user_id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Usuario")]
    public string usuario { get; set; }

    [Required]
    [StringLength(256)]
    [Display(Name = "Contraseña")]
    public string password { get; set; }
}
