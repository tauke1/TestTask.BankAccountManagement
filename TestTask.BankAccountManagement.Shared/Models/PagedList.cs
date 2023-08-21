using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.Shared.Models
{
    public class PagedList<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; }
        public int TotalCount { get; }
        public IList<T> Data { get; }

        private PagedList()
        {
        }

        public PagedList(int pageSize, int pageNumber, int totalCount, IList<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
