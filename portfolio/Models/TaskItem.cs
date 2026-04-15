using System.ComponentModel.DataAnnotations;

namespace portfolio.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        // 🔥 NEW (for Trash feature)
        public bool IsDeleted { get; set; } = false;
    }
}