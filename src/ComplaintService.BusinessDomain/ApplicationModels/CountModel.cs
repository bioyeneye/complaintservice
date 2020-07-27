using System.Collections.Generic;

namespace ComplaintService.BusinessDomain.ApplicationModels
{
    public class CountModel<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}