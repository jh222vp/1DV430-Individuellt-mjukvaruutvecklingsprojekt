using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BF
{
    class Presentation
    {
        //static string internIP;
        static string externIP;
        

        public static void GetMenu(){
        
            System.Net.WebClient wc = new System.Net.WebClient();
            externIP = wc.DownloadString("http://icanhazip.com/");
            string hostName = Environment.UserName;



            //visar en grundmeny
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=================================");
            Console.WriteLine(" WoodPecker FTP Brute Force v.3.2");
            Console.WriteLine("=================================");
            Console.ResetColor();

            Console.WriteLine("");
            //Visar externa IP-Adressen
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("External IP: {0}",externIP);
            Console.Write("Active user: {0}",hostName);
            Console.ResetColor();
            Console.WriteLine("");
            //Console.Write("Internal IP: {0}", localIP);
            Console.WriteLine("                               .--.");
            Console.WriteLine(@"                             /   o\_");
            Console.WriteLine(@"                             \   ___\");
            Console.WriteLine(@"           __________________/   \_________________");
            Console.WriteLine(@"           \_______         /     \        _______/");
            Console.WriteLine(@"              \________     |     |     ________/");
            Console.WriteLine(@"                  \_______  \     / ________/");
            Console.WriteLine(@"                      \_____/\   /\_____/");
            Console.WriteLine(@"                             /A A\");
            Console.WriteLine(@"                            /     \");

            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("========================================");
            Console.WriteLine("=                Menyval               =");
            Console.WriteLine("= --Arkiv----------------------------- =");
            Console.WriteLine("=    0. Exit                           =");
            Console.WriteLine("=    1. Classic Brute Force            =");
            Console.WriteLine("=    2. Dictionary Attack              =");
            Console.WriteLine("= --Redigera-------------------------- =");
            Console.WriteLine("=    3. Port Scan                      =");
            Console.WriteLine("= --More------------------------------ =");
            Console.WriteLine("=    4. Automatic Hacking              =");
            Console.WriteLine("=    5. Quick Port Scan                =");
            Console.WriteLine("= ==================================== =");
            Console.WriteLine("=          Ange menyval [0-5]          =");
            Console.WriteLine("========================================\n");
            Console.ResetColor();
            Console.Write("Skriv in ditt val: ");
        }
    }
}