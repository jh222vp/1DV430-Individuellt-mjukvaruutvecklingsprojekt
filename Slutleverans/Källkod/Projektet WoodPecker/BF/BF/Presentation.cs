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


        public static void GetMenu()
        {
            //Tar fram vilken användare som är inloggad
            string hostName = Environment.UserName;

            //Läser källkoden ur URLen icanhazip.com och skriver ut IP-adressen.
            System.Net.WebClient wc = new System.Net.WebClient();
            try
            {
                externIP = wc.DownloadString("http://icanhazip.com/");
            }
            catch (Exception)
            {
                Console.WriteLine("external IP Could not be found");
            }

            //Visar en grundmeny
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=================================");
            Console.WriteLine(" WoodPecker FTP Brute Force v.3.2");
            Console.WriteLine("=================================");
            Console.ResetColor();

            Console.WriteLine("");
            //Visar externa IP-Adressen och den inloggade användaren
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("External IP: {0}", externIP);
            Console.Write("Active user: {0}", hostName);
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("                    .--.");
            Console.WriteLine(@"                  /   o\_");
            Console.WriteLine(@"                  \   ___\");
            Console.WriteLine(@"__________________/vvv\_________________");
            Console.WriteLine(@"\_______         /vvvvv\        _______/");
            Console.WriteLine(@"   \________     |vvvvv|     ________/");
            Console.WriteLine(@"       \_______  \vvvvv/ ________/");
            Console.WriteLine(@"           \_____/\vvv/\_____/");
            Console.WriteLine(@"                  /A A\");
            Console.WriteLine(@"                 /     \");
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("===========================================");
            Console.WriteLine("=                Menu                     =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=   0. Exit                               =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=                Attacks                  =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=   1.  Classic Brute Force               =");
            Console.WriteLine("=   2.  Dictionary Attack                 =");
            Console.WriteLine("=   3.  Username Brute Force              =");
            Console.WriteLine("=   4.  Burst Mode                        =");
            Console.WriteLine("=   5.  Port Scan                         =");
            Console.WriteLine("=   6.  Quick Port Scan                   =");
            Console.WriteLine("=   7.  Automatic Hacking                 =");
            Console.WriteLine("=   8.  Statistics                        =");
            Console.WriteLine("=   9.  Set Email Properties              =");
            Console.WriteLine("=   10. Help                              =");
            Console.WriteLine("= =========================================");
            Console.WriteLine("=   Select your chooice [0-10]            =");
            Console.WriteLine("===========================================\n");
            
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("= =========================================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("= Tip:Attacks can be abortet with ESC-key =");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("===========================================\n");
            Console.ResetColor();
            
            Console.ResetColor();
            Console.Write("Select your attack: ");
        }

        //Presentation av hjälpmenyn
        public static void HelpMenu()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===========================================");
            Console.WriteLine("=                Help                     =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=   0.  Back                              =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=       Select attack to get help         =");
            Console.WriteLine("===========================================");
            Console.WriteLine("=   1.  Classic Brute Force               =");
            Console.WriteLine("=   2.  Dictionary Attack                 =");
            Console.WriteLine("=   3.  Username Brute Force              =");
            Console.WriteLine("=   4.  Burst Mode                        =");
            Console.WriteLine("=   5.  Port Scan                         =");
            Console.WriteLine("=   6.  Quick Port Scan                   =");
            Console.WriteLine("=   7.  Automatic Hacking                 =");
            Console.WriteLine("=   8.  Statistics                        =");
            Console.WriteLine("=   9.  Set Email and Logging Properties  =");
            Console.WriteLine("= =========================================");
            Console.WriteLine("=   Select your chooice [0-9]             =");
            Console.WriteLine("===========================================\n");
            Console.ResetColor();
            Console.Write("Select what you need help with: ");
        }
        
        //Menyn för hjälpavsnittet vid Brute Force attacken
        public static void HelpBruteFoce()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("==============================================");
            Console.WriteLine("= The bruteforce attack tries every possible =");
            Console.WriteLine("= combination from the alphabet untill the   =");
            Console.WriteLine("= correct password is found..                =");
            Console.WriteLine("==============================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("==============================================");
            Console.WriteLine("=            How-to example below:           =");
            Console.WriteLine("==============================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("==============================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1     =");
            Console.WriteLine("= Select Port: 21                            =");
            Console.WriteLine("= Username: Admin                            =");
            Console.WriteLine("==============================================");

            Console.ResetColor();
        }

        //Menyn för hjälpavsnittet vid Dictionary attacken
        public static void HelpDictionaryAttack()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= The dictionary attack reads and tries passwords  =");
            Console.WriteLine("= you would like to try from a text file that is   =");
            Console.WriteLine("= located somewhere on the this computer           =");
            Console.WriteLine("====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("====================================================");
            Console.WriteLine("=              How-to example below:               =");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1           =");
            Console.WriteLine("= Select Port: 21                                  =");
            Console.WriteLine("= Username: Admin                                  =");
            Console.WriteLine("= Enter location to dictionary: C:/textfile.txt    =");
            Console.WriteLine("====================================================");

            Console.ResetColor();
        }
        public static void HelpUsernameBruteForce()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= The username brute force attack reads and tries  =");
            Console.WriteLine("= different usernames you would like to try from  =");
            Console.WriteLine("= a text file that is located somewhere on the    =");
            Console.WriteLine("= computer. You set a default password before you =");
            Console.WriteLine("= begin the username brute force attack.          =");
            Console.WriteLine("====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("====================================================");
            Console.WriteLine("=              How-to example below:               =");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1                      =");
            Console.WriteLine("= Select Port: 21                                             =");
            Console.WriteLine("= Username: Admin                                             =");
            Console.WriteLine("= Enter default password to try against username list: LOVE   =");
            Console.WriteLine("= Enter location to dictionary: C:/textfile.txt               =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }
        public static void HelpBurstMode()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= The Burst Mode brute force attack attacks the    =");
            Console.WriteLine("= choosen FTP-Server by only doing some attacks    =");
            Console.WriteLine("= and then pause for a selected time. This will    =");
            Console.WriteLine("= make your attack stay under the radar and prevent=");
            Console.WriteLine("= you from getting blocked                         =");
            Console.WriteLine("====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("====================================================");
            Console.WriteLine("=              How-to example below:               =");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1                      =");
            Console.WriteLine("= Select Port: 21                                             =");
            Console.WriteLine("= Username: Admin                                             =");
            Console.WriteLine("= How many seconnds would you like to wait between attacks: 3 =");
            Console.WriteLine("= How many tries would you like to do between the pause: 5    =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }
        public static void HelpPortScan()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= The Portscan attack scans the open ports on the  =");
            Console.WriteLine("= selected server and then repports which ones are =");
            Console.WriteLine("= open and closed.                                 =");
            Console.WriteLine("====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("====================================================");
            Console.WriteLine("=              How-to example below:               =");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1                      =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }
        public static void HelpQuickPortScan()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("====================================================");
            Console.WriteLine("= The Quick Port Scan attack scans some of the most=");
            Console.WriteLine("= common FTP-ports and then display which ones are =");
            Console.WriteLine("= open and closed.                                 =");
            Console.WriteLine("====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("====================================================");
            Console.WriteLine("=              How-to example below:               =");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1                      =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }
        public static void HelpAutomaticAttack()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====================================================");
            Console.WriteLine("= The Automatic Attack automaticly scan after open  =");
            Console.WriteLine("= ports on the selected server. Then tries different=");
            Console.WriteLine("= attacks to break in to the FTP-Server.            =");
            Console.WriteLine("=====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("=====================================================");
            Console.WriteLine("=              How-to example below:                =");
            Console.WriteLine("=====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====================================================");
            Console.WriteLine("= Select Username: Admin                            =");
            Console.WriteLine("= Enter Location to dictionary: C:/dictionary.txt   =");
            Console.WriteLine("= Select IP to the FTP server: 127.0.0.1            =");
            Console.WriteLine("=====================================================");

            Console.ResetColor();
        }
        public static void HelpStatistics()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====================================================");
            Console.WriteLine("= The Statistics function let you save logfiles     =");
            Console.WriteLine("= somewhere on the local computer when Wood Pecker  =");
            Console.WriteLine("= is done attacking an FTP-Server.                  =");
            Console.WriteLine("=====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("=====================================================");
            Console.WriteLine("=              How-to example below:                =");
            Console.WriteLine("=====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Would you like to log the information when Wood Pecker is   =");
            Console.WriteLine("= done attacking?: yes                                        =");
            Console.WriteLine("= Where would you like to save the logfiles?: C:/             =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }
        public static void HelpEmail()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====================================================");
            Console.WriteLine("= By activating the Email function Wood Pecker will =");
            Console.WriteLine("= and send an Email to a selected Email-adress when =");
            Console.WriteLine("= an attack is completeted.                         =");
            Console.WriteLine("=====================================================");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("=====================================================");
            Console.WriteLine("=              How-to example below:                =");
            Console.WriteLine("=====================================================");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("=====================================================");
            Console.WriteLine("= Would you like to send email when Wood Pecker is  =");
            Console.WriteLine("= done attacking?: yes                              =");
            Console.WriteLine("= Enter selected Email where logging will be        =");
            Console.WriteLine("= sent:me@email.com                                 =");
            Console.WriteLine("===============================================================");

            Console.ResetColor();
        }

    }
}