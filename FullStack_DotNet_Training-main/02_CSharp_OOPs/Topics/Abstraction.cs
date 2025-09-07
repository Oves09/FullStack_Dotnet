namespace CSharpOops.Topics
{
    public class Abstraction
    {
        public static void Run()
        {
            Console.WriteLine("--- Abstraction Example ---");

            // We cannot create object of abstract class
            // But we can use reference
            AbstractDevice device = new Laptop();
            device.TurnOn();
        }
    }

    // Abstract class (cannot be instantiated)
    abstract class AbstractDevice
    {
        // Abstract method must be implemented in derived class
        public abstract void TurnOn();
    }

    // Concrete class implementing abstract method
    class Laptop : AbstractDevice
    {
        public override void TurnOn()
        {
            Console.WriteLine("Laptop is turning on...");
        }
    }
}
