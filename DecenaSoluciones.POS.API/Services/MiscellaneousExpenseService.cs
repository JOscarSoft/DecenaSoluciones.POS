using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class MiscellaneousExpenseService : IMiscellaneousExpenseService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public MiscellaneousExpenseService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AddEditMiscellaneousExpense>> GetMiscellaneousExpensesList()
        {
            var expenses = await _dbContext.MiscellaneousExpenses
                .ToListAsync();
            return _mapper.Map<List<AddEditMiscellaneousExpense>>(expenses);
        }

        public async Task<AddEditMiscellaneousExpense> GetMiscellaneousExpenseById(int id)
        {
            var expense = await _dbContext.MiscellaneousExpenses
                .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<AddEditMiscellaneousExpense>(expense);
        }

        public async Task<AddEditMiscellaneousExpense> AddNewMiscellaneousExpense(AddEditMiscellaneousExpense miscellaneousExpense)
        {
            var newMiscellaneousExpense = _mapper.Map<MiscellaneousExpense>(miscellaneousExpense);

            _dbContext.MiscellaneousExpenses.Add(newMiscellaneousExpense);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditMiscellaneousExpense>(newMiscellaneousExpense);
        }

        public async Task<AddEditMiscellaneousExpense> UpdateMiscellaneousExpense(int id, AddEditMiscellaneousExpense miscellaneousExpense)
        {
            var oldExpense = await _dbContext.MiscellaneousExpenses
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el gasto a editar.");

            var newMiscellaneousExpense = _mapper.Map<MiscellaneousExpense>(miscellaneousExpense);

            _dbContext.MiscellaneousExpenses.Update(newMiscellaneousExpense);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AddEditMiscellaneousExpense>(newMiscellaneousExpense);
        }

        public async Task<bool> RemoveMiscellaneousExpense(int id)
        {
            var miscellaneousExpense = await _dbContext.MiscellaneousExpenses.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("No se encontró el gasto a eliminar.");

            _dbContext.MiscellaneousExpenses.Remove(miscellaneousExpense);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
