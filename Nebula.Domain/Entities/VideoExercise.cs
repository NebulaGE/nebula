 

namespace Nebula.Domain.Entities
{
    public class VideoExercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Ordering { get; set; }

        public int TagId { get; set; }
        public CSTag CSTag { get; set; }
       
        public int MyProperty { get; set; }
    }
}