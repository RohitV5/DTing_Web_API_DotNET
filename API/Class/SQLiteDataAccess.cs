using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Class
{
    public class SQLiteDataAccess : AbstractDemoClass
    {
        public override void LoadData(string sql)
        {
            Console.WriteLine("Loading sqllite data");
        }

        public override void SaveData(string sql)
        {
               Console.WriteLine("Saving sqllite data");
        }
    }
}