using DecenaSoluciones.POS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class GridRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public List<ColumnFilter> Filters { get; set; } = new();
    }

    public class ColumnFilter
    {
        public string Field { get; set; } = string.Empty; 
        public FilterOperator Operator { get; set; }
        public string? Value { get; set; }
    }
}
