using System;
class Program
{
    static void Main()
    {
        TicketFactory planeFactory = new PlaneFactory();
        planeFactory.ProcessBooking();

        TicketFactory trainFactory = new TrainFactory();
        trainFactory.ProcessBooking();

        TicketFactory busFactory = new BusFactory();
        busFactory.ProcessBooking();
    }
}