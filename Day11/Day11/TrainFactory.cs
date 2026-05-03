public class TrainFactory : TicketFactory
{
    public override ITicket CreateTicket() => new TrainTicket();
}
