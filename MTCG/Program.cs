using MTCG.BLL;
using MTCG.DAL;
using MTCG.HttpServer;
using MTCG.HttpServer.Routing;
using System.Net;

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=swe1messagedb";

            IUserDao userDao = new DatabaseUserDao(connectionString);

            IUserManager userManager = new UserManager(userDao); 

            var router = new Router(userManager);
            var server = new HttpServer.HttpServer(router, IPAddress.Any, 10001);
            server.Start();
        }
    }
}