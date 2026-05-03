public class BusFactory : TicketFactory
{
    public override ITicket CreateTicket() => new BusTicket();
}
