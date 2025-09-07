namespace CSharpOops.Topics
{
    public class Inheritance
    {
        public static void Run()
        {
            Console.WriteLine("--- Inheritance Example ---");

            // Creating object of derived class Dog
            Dog dog = new Dog();

            // Method overridden in Dog class will run
            dog.Speak();
        }
    }

    // Base class (parent)
    class Animal
    {
        public virtual void Speak()
        {
            Console.WriteLine("The animal makes a sound");
        }
    }

    // Derived class (child) inherits Animal
    class Dog : Animal
    {
        // Override parent method
        public override void Speak()
        {
            Console.WriteLine("The dog barks");
        }
    }
}
