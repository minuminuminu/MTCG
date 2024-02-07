using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    internal class PackageManager : IPackageManager
    {
        private Queue<Card> Package = new Queue<Card>();

        public void CreatePackage(UserCredentials credentials)
        {

        }
    }
}
