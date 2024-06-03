using static System.Collections.Specialized.BitVector32;

namespace Pos.Web.Data.Entities
{
    public class RoleSection
    {

            public int RoleId { get; set; }
            public PrivatePosRole Role { get; set; }

            public int SectionId { get; set; }
            public Section Section { get; set; }
      
    }
}

