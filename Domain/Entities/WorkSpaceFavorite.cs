using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceFavorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int WorkspaceId { get; set; }

        public virtual AppUser? User { get; set; }
        public virtual WorkSpace? Workspace { get; set; }
    }
}
