using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class CreateRoleViewModel
    {
       public List<Menu> menus {  get; set; }
        public string name { get; set; }
        public int accounttype { get; set; }
        public List<int> menuid { get; set; }
        public List<Role> roles { get; set; }
        enum account { january,Admin,Physician};
        public string findAccount(int id)
        {
            return ((account)id).ToString();
        }
        public List<Rolemenu> rolemenus { get; set; }
        public int role_id { get; set; }
    }
}
