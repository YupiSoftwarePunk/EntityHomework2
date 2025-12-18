using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DatabaseControl
    {
        public static List<Phone> GetPhonesList()
        {
            using (DbAppContext context = new DbAppContext())
            {
                return context.Phones.ToList();
            }
        }
    }
}
