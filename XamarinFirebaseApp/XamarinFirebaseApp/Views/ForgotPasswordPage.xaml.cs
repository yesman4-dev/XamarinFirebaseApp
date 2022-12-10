using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFirebaseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private async void ButtonSendLink_Clicked(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            if(string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Ingrese el correo", "Ok");
                return;
            }

            bool isSend =await _userRepository.ResetPassword(email);
            if(isSend)
            {
                await DisplayAlert("Reset Password", "Revisa tu correo para verificar", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Reset Password", "Error al enviar el correo", "Ok");
            }
        }
    }
}