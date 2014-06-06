using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BF
{
    class ftp
    {
        private int port;
        private string host = null;   
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;

        public ftp(string hostIP, int portIP, string userName, string password) 
        { 
          host = hostIP;
          port = portIP;
          user = userName;
          pass = password;
        }

        // Anslutning till FTP - Server och listar innehållet
        public bool LoginCheck()
        {
            try
            {
                // Skapa en FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host+":"+port);
                // Loggar in på FTP Server med användarnman och lösenord
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                //Specificerar vilken typ av FTP Request
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                //Etablera Return kommunikation med FTP-servern
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                // Lagra FTP Serverns svarström
                ftpStream = ftpResponse.GetResponseStream();

                // Lagra FTP Serverns svarström
                StreamReader ftpReader = new StreamReader(ftpStream);

                // Lagrar responesen i variabeln directoryRaw
                string directoryRaw = null;
                
                // Läser rad för ar av svaret och lägger till en | för varje rad
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }

                // Städar anslutningarna
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                
                try
                {
                    string[] directoryList = directoryRaw.Split("|".ToCharArray());

                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("========================================");
                    Console.ResetColor();
                    Console.WriteLine("Content of the FTP-Server");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("========================================");
                    Console.ResetColor();

                    foreach (var direct in directoryList)
                    {
                        Console.WriteLine(direct);
                    }
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("========================================");
                    Console.ResetColor();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}