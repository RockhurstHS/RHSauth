using System;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Text;

namespace ADHelper.Tasks {
    class Task_GeneratePasswords {

        private int length;
        private string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345678901234567890123456789";
        private ADClasses.AD_UsersCollection users;
        private StreamWriter writer;

        public Task_GeneratePasswords(ADClasses.AD_UsersCollection users, int length) {
            this.length = length;
            this.users = users;
        }

        public void Run() {
            writer = new StreamWriter("student_passwords.csv", true);
            foreach (DirectoryEntry user in users.Users) {
                Console.WriteLine(user.Path);
                string newPassword = GenerateSinglePassword(length);
                user.Invoke("SetPassword", new object[] { newPassword });
                writer.WriteLine(user.Path + "," + newPassword);
            }
            foreach (string name in users.BadSamAccountNames) {
                Console.WriteLine(name);
            }
            writer.Close();
        }

        /// <summary>
        /// generates random character sequence where index 1 is upper, 2 is lower, 3 is numeric
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GenerateSinglePassword(int length) {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            //rule: there must be one lower, one upper, and one number
            //upper: always capitalize first character = between index of A and Z
            builder.Append(chars[random.Next(chars.IndexOf('A'), chars.IndexOf('Z'))]);
            //lower: always lowercase second character = between index of a and z
            builder.Append(chars[random.Next(chars.IndexOf('a'), chars.IndexOf('z'))]);
            //numeric: always use number third character = between index of Z+1 and chars.length
            builder.Append(chars[random.Next(chars.IndexOf('Z') + 1, chars.Length - 1)]);
            //the rest can be any random
            for (int i = 3; i < length; i++) {
                builder.Append(chars[random.Next(0, chars.Length)]);
            }
            return builder.ToString();
        }
    }
}
