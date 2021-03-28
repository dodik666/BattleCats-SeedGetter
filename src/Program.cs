using System;
using System.IO;

namespace SeedGetter
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Place your save file into the save folder and type in name of this file (just name, without backslashes)");
            string fileToOpen = "saves/" + Console.ReadLine();
            string path = Path.Combine(fileToOpen);
            string result = Path.GetFileName(path);

            Console.WriteLine("Save \"{0}\" is selected", result);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Seed(fileToOpen);
        } 
        static void Seed(string path)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

            int length = (int)stream.Length;
            byte[] allData = new byte[length];
            stream.Read(allData, 0, length);

            Console.WriteLine("Scan Complete");

            byte[] year = new byte[2];
            year[0] = allData[15];
            year[1] = allData[16];

            stream.Close();

            int[] occurrence = OccurrenceE(path, year);

            using var stream2 = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            try
            {
                stream2.Position = occurrence[4] - 21;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Sorry your seed position couldn't be found\nPlease upload your save onto the save editor discord linked in the readme.md of the github\nBecome a save donater and put it in #save-files in the discord\nThank you");
            }

            Console.WriteLine("Position: {0}", stream2.Position);
            BinaryReader w = new BinaryReader(stream2);

            w.BaseStream.Position = stream2.Position;
            
            Console.WriteLine("Your seed: {0}", BitConverter.ToInt32(w.ReadBytes(5)));
            Console.WriteLine("Press any key to close application");
            Console.ReadKey();
        }
        
        static int[] OccurrenceE(string path, byte[] Currentyear)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

            int length = (int)stream.Length;
            byte[] allData = new byte[length];
            stream.Read(allData, 0, length);

            int amount = 0;
            int[] occurrence = new int[50];

            for (int i = 0; i < allData.Length - 1; i++)
            {
                if (allData[i] == Convert.ToByte(Currentyear[0]) && allData[i + 1] == Convert.ToByte(Currentyear[1]))
                {
                    occurrence[amount] = i;
                    amount++;
                }

            }
             return occurrence;
        }
    }
}