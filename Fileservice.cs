using System;
using EasyXLS;


public class MyFile
{
	static void Main(string[] args)
        {
            Console.WriteLine("Tutorial 03\n----------\n");

        // Create an instance of the class that creates Excel files, having two sheets (1)
        ExcelDocument workbook = new ExcelDocument(2);
        
        // Set the sheet names (2)
        workbook.easy_getSheetAt(0).setSheetName("First tab");
        workbook.easy_getSheetAt(1).setSheetName("Second tab");
        
        // Create the Excel file
        Console.WriteLine("Writing file C:\\Samples\\Tutorial03.xls.");
        workbook.easy_WriteXLSFile("C:\\Samples\\Tutorial03.xls");

        // Confirm the creation of Excel file
        String sError = workbook.easy_getError();
        if (sError.Equals(""))
            Console.Write("\nFile successfully created. Press Enter to Exit...");
        else
            Console.Write("\nError encountered: " + sError + "\nPress Enter to Exit...");

        // Dispose memory
        workbook.Dispose();

        Console.ReadLine();
        }
}