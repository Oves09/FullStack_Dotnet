using System;

interface IDevice
{
    void TurnOn();
}

class LightBulb : IDevice
{
    public void TurnOn() => Console.WriteLine("LightBulb On");
}

class Switch
{
    private IDevice _device;
    public Switch(IDevice device)
    {
        _device = device;
    }

    public void On() => _device.TurnOn();
}

class Program
{
    static void Main()
    {
        IDevice bulb = new LightBulb();
        Switch s = new Switch(bulb);
        s.On();
    }
}
