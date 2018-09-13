using System;
using System.IO.Ports;
using System.Threading;
public class PortChat
{
    static bool _continue;
	static byte Infodata;
    static SerialPort _serialPort;
    public static void Main()
    {
        string name;
        //string message;
		char [] data=new char[2];
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(Read);

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        _serialPort.PortName = SetPortName(_serialPort.PortName);
        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
        _serialPort.Parity = SetPortParity(_serialPort.Parity);
        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        _continue = true;
        readThread.Start();

        Console.Write("Name: ");
        name = Console.ReadLine();

        Console.WriteLine("Type QUIT to exit");

        while (_continue)
        {
            data[0] = Console.ReadKey().KeyChar;
			Console.WriteLine("");

            if (data[0]==0x1B)
            {
                _continue = false;
            }
          /*  else
            {
                _serialPort.Write(			//// problem dorozwiazania 
                   data,0,1);
            }*/
        }
        readThread.Join();
        _serialPort.Close();
    }

    public static void Read()
    {
        while (_continue)
        {
            try
            {
					byte _message = (byte)_serialPort.ReadByte();
					Infodata=_message;
					RxData(Infodata,(byte)_serialPort.ReadByte());
			}
			catch (TimeoutException) { }
		}
	}

    // Display Port values and prompt user to enter a port.
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
    // Display BaudRate values and prompt user to enter a value.
    public static int SetPortBaudRate(int defaultPortBaudRate)
    {
        string baudRate;

        Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
        baudRate = Console.ReadLine();

        if (baudRate == "")
        {
            baudRate = defaultPortBaudRate.ToString();
        }

        return int.Parse(baudRate);
    }

    // Display PortParity values and prompt user to enter a value.
    public static Parity SetPortParity(Parity defaultPortParity)
    {
        string parity;

        Console.WriteLine("Available Parity options:");
        foreach (string s in Enum.GetNames(typeof(Parity)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
        parity = Console.ReadLine();

        if (parity == "")
        {
            parity = defaultPortParity.ToString();
        }

        return (Parity)Enum.Parse(typeof(Parity), parity, true);
    }
    // Display DataBits values and prompt user to enter a value.
    public static int SetPortDataBits(int defaultPortDataBits)
    {
        string dataBits;

        Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
        dataBits = Console.ReadLine();

        if (dataBits == "")
        {
            dataBits = defaultPortDataBits.ToString();
        }

        return int.Parse(dataBits.ToUpperInvariant());
    }

    // Display StopBits values and prompt user to enter a value.
    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
    {
        string stopBits;

        Console.WriteLine("Available StopBits options:");
        foreach (string s in Enum.GetNames(typeof(StopBits)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter StopBits value (None is not supported and \n" +
         "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
        stopBits = Console.ReadLine();

        if (stopBits == "" )
        {
            stopBits = defaultPortStopBits.ToString();
        }

        return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
    }
    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
    {
        string handshake;

        Console.WriteLine("Available Handshake options:");
        foreach (string s in Enum.GetNames(typeof(Handshake)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
        handshake = Console.ReadLine();

        if (handshake == "")
        {
            handshake = defaultPortHandshake.ToString();
        }

        return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
    }

		public static void RxData( byte infodata, byte data)
		{
			if(infodata==0x00)	//Usatrt init
			{
				Console.WriteLine("Usart has been initialized");
			}
			if(infodata==0x01)	//TWI init
			{
				Console.WriteLine("twi has been initialized");
			}
			if(infodata==0x02)	//ERR1
			{
				Console.WriteLine("ERR1 reserwation for something");
			}
			if(infodata==0x03)	//ERR2
			{
				Console.WriteLine("ERR2 reserwation for something");
			}
			if(infodata==0x04)	//ERR3
			{
				Console.WriteLine("ERR3 reserwation for something");
			}
			if(infodata==0x10)	//Ax
			{
				Console.WriteLine("Ax:"+data);
			}
			if(infodata==0x11)	//Ay
			{
				Console.WriteLine("Ay:"+data);
			}
			if(infodata==0x12)	//Az
			{
				Console.WriteLine("Az:"+data);
			}
			if(infodata==0x20)	//T1
			{
				Console.WriteLine("Temp1:"+data);
			}
			if(infodata==0x21)	//T2
			{
				Console.WriteLine("Temp2:"+data);
			}
			if(infodata==0x22)	//T3
			{
				Console.WriteLine("Temp3:"+data);
			}
			if(infodata==0x23)	//T4
			{
				Console.WriteLine("Temp4:"+data);
			}
		}
}