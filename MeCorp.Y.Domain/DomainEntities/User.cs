namespace MeCorp.Y.Domain.DomainEntities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? BlockUntil { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool IsBlocked { get => FailedLoginAttempts >= 9; }

        public void IncreaseFailedLoginCount()
        {
            FailedLoginAttempts++;
        }

        public void ResetFailedLogins()
        {
            FailedLoginAttempts = 0;
        }
    }
}