namespace DecenaSoluciones.POS.API.Models
{
    public interface IDefaultContextFactory
    {
        DecenaSolucionesDBContext CreateContext();
    }
}
