public abstract class DeliveryPlatform{

    public string PartnerName{get; set;}

    public void TrackOrder(){  // non-abstract method
        Console.WriteLine("Order is being tracked: " + PartnerName);
    }

    public abstract void DeliveryOrder(); //abstract method
}

