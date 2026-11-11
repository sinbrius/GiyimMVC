namespace GiyimStokMvc.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty; // Örn: Feyzanur Duymuş
        public string Username { get; set; } = string.Empty; // Örn: feyza
        public string UserCode { get; set; } = string.Empty;
        public string Role { get; set; } = "Kasiyer";       // Kasiyer, Depocu, Admin
        
        public ICollection<StockTransaction> Transactions { get; set; } = new List<StockTransaction>();
    }
}