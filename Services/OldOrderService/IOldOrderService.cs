namespace betacomio.Services.OldOrderService
{
    // Definizione dell'interfaccia IOldOrderService che elenca i metodi per fornire i servizi relativi agli ordini precedenti (OldOrder)
    public interface IOldOrderService
    {
        // Metodo per ottenere tutti gli ordini precedenti (OldOrder) per il cliente corrente e restituire una lista di OldOrderDto
        Task<ServiceResponse<List<OldOrderDto>>> GetAllOldOrderForCustomer();
    }
}
