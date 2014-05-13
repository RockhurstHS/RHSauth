using System;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace ADHelper {
    class Program {
        static void Main(string[] args) {

            string dn = "OU=2018,OU=Lightly Managed,OU=Users,OU=Student.Greenlease,DC=student,DC=rockhurst,DC=int";

            var username = ConfigurationManager.AppSettings["ldap_admin_username"];
            var password = ConfigurationManager.AppSettings["ldap_admin_password"];

            ADClasses.AD_UsersCollection users = new ADClasses.AD_UsersCollection(@"C:\Apps\Consoles\ADHelper\ADHelper\students.csv", true);
            Tasks.Task_GeneratePasswords task = new Tasks.Task_GeneratePasswords(users, 8);
            task.Run();
            //Console.WriteLine(GetUser("mhammonds").Path);

            //ChangePW();
        }
        private static void ChangePW(DirectoryEntry entry) {
            try {
                // Change the password.
                entry.Invoke("ChangePassword", new object[] { "old", "new" });
            } catch (Exception excep) {
                Console.WriteLine("Error changing password. Reason: " + excep.Message + "\n" + excep.InnerException);
            }
        }
    }
}
