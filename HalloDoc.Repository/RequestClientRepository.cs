using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{
    public class RequestClientRepository : Repository<Requestclient>, IRequestClientRepository
    {
        private readonly ApplicationDbContext _db;
        public RequestClientRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
