using System;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace ADHelper {
    /// <summary>
    /// 
    /// </summary>
    class Program {

        /**
         * @TODO use parameterized input instead of the app.config file name
         */ 
        
        static void Main(string[] args) {

            //string dn = "OU=2018,OU=Lightly Managed,OU=Users,OU=Student.Greenlease,DC=student,DC=rockhurst,DC=int";

            var username = ConfigurationManager.AppSettings["ldap_admin_username"];
            var password = ConfigurationManager.AppSettings["ldap_admin_password"];
            var file = ConfigurationManager.AppSettings["ldap_user_file"]; //this files burns the header

            ADClasses.AD_UsersCollection users = new ADClasses.AD_UsersCollection(file, true);
            Tasks.Task_GeneratePasswords task = new Tasks.Task_GeneratePasswords(users, 8, "@school1");
            task.Run();
            //Console.WriteLine(GetUser("mhammonds").Path);

            //ChangePW();
        }
    }
}
