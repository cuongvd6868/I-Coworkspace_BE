using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpacePromotion
    {
        public int WorkSpaceId { get; set; }
        public virtual WorkSpace WorkSpace { get; set; }
        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
