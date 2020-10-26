using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuPlan.Model
{
    public class MenuplanDatabaseSettings : IMenuplanDatabaseSettings
    {
        public string MenusCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMenuplanDatabaseSettings
    {
        string MenusCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
