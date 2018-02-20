using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Business.Interfaces
{
    public interface IAdminService
    {
        bool AddUser(IEnumerable<AspNetUser> users);
        List<AspNetUser> GetAll();
        void UpdateFirstPasswordReset(string Id);

    }
}
