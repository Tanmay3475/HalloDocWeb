using HalloDoc.Models.DataContext;
using HalloDoc.Models;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{
    public class PhysicianRepository : Repository<Physician>, IPhysicianRepository
    {
        private readonly ApplicationDbContext _db;
        public PhysicianRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
