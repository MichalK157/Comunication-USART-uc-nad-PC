using System;
using System.IO.Ports;
using System.Threading;
public class PortChat
{
    static bool _continue;
	static byte []maindata=new byte[3];
	const double a=0.24;
	const double b=22.23;
    static SerialPort _serialPort;
	static byte [] msg= new byte[10];
	
	//msg[1]=0x40;
	
	
    public static void Main()
    {
		//msg[] input
		msg[1]=0x40;
		
		////	
	
        string name;
        //string message;
		char [] data=new char[2];
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(Read);

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();
		_serialPort.PortName="COM4";
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
        

        Console.Write("Name: ");
        name = Console.ReadLine();

        Console.WriteLine("Press Esc to exit");
		readThread.Start();
        while (_continue)
        {
            data[0] = Console.ReadKey().KeyChar;
			Console.WriteLine("");

            if (data[0]==0x1B)
            {
                _continue = false;
            }
			if(data[0]==0x68)
			{
				Help();
			}
			if(data[0]==0x6c)
			{
				LastData(maindata);
			}
			/*if else (data[0]==0x40)
			{
				_serialPort .Write(msg,0,1);
			}*/
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
					RxData((byte)_serialPort.ReadByte(),(byte)_serialPort.ReadByte(),(byte)_serialPort.ReadByte());
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
	
		public static void ADCValue(string sensor,byte adress,byte datah,byte datal)
		{
			int data=0;
			maindata[0]=adress;
			maindata[1]=datal;
			maindata[2]=datah;
			try
			{
				data=BitConverter.ToUInt16(maindata,1);
			}
			catch(Exception e){
				Console.WriteLine(e);
			}
			data=(int)(a*data-b);
			Console.WriteLine(sensor+data+" (+/-2°C)");
		}
		
		public static void LastData(byte[]_data)
		{
			int data=0;
			
			try
			{
				data=BitConverter.ToUInt16(_data,1);
			}
			catch(Exception e){
				Console.WriteLine(e);
			}
			data=(int)(a*data-b);
			
			if (_data[0]==0x00)	//Usatrt init
			{
				Console.WriteLine("Usart has been initialized");
			}
			if (_data[0]==0x01)	//TWI init
			{
				Console.WriteLine("twi has been initialized");
			}
			if (_data[0]==0x02)	//ERR1
			{
				Console.WriteLine("ERR1 reserwation for something");
			}
			if (_data[0]==0x03)	//ERR2
			{
				Console.WriteLine("ERR2 reserwation for something");
			}
			if (_data[0]==0x04)	//ERR3
			{
				Console.WriteLine("ERR3 reserwation for something");
			}
			if(_data[0]==0x10)	//Ax
			{
				Console.WriteLine("Ax:"+_data[2]);
			}
			if(_data[0]==0x11)	//Ay
			{
				Console.WriteLine("Ay:"+_data[2]);
			}
			if (_data[0]==0x12)	//Az
			{
				Console.WriteLine("Az:"+_data[2]);
			}
			if(_data[0]==0x20||_data[0]==0x21||_data[0]==0x22||_data[0]==0x23)
			{
			Console.WriteLine("0x{0:X}",_data[0]);
			Console.WriteLine(data+" (+/-2°C)");
			}
			
		} 
		
		public static void RxData( byte infodata, byte datah,byte datal)
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
				Console.WriteLine("Ax:"+datah);
			}
			if(infodata==0x11)	//Ay
			{
				Console.WriteLine("Ay:"+datah);
			}
			if(infodata==0x12)	//Az
			{
				Console.WriteLine("Az:"+datah);
			}
			if(infodata==0x20)	//T1
			{
				ADCValue("Board temperature: ",infodata,datah,datal);
			}
			if(infodata==0x21)	//T2
			{
				ADCValue("Sensor 1 temperature:",infodata,datah,datal);
			}
			if(infodata==0x22)	//T3
			{
				ADCValue("Sensor 2 temperature:",infodata,datah,datal);
			}
			if(infodata==0x23)	//T4
			{
				ADCValue("Sensor 3 temperature:",infodata,datah,datal);
			}
			if(infodata==0xff)
			{
					
			}
		}
		
		public static void Help()
		{
			Console.WriteLine("##########################################HELP##########################################");
			Console.WriteLine("########################################################################################");
			Console.WriteLine("Program connect to Cooling System by UART and read data errors from uC");
			Console.WriteLine("Reading data- use micro switch on board to start and stop communication or");
			Console.WriteLine("Press \"t\" to start communication and read data(now doesn't work");
			Console.WriteLine("Press \"h\" show Help");
			Console.WriteLine("Press \"Esc\" to Exit");
			Console.WriteLine("Press \"l\" to show last get data ");
			Console.WriteLine("");
		}
		
		
}