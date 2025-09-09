class ZomatoPartner : DeliveryPlatform, IPaymentGateway{

    public override void DeliveryOrder(){

        Console.WriteLine("Delivery in 20 mins");
    }

    public void ProcessPayment(){
        Console.WriteLine("payment gateway (Paytm) transaction started...");
    }
}