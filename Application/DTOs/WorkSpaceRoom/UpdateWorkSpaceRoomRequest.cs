using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.WorkSpaceRoom
{
    public class UpdateWorkSpaceRoomRequest : CreateWorkSpaceRoomRequest
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
