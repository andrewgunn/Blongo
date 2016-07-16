using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo.Models.ListPosts
{
    public class Paging
    {
        public Paging(int? previousPageNumber, int? nextPageNumber)
        {
            PreviousPageNumber = previousPageNumber;
            NextPageNumber = nextPageNumber;
        }

        public int? NextPageNumber { get; }

        public int? PreviousPageNumber { get; }
    }
}
