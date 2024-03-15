using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{
    public class ViewNotes : Repository<Requestnote>,IViewNotes
    {
        private readonly ApplicationDbContext applicationDbContext;
       
        public ViewNotes(ApplicationDbContext db) : base(db)
        {
            applicationDbContext = db;
        }

        public void AddOrUpdateViewNote(ViewNotesViewModel vm)
        {
            
           var ViewNote=new Requestnote();
            var req = applicationDbContext.Requests.FirstOrDefault(m => m.Requestid == vm.req_id);
            var exist=applicationDbContext.Requestnotes.FirstOrDefault(M=>M.Requestid == vm.req_id);
            if (exist != null) {
                exist.Adminnotes = vm.req_data;
                exist.Modifiedby = vm.adminName;
                exist.Modifieddate=DateTime.Now;
                applicationDbContext.Requestnotes.Update(exist);
            applicationDbContext.SaveChanges();
            }
            else
            {
                ViewNote.Requestid= vm.req_id;
                ViewNote.Adminnotes = vm.Admin;
                ViewNote.Createdby = vm.adminName;
                ViewNote.Createddate= DateTime.Now;
                dbSet.Add(ViewNote);
                applicationDbContext.SaveChanges();
            }
        }
    }
}
