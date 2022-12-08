using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
       // [PrimeraLetraMayuscula]
        public string Name { get; set; }
        //[Range(18, 120)]
        //[NotMapped]
        //public int Edad { get; set; }
        //[NotMapped]
        //[CreditCard]
        //public string TrajetaDeCredito { get; set; }        
        //[NotMapped]
        //[Url]
        //public string URL { get; set; }
        //[NotMapped]
        //public int Mayor { get; set; }
        //[NotMapped]
        //public int Menor { get; set; }

        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var PrimeraLetra = Name[0].ToString();
                if (PrimeraLetra != PrimeraLetra.ToUpper())
                    yield return new ValidationResult("La primera letra dede ser mayuscula", new string[] { nameof(Name)});
            }

            //if (Menor > Mayor)
            //    yield return new ValidationResult("Este valor no puede ser mas grande que el campo Mayor",
            //        new string[] { nameof(Menor) });
        }
    }
}
