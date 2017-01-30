using System; 

namespace Nebula.Domain.Entities
{
    public class Cupon
    {
        public int Id { get; set; }
        public string CuponCode { get; set; }
        public byte Percent { get; set; }
        public bool IsActivated { get; set; }
        public bool IsUsed { get; set; }
        public int? ExpireTime { get; set; }
        public DateTime ExpireDate { get; set; } 

        public CouponType? Type { get; set; }
    }

    public enum CouponType
    {
        Geo,
        Eng,
        Cs
    }
}