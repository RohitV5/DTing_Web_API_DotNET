using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Class
{
    public class ExecuteAbstract
    {
        static void Majin(string[] args)
        {
            var da1 = new PostgresDataAccess();
            AbstractDemoClass da2 = new SQLiteDataAccess();

           List<AbstractDemoClass> databases = new List<AbstractDemoClass>()
           {
            new SQLiteDataAccess(),
            new SQLiteDataAccess(),
            new PostgresDataAccess()

           };

           foreach (var db in databases)  //instead of var we can write AbstractDemoClass
           {
            db.LoadConnection("Loading connection");
            db.LoadData("select * from table");
            db.SaveData("insert into table");
           }

           Console.ReadLine();
            
        }
    }
}