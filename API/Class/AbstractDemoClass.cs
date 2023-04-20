using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Class
{
 
    public abstract class AbstractDemoClass
    {
        public string LoadConnection(string name)
        {
            Console.WriteLine("Load connection string");
            return "textconnectionstring";
        }

        public abstract void LoadData(string sql);

        public abstract void SaveData(string sql);

        public virtual string CloseConnection(string name)
        {
            Console.WriteLine("Close connection string");
            return "null";
        }
        
    }
}