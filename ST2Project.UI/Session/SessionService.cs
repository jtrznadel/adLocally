using Microsoft.AspNetCore.Mvc;
using ST2Project.UI.Models;

namespace ST2Project.UI.Session
{
    public static class SessionService
    {
        public static UserModel _currentUser;
        public static bool _isLoggedIn = false;
        public static bool _isAdmin = false;
        public static void SaveSession(UserModel _user)
        {
            if(_user.Username == "admin" && _user.Password =="admin") _isAdmin = true;
            _currentUser = _user;
            _isLoggedIn = true;
        }
        public static void CloseSession()
        {
            _isLoggedIn = false;
            _currentUser = null;
            _isAdmin = false;
        }

        public static UserModel GetCurrentSession()
        {
            return _currentUser;
        }

        
    }
}
