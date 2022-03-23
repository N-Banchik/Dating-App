using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class LikeParams : PaginationParams
    {
        public int userId { get; set; }
        
        public string predicate { get; set; }
        
        
        
    }
}