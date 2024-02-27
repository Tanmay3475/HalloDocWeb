﻿using HalloDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestRepository: IRepository<Request>
    {
        void Update(Request request);
        void Save();
    }
}