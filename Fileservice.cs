using System;
using System.IO;

namespace Fileservice
{
public class MyFile
{	
	public static string path = @"C:\Users\mkac\Documents\Gitprograms\Comunication-USART-uc-nad-PC\MyTestv2.txt";
	public static  StreamWriter wr;
	public  static StreamReader sr;
	public  static FileStream fs;
	//public static byte [] _byte=new byte[3];
	
/*
	public static void Main()
	{
		CreateFile();
		WriteToFile();
       	ReadFile();
	}
*/	

	public static void CreateFile()
	{
		if(!File.Exists(MyFile.path))
		{
			using(MyFile.wr=File.CreateText(MyFile.path))
			{
				MyFile.wr.WriteLine("DATA");
                MyFile.wr.WriteLine("####################");
                MyFile.wr.WriteLine("####################");
			}
		}
	}
	public static void WriteToFile(byte []m_byte)
	{
		using(MyFile.fs=File.Open(MyFile.path,FileMode.Open,FileAccess.Write))
		{
			MyFile.fs.Position=MyFile.fs.Length;
			MyFile.fs.Write(m_byte,0,8);
		}
	}
	public static void ReadFile()
	{
		using (MyFile.sr = File.OpenText(MyFile.path))
        {
            string s;
            while ((s = MyFile.sr .ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }
	}
	public static void WriteToFileFF(byte[] m_byte)
	{
		byte[] m_bytechanel=new byte[2];
		m_bytechanel[0]=0;
		m_bytechanel[1]=m_byte[0];
		byte[] m_bytedata=new byte[2];
		m_bytedata[0]=m_byte[1];
		m_bytedata[1]=m_byte[2];
		ushort _chanel=BitConverter.ToUInt16(m_bytechanel,0);
		ushort _data=BitConverter.ToUInt16(m_bytedata,0);
	}
}
}