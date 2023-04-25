using ST2Project.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ST2Project.Logic.Services
{
    public class UserService
    {
        st2databaseEntities2 _db = new st2databaseEntities2();
        public void RegisterUser(Users _employee)
        {
               _db.Users.Add(_employee);
               _db.SaveChanges();
        }
        public bool IsEmailExists(string value)
        {
                return _db.Users.Any(x => x.Email == value);
        }
        public bool IsUsernameExists(string value)
        {
                return _db.Users.Any(x => x.Username == value);
        }

        public bool IsUsersDataCorrect(Users _user)
        {
                return _db.Users.Any(x => x.Username == _user.Username && x.Password == _user.Password);
        }

        public Users FetchUserData(Users _user)
        {
                return _db.Users.SingleOrDefault(x => x.Username == _user.Username && x.Password == _user.Password);  
        }

        public List<Users> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public void RemoveUser(int id)
        {
            var userToRemove = _db.Users.Where(x => x.UserID == id).FirstOrDefault();
            foreach(var offer in _db.Offers.Where(x => x.OwnerID == id))
            {
                _db.Offers.Remove(offer);
            }
            _db.Users.Remove(userToRemove);
            _db.SaveChanges();
        }
    }
}
