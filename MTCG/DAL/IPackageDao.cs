﻿using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal interface IPackageDao
    {
        bool InsertPackage(List<string> cardIds);
        bool IsPackageAvailable();
        void DeletePackage(int packageId);
        int GetOldestPackageId();
    }
}
