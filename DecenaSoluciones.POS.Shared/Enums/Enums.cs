using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Enums
{
    public enum EnumReportType
    {
        Sales = 1,
        SoldProducts = 2,
        ExpenseAndIncome = 3,
    }

    public enum InventoryEntryType
    {
        In = 1,
        Out = 2,
    }

    public enum FilterOperator
    {
        None,
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
        Contains,
        StartsWith,
        EndsWith,
        DoesNotContain,
        IsNull,
        IsEmpty,
        IsNotNull,
        IsNotEmpty,
        Clear
    }
}
