
// Конкретные фабрики
public class PlaneFactory : TicketFactory
{
    public override ITicket CreateTicket() => new PlaneTicket();
}
