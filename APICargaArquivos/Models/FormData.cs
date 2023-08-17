using System.ComponentModel.DataAnnotations;

namespace APICargaArquivos.Models;

public class FormData
{
    [Required(ErrorMessage = "Pelo menos um arquivo deve ser enviado.")]
    public List<IFormFile>? Files { get; set; }
    
    [Required(ErrorMessage = $"O campo *{nameof(User)}* é de preenchimento obrigatório.")]
    [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
    public string? User { get; set; }

    public string? Note { get; set; }
}