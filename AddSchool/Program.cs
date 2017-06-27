using SchoolSupplyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSchool
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new AccountRepository(Properties.Settings.Default.ConStr);
            Console.WriteLine("Enter the school's name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the admin's first name:");
            string adminFirstName = Console.ReadLine();
            Console.WriteLine("Enter the admin's last name: ");
            string adminLastName = Console.ReadLine();
            Console.WriteLine("Enter the admin's email address: ");
            string email = Console.ReadLine();
            Console.WriteLine("Enter the admin's password: ");
            string password = Console.ReadLine();
            repo.AddSchool(name, adminFirstName, adminLastName, email, password);
        }
    }
}
