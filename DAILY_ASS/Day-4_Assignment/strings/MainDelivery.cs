class DeliveryApp{

    static void Main(){

        DeliveryPlatform partner = new ZomatoPartner(); // implementation class

        partner.PartnerName = "Zomato";

        partner.TrackOrder();

        partner.DeliveryOrder();

        IPaymentGateway payment = (IPaymentGateway)partner;

        payment.ProcessPayment();
    }
}