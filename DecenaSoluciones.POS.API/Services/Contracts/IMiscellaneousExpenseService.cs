using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IMiscellaneousExpenseService
    {
        Task<List<AddEditMiscellaneousExpense>> GetMiscellaneousExpensesList();
        Task<AddEditMiscellaneousExpense> GetMiscellaneousExpenseById(int id);
        Task<AddEditMiscellaneousExpense> AddNewMiscellaneousExpense(AddEditMiscellaneousExpense miscellaneousExpense);
        Task<AddEditMiscellaneousExpense> UpdateMiscellaneousExpense(int id, AddEditMiscellaneousExpense miscellaneousExpense);
        Task<bool> RemoveMiscellaneousExpense(int id);
    }
}
