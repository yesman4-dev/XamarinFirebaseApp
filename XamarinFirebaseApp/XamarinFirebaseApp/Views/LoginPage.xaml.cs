using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFirebaseApp.Views.Student;

namespace XamarinFirebaseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ICommand TapCommand => new Command(async() => await Navigation.PushModalAsync(new RegisterUser()));

        public LoginPage()
        {
            InitializeComponent();
            checkInternet();
            bool hasKey = Preferences.ContainsKey("token");
            if(hasKey)
            {
                string token = Preferences.Get("token", "");
                if(!string.IsNullOrEmpty(token))
                {
                    Navigation.PushAsync(new HomePage());
                }
            }
        }
        public async void checkInternet()
        {
            //await DisplayAlert("Aviso", "si", "OK");
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                // Connection to internet is available
                await DisplayAlert("Aviso", "Usted no tiene acceso a Internet.\nEl acceso a Internet es requerido para el buen funcionamiento de la aplicación.", "OK");
            }
        }

        private async void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;

                //validamos que las cajas de texto no esten vacias
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Aviso", "Correo requerido", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Aviso", "Clave requerida", "Ok");
                    return;
                }

                //recibimo los paramtros 
                string token = await _userRepository.SignIn(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    Preferences.Set("token", token);
                    Preferences.Set("userEmail", email);
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    // si en un caso los parametros son invalidos
                    await DisplayAlert("Inicio", "Operacion fallida", "ok");
                }
            }
            catch(Exception exception)
            {
                if(exception.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    //si el correo no es valido
                    await DisplayAlert("Unauthorized", "Correo no valido", "ok");
                }
                else if(exception.Message.Contains("INVALID_PASSWORD"))
                {
                    //si la clave no es valida
                    await DisplayAlert("Unauthorized", "Clave incorrecta", "ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "ok");
                }
            }
            
        }

        private async void RegisterTap_Tapped(object sender, EventArgs e)
        {
            // si vamos a crear un nuevo perfil
            await Navigation.PushModalAsync(new RegisterUser());
        }

        private async void ForgotTap_Tapped(object sender, EventArgs e)
        {
            // si vamos recuperar la clave
            await Navigation.PushModalAsync(new ForgotPasswordPage());
        }
    }
}