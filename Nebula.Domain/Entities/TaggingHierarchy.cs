 
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nebula.Domain.Entities
{
    public class TaggingHierarchy
    {
        public int Id { get; set; }

        [ForeignKey("ParentTag")]
        public int? ParentId { get; set; }
        public CSTag ParentTag { get; set; }


        [ForeignKey("ChildTag")]
        public int? ChildId { get; set; }
        public CSTag ChildTag { get; set; }
    }
}