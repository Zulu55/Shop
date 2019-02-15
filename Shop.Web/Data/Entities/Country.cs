namespace Shop.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Country : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Required]
        public string Name { get; set; }
    }
}
