using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Data;
using System.Data;
using System.Data.Entity;
using Utilities;

namespace Business.Services
{
    public class AdminServices : IAdminService
    {
        private readonly Data.TimeTrackingEntities _ttContext;

        public AdminServices()
        {
            _ttContext = new TimeTrackingEntities();
        }

        public List<TimeSheet> getAllSheets()
        {
            return _ttContext.TimeSheets.Include(i=>i.TimeInOuts).ToList();
        }

        public bool AddUser(IEnumerable<AspNetUser> users,string password)
        {
            try
            {
                foreach (var @user in users)
                {
                    var chck = _ttContext.AspNetUsers.FirstOrDefault(c => c.UserName == @user.UserName);
                    
                }
                _ttContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddUser(IEnumerable<AspNetUser> users)
        {
            throw new NotImplementedException();
        }

        public List<AspNetUser> GetAll()
        {
            return _ttContext.AspNetUsers.ToList();
        }

        public AspNetUser getByUsername(string username)
        {
            return _ttContext.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault() ;
        }

        public AspNetUser getByEmail(string email)
        {
            return _ttContext.AspNetUsers.Where(u => u.Email == email).FirstOrDefault();
        }

        public void AddUserInfo(string firstName,string lastName,string middleName,string id,bool isFirst)
        {
            UserDetail myuser = new UserDetail();
            myuser.FirstName =firstName;
            myuser.LastName =lastName;
            myuser.MiddleName = middleName;
            myuser.FK_NetID = id;
            myuser.isFirst = isFirst;

            _ttContext.UserDetails.Add(myuser);
            _ttContext.SaveChanges();
        }

        public void UpdateFirstPasswordReset(string Id)
            {
            var user = _ttContext.UserDetails.Where(u => u.FK_NetID == Id).FirstOrDefault();
            if(user !=null)
            {
                user.isFirst = false;
                _ttContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _ttContext.SaveChanges();
            }

        }
    }
}
