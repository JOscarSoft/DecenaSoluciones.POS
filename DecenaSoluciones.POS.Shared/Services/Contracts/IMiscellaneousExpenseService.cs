using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface IMiscellaneousExpenseService
    {
        Task<ApiResponse<List<AddEditMiscellaneousExpense>>> GetMiscellaneousExpenseList();
        Task<ApiResponse<AddEditMiscellaneousExpense>> GetMiscellaneousExpenseById(int id);
        Task<ApiResponse<AddEditMiscellaneousExpense>> AddNewMiscellaneousExpense(AddEditMiscellaneousExpense expense);
        Task<ApiResponse<AddEditMiscellaneousExpense>> UpdateMiscellaneousExpense(int id, AddEditMiscellaneousExpense expense);

        Task<ApiResponse<bool>> RemoveMiscellaneousExpense(int id);
    }
}
