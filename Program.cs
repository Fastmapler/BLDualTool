using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace BLDualTool
{
    class Program
    {
        //
        //  Created by Fastmapler (BLID 31470) - pls do not steal lol
        //

        public static string addOnPath, dumpLocation;

        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Created by Fastmapler (BLID 31470)";
            Console.WriteLine("Press [1] for the BLS empty Brick UI Scanner.");
            Console.WriteLine("Press [2] for the Add-On List To Gamemode Add-On List converter.");
            while (true)
            {
                char key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (key.Equals('1'))
                    checkSaveFile();
                else if (key.Equals('2'))
                    checkAddOnList();
            }
        }

        public static void checkSaveFile()
        {
            Console.Title = "Empty Brick UI Name scanner - Fastmapler (BLID 31470)";
            Console.WriteLine("Please locate the save file...");
            addOnPath = openLocationBox(true);

            if (addOnPath == null || !File.Exists(addOnPath)) Environment.Exit(1);
            int blankCount = 0;

            Console.WriteLine("Scanning file...");
            string[] lines = File.ReadAllLines(addOnPath);
            for (int i = 0; i < lines.Length; i++)
            {
                string str = lines[i];
                if (str.IndexOf('"') == 0)
                {
                    Console.WriteLine($"Blank Name at line: {i + 1}");
                    blankCount++;
                }
            }
            Console.WriteLine($"Scanning finished. {blankCount} blank UI names found. You can now exit out.");
            Thread.Sleep(-1);
        }

        public static void checkAddOnList()
        {
            Console.Title = "Add-On List To Gamemode Add-On List - Fastmapler (BLID 31470)";
            Console.WriteLine("Please locate the ADD_ON_LIST.cs file...");
            addOnPath = openLocationBox(false);

            if (addOnPath == null || !File.Exists(addOnPath)) Environment.Exit(1);

            Console.WriteLine("Please indicate the file drop location...");
            dumpLocation = openFindLocationBox();

            if (dumpLocation == null || !Directory.Exists(dumpLocation)) Environment.Exit(1);

            string[] lines = File.ReadAllLines(addOnPath);
            List<string> addOnList = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains(" = 1;"))
                {
                    string addOn = "ADDON " + line.Substring(8, (line.Length - 13));
                    Console.WriteLine(addOn);
                    addOnList.Add(addOn);
                }
            }
            Console.WriteLine("Creating File...");
            File.AppendAllLines(dumpLocation + @"\gamemode.txt", addOnList.ToArray());
            Console.WriteLine("File created. You can now exit out.");
            Thread.Sleep(-1);
        }

        public static string openLocationBox(bool blsOnly)
        {
            string path = null;
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = @"C:\\"; //@"C:\Users\Maya\Documents\Blockland\config\server\ADD_ON_LIST.cs"
            if (blsOnly)
                dialog.Filter = "Blockland Save files (*.bls)|*.bls|All files (*.*)|*.*";
            else
                dialog.Filter = "Add-On List (ADD_ON_LIST.cs)|ADD_ON_LIST.cs|cs files (*.cs)|*.cs|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = dialog.FileName;
                    Console.WriteLine("\tPath: " + path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            return path;
        }

        public static string openFindLocationBox()
        {
            string path = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = dialog.SelectedPath;
                    Console.WriteLine("\tPath: " + path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Could not read location from disk. Original error: " + ex.Message);
                }
            }

            return path;
        }
    }
}
