namespace CSharpOops.Topics
{
    public class Interfaces
    {
        public static void Run()
        {
            Console.WriteLine("--- Interfaces Example ---");

            IVehicle car = new Car1();
            car.Start();

            IVehicle bike = new Bike();
            bike.Start();
        }
    }

    // Interface defines contract but no implementation
    interface IVehicle
    {
        void Start(); // Must be implemented
    }

    class Car1 : IVehicle
    {
        public void Start()
        {
            Console.WriteLine("Car is starting...");
        }
    }

    class Bike : IVehicle
    {
        public void Start()
        {
            Console.WriteLine("Bike is starting...");
        }
    }
}
