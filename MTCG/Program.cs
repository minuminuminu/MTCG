using MTCG.BLL;
using MTCG.DAL;
using MTCG.HttpServer;
using MTCG.HttpServer.Routing;
using Npgsql;
using System.Net;

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=mtcg_minu;Password=minuminuminu;Database=mydb;Include Error Detail=true";

            IUserDao userDao = new DatabaseUserDao(connectionString);
            ICardDao cardDao = new DatabaseCardDao(connectionString);
            IPackageDao packageDao = new DatabasePackageDao(connectionString);
            IDeckDao deckDao = new DatabaseDeckDao(connectionString);

            IDeckManager deckManager = new DeckManager(deckDao);
            IUserManager userManager = new UserManager(userDao);
            IPackageManager packageManager = new PackageManager(packageDao, cardDao);

            var router = new Router(userManager, deckManager, packageManager);
            var server = new HttpServer.HttpServer(router, IPAddress.Any, 10001);
            Console.WriteLine("Server is running on port 10001.");
            server.Start();
        }
    }
}