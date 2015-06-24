using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;

namespace ADHelper.Tasks {
	class Task_BatchCreateUsers {

		bool hasHeaders;
		string pathToUserCsv;

		public Task_BatchCreateUsers(string pathToUserCsv, bool hasHeaders) {
			this.hasHeaders = hasHeaders;
			this.pathToUserCsv = pathToUserCsv;
		}

		public void Run() {
			try {
				//open a users csv and start reading it
				StreamReader reader = new StreamReader(pathToUserCsv, true);

				//if headers, burn the first line
				if (hasHeaders) {
					reader.ReadLine();
				}
				//read the rest of the file
				int count = 0;
				string line;
				while ((line = reader.ReadLine()) != null) {
					count++;
					//foreach user in file
					string[] columns = line.Split(',');

					//try without removing punctuation
					string fname = columns[1];
					string lname = columns[3];
					string samAccountName = columns[6];
					string email = columns[5];
					string password = columns[8];

					try {
						DirectoryEntry directory = new DirectoryEntry("LDAP://OU=2019,OU=Highly Managed,OU=Users,OU=Student.Greenlease,DC=student,DC=rockhurst,DC=int");
						DirectoryEntry user = directory.Children.Add("CN=" + samAccountName, "user");
						user.CommitChanges();
						directory.CommitChanges();
						user.Invoke("SetPassword", new object[] { password });
						user.CommitChanges();
						Console.WriteLine("Created: " + email);
					} catch (Exception) {
						Console.WriteLine("exception when email = " + email);
					}
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}
	}
}
