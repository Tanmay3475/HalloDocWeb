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
    public class AspnetuserRepository: Repository<Aspnetuser>, IAspnetuserRepository
    {
        private readonly ApplicationDbContext _db;
        public AspnetuserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
