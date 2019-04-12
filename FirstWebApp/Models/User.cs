using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApp.Models
{
    [Table("user")]
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
