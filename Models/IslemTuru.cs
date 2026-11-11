namespace GiyimStokMvc.Models
{
    public enum IslemTuru
    {
        Satis = 1,          // Stok düşer (-)
        Iade = 2,           // Stok artar (+)
        SatisIptal = 3,     // Stok artar (+) (Kasada anlık iptal)
        SayimDuzeltme = 4   // Stok düzeltme
    }
}