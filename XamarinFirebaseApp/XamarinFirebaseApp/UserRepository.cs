using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinFirebaseApp
{
    public class UserRepository
    {
        string webAPIKey = "AIzaSyC9oLIMYAd6dXXEk3zxfE8-Tz4zbDmHi7A"; //obteneno el link del firebase
        FirebaseAuthProvider authProvider;
        
        public UserRepository()
        {
           authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }

        public async Task<bool> Register(string email,string name,string password)
        {
            
            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }

        public async Task<string>SignIn(string email,string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password); // para iniciar recibe dos parametros           
            if(!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<bool>ResetPassword(string email)
        {
          await authProvider.SendPasswordResetEmailAsync(email);//
            return true;
        }

        public async Task<string>ChangePassword(string token,string password)
        {
           var auth= await authProvider.ChangeUserPassword(token, password);
            return auth.FirebaseToken;
        }
    }
}
