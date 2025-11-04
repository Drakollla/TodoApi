namespace TodoApi.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}