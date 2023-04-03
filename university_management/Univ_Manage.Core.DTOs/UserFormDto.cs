namespace Univ_Manage.Core.DTOs
{
    public class UserFormDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public bool Terms { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public int Department { get; set; }
    }
}
