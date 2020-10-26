using MenuPlan.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuPlan.Infrastruktur
{
    public class MenuPlanCRUDService
    {
        private readonly IMongoCollection<Menu> _Menus;

        public MenuPlanCRUDService(ILogger<MenuPlanCRUDService> loggerIn)
        {
            MenuplanDatabaseSettings settings = new MenuplanDatabaseSettings
            {
                ConnectionString = Environment.GetEnvironmentVariable("ConnectionString"),
                DatabaseName = Environment.GetEnvironmentVariable("DatabaseName"),
                MenusCollectionName = Environment.GetEnvironmentVariable("MenusCollectionName")
            };

            loggerIn.LogInformation($"ENVVAR_ConnectionString={Environment.GetEnvironmentVariable("ConnectionString")}");
            loggerIn.LogInformation($"ENVVAR_DatabaseName={Environment.GetEnvironmentVariable("DatabaseName")}");
            loggerIn.LogInformation($"ENVVAR_MenusCollectionName={Environment.GetEnvironmentVariable("MenusCollectionName")}");

            IMongoDatabase database = null;
            try
            {
                MongoClient client = new MongoClient(settings.ConnectionString);
                InitMongo(settings, client, loggerIn);

                database = client.GetDatabase(settings.DatabaseName);
                
                _Menus = database.GetCollection<Menu>(settings.MenusCollectionName);                                

                var oo = client.ListDatabaseNames().ToList();
            }
            catch (Exception e)
            {
                loggerIn.LogError($"DB kann nicht erreicht werden e={e.Message}");
                throw new ServiceException("DB kann nicht erreicht werden", e);
            }

        }

        private void InitMongo(MenuplanDatabaseSettings settings, MongoClient client, ILogger<MenuPlanCRUDService> loggerIn)
        {
            if (!client.ListDatabaseNames().ToList().Contains(settings.DatabaseName))
            {
                loggerIn.LogInformation("DB-Angelegt");
                client.GetDatabase(settings.DatabaseName).CreateCollection(settings.MenusCollectionName);               
                return;
            }

            var db = client.GetDatabase(settings.DatabaseName);
            if(! db.ListCollectionNames().ToList().Contains(settings.MenusCollectionName))
            {
                loggerIn.LogInformation("DB-Collection-Angelegt");
                db.CreateCollection(settings.MenusCollectionName);
            }
        }

        public List<Menu> Get() =>
            _Menus.Find(menu => true).ToList();

        public Menu Get(string id) =>
            _Menus.Find<Menu>(menu => menu.Id == id).FirstOrDefault();

        public Menu Create(Menu menu)
        {
            _Menus.InsertOne(menu);
            return menu;
        }

        public void Update(string id, Menu menuIn) =>
            _Menus.ReplaceOne(menu => menu.Id == id, menuIn);

        public void Remove(Menu menuIn) =>
            _Menus.DeleteOne(book => book.Id == menuIn.Id);

        public void Remove(string id) =>
            _Menus.DeleteOne(menu => menu.Id == id);

    }
}
