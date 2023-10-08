using System;
using System.Collections.Generic;

class Computer
{
    public string IPAddress { get; set; }
    public int Power { get; set; }
    public string OperatingSystem { get; set; }

    public Computer(string ipAddress, int power, string os)
    {
        IPAddress = ipAddress;
        Power = power;
        OperatingSystem = os;
    }
}

class Server : Computer
{
    public int MaxConnections { get; set; }

    public Server(string ipAddress, int power, string os, int maxConnections)
        : base(ipAddress, power, os)
    {
        MaxConnections = maxConnections;
    }
}

class Workstation : Computer
{
    public string UserName { get; set; }

    public Workstation(string ipAddress, int power, string os, string userName)
        : base(ipAddress, power, os)
    {
        UserName = userName;
    }
}

class Router : Computer
{
    public List<Computer> ConnectedDevices { get; private set; }

    public Router(string ipAddress, int power, string os)
        : base(ipAddress, power, os)
    {
        ConnectedDevices = new List<Computer>();
    }
}

interface IConnectable
{
    void ConnectTo(Computer computer);
    void DisconnectFrom(Computer computer);
    void TransmitData(Computer source, Computer destination, string data);
}

class Network : IConnectable
{
    private List<Computer> computers;

    public Network()
    {
        computers = new List<Computer>();
    }

    public void AddComputer(Computer computer)
    {
        computers.Add(computer);
    }

    public void ConnectTo(Computer computer)
    {
        foreach (var comp in computers)
        {
            if (comp != computer)
            {
                if (comp is Router router)
                {
                    router.ConnectedDevices.Add(computer);
                }
                else if (computer is Router router1)
                {
                    router1.ConnectedDevices.Add(comp);
                }
            }
        }
    }

    public void DisconnectFrom(Computer computer)
    {
        foreach (var comp in computers)
        {
            if (comp != computer)
            {
                if (comp is Router router)
                {
                    router.ConnectedDevices.Remove(computer);
                }
                else if (computer is Router router1)
                {
                    router1.ConnectedDevices.Remove(comp);
                }
            }
        }
    }

    public void TransmitData(Computer source, Computer destination, string data)
    {
        Console.WriteLine($"Transmitting data from {source.IPAddress} to {destination.IPAddress}: {data}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server("192.168.1.0", 1000, "Windows Server", 100);
        Workstation workstation = new Workstation("192.168.1.1", 500, "Windows 11", "Person1");
        Router router = new Router("192.168.1.3", 200, "RouterOS");

        Network network = new Network();
        network.AddComputer(server);
        network.AddComputer(workstation);
        network.AddComputer(router);

        network.ConnectTo(server);
        network.ConnectTo(workstation);
        network.ConnectTo(router);

        network.TransmitData(workstation, server, "Say Hi! to Server");

        network.DisconnectFrom(router);
        network.TransmitData(workstation, server, "Say Hi! again to Server");

    }
}