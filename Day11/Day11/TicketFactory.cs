
// Абстрактный создатель
public abstract class TicketFactory
{
    // Тот самый "Фабричный метод"
    public abstract ITicket CreateTicket();

    // Можно добавить логику, общую для всех фабрик
    public void ProcessBooking()
    {
        ITicket ticket = CreateTicket();
        ticket.Book();
    }
}
