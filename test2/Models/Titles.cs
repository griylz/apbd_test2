using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test2.Models;

[Table("titles")]
public class Titles
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

    public ICollection<Characters> Characters { get; set; } =  new HashSet<Characters>();
}