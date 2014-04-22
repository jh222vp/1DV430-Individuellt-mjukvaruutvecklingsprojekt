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
        private string host = null;
        private int port;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;

        /* Construct Object */
        public ftp(string hostIP, int portIP, string userName, string password) 
        { 
          host = hostIP;
          port = portIP;
          user = userName;
          pass = password;
        }



        // Connect to and list files from FTP Server
        public bool LoginCheck()
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host+":"+port);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();

                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);

                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) 
                {
                    
                    Console.WriteLine(ex.ToString());
                
                }

                /* Resource Cleanup*/
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
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

                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
