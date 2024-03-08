using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) {
        _context = context;
        }
        public LoggedInPersonViewModel Login(string username,string password)
        {
          var aspuser=_context.Aspnetusers.FirstOrDefault(u=>u.Username==username&&u.Passwordhash==password);
            var role = _context.Aspnetuserroles.FirstOrDefault(u => u.Userid == aspuser.AspNetUserId);
            var roles = _context.Aspnetroles.FirstOrDefault(U => U.Id == role.Roleid);
            return new LoggedInPersonViewModel { AspnetId = aspuser.AspNetUserId, UserName = aspuser.Username, Role = roles.Name };
        }
        public Aspnetuser GetUserById(string id)
        {
            return _context.Aspnetusers.Include(u => u.Aspnetuserroles).FirstOrDefault(u => u.AspNetUserId == id);
        }
        public void SetUser(Aspnetuser user)
        {
            throw new NotImplementedException();
        }
    }
}
