﻿using HalloDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IUserService
    {
        LoggedInPersonViewModel Login(string username, string password);
    }
}