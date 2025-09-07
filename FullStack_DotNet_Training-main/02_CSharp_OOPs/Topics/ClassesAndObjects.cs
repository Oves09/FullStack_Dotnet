namespace CSharpOops.Topics
{
    public class ClassesAndObjects
    {
        public static void Run()
        {
            Console.WriteLine("--- Classes and Objects Example ---");

            // Creating an object of Car class
            Car car1 = new Car("Tesla", "Model S");

            // Calling method of the Car class
            car1.DisplayInfo();
        }
    }

    // Class represents a blueprint of an object
    class Car
    {
        string brand;
        string model;

        // Constructor initializes object properties
        public Car(string brand, string model)
        {
            this.brand = brand;
            this.model = model;
        }

        // Method to display object data
        public void DisplayInfo()
        {
            Console.WriteLine($"Car: {brand}, Model: {model}");
        }
    }
}
