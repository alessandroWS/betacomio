namespace betacomio.Services.OrderService
{
    // Definizione dell'interfaccia IOrderService che elenca i metodi per fornire i servizi relativi agli ordini
    public interface IOrderService
    {
        // Metodo per ottenere tutti gli ordini e restituire una lista di GetOrderDto
        Task<ServiceResponse<List<GetOrderDto>>> GetAllOrder();

        // Metodo per ottenere un ordine per ID e restituire l'oggetto GetOrderDto corrispondente
        Task<ServiceResponse<GetOrderDto>> GetOrderById(int id);

        // Metodo per aggiungere un nuovo ordine e restituire la lista aggiornata di GetOrderDto
        Task<ServiceResponse<List<GetOrderDto>>> AddOrder(AddOrderDto newOrder);

        // Metodo per aggiornare un ordine esistente e restituire l'oggetto GetOrderDto aggiornato
        Task<ServiceResponse<GetOrderDto>> UpdateOrder(UpdateOrderDto updatedOrder);

        // Metodo per eliminare un ordine per ID e restituire la lista aggiornata di GetOrderDto
        Task<ServiceResponse<List<GetOrderDto>>> DeleteOrder(int id);
    }
}
