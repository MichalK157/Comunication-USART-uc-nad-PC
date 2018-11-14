using System;
using System.IO;

namespace Fileservice
{
public class MyFile
{	
	public static string path = @"C:\Users\mkac\Documents\Gitprograms\Comunication-USART-uc-nad-PC\MyTestv2.txt";
	public static StreamWriter wr;
	public static StreamReader sr;
	public static FileStream fs;
	public static byte [] _byte=new byte[3];
	
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
	public static void WriteToFile()
	{
		using(MyFile.fs=File.Open(MyFile.path,FileMode.Open,FileAccess.Write))
		{
			MyFile.fs.Position=MyFile.fs.Length;
			MyFile.fs.Write(_byte,0,3);
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
}
}