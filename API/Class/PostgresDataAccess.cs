using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Class
{
    public class PostgresDataAccess : AbstractDemoClass
    {
        public override void LoadData(string sql)
        {
            Console.WriteLine("Loading postgres data");
        }

        public override void SaveData(string sql)
        {
            Console.WriteLine("Saving postgres data");
        }

        public override string CloseConnection(string name)
        {
            // return base.CloseConnection(name); in case you want to return existing implementation
            return "customNull";
        }
    }
}