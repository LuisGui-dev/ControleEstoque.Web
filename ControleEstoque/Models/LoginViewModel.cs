using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o usuário")]
        [Display(Name = "Usuário:")]
        public string Usuario { get; set; }
        
        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Senha { get; set; }
        
        [Display(Name = "Lembrar Me")]
        public bool LembrarMe { get; set; }
    }
}