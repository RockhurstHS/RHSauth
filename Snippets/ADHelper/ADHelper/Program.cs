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
        private static DirectoryEntry GetDirectoryObject() {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry("LDAP://artists.com", "supervisor", "R0ck753!", AuthenticationTypes.Secure);
            return oDE;
        }
        private static DirectoryEntry GetUser(string UserName) {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (!(results == null)) {
                de = new DirectoryEntry(results.Path, "supervisor", "R0ck753!", AuthenticationTypes.Secure);
                return de;
            } else {
                return null;
            }
        }
        //LDAP://artists.com/CN=Miles Hammonds,CN=Users,DC=artists,DC=com
        private static void ChangePW() {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry("LDAP://artists.com/CN=Miles Hammonds,CN=Users,DC=artists,DC=com", "supervisor", "R0ck753!", AuthenticationTypes.Secure);

            try {
                // Change the password.
                oDE.Invoke("ChangePassword", new object[] { ".Winter2012", "@school15" });
                Console.WriteLine("Supposedly it worked.");
            } catch (Exception excep) {
                Console.WriteLine("Error changing password. Reason: " + excep.Message + "\n" + excep.InnerException);
            }
        }
    }
}
