using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public class clsUtil
    {
        static public bool CopyToImagesFolder(ref string SourceFile)
        {
            string DestinationFolder = @"C:\Users\Admin\Desktop\dev\Full Real Project\People Images\";

            FileInfo fi = new FileInfo(SourceFile);

            string DestinationFile = DestinationFolder + Guid.NewGuid().ToString() + fi.Extension.ToString();

            try
            {
                File.Copy(SourceFile, DestinationFile);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Copying Image Failed!");
                return false;
            }

            SourceFile = DestinationFile;
            return true;
        }

        static public void SaveLogedInUserInTextFile(string Username, string Password)
        {
            //Saving in File
            //string[] content = { Username, Password };

            //string FileName = "C:\\Users\\Admin\\Desktop\\dev\\Full Real Project\\DVLD\\Loged In User.txt";

            //File.WriteAllLines(FileName, content);

            string KeyName = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string UsernameValue = "Username";
            string PasswordValue = "Password";

            try
            {
                Registry.SetValue(KeyName, UsernameValue, Username);
                Registry.SetValue(KeyName, PasswordValue, Password);

            }
            catch (Exception ex)
            {
                //error message
            }

        }
        static public void DeleteLogedInUser()
        {
            string KeyName = @"SOFTWARE\DVLD";
            string UsernameValue = "Username";
            string PasswordValue = "Password";

            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(KeyName, true))
                    {
                        if (key != null)
                        {
                            // Delete the specified value
                            key.DeleteValue(UsernameValue);
                            key.DeleteValue(PasswordValue);

                        }
                        
                    }
                }
            }
            
            catch (Exception ex)
            {
                //error messsage
            }



            //Delete from file
            //string FileName = "C:\\Users\\Admin\\Desktop\\dev\\Full Real Project\\DVLD\\Loged In User.txt";

            //File.WriteAllText(FileName, string.Empty);
        }

        static public bool GetUserCredentials(ref string Username, ref string Password)
        {
            string KeyName = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string UsernameValue = "Username";
            string PasswordValue = "Password";

            Username = Registry.GetValue(KeyName, UsernameValue, null) as string;
            Password = Registry.GetValue(KeyName, PasswordValue, null) as string;

            bool isfound = false;

            if(Username != null && Password != null)
            {
                isfound = true;
            }

            //using File
            //string FileName = "C:\\Users\\Admin\\Desktop\\dev\\Full Real Project\\DVLD\\Loged In User.txt";

            //string[] Credentials = File.ReadAllLines(FileName);

            //bool isfound = false;

            //if (Credentials.Length != 0)
            //{
            //    isfound = true;
            //    Username = Credentials[0];
            //    Password = Credentials[1];
            //}

            return isfound;
            

        }

    }
}
