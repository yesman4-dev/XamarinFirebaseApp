using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFirebaseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUser : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile FileFoto = null;
        byte[] FileFotoBytes = null;

        UserRepository _userRepository = new UserRepository();
        public RegisterUser()
        {
            
                InitializeComponent();
                liname.Items.Add("ING en computacion");
                liname.Items.Add("ING electrica");
                liname.Items.Add("ING Industrial");
                liname.Items.Add("Derecho");

            
        }

        private async void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                string name = TxtName.Text;
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;
                string confirmPassword = TxtConfirmPass.Text;

                //aplicamos las validaciones para cada unas de las cajas de texto
                if (String.IsNullOrEmpty(name))
                {
                    await DisplayAlert("Warning", "Escribe tu nombre", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Escribe tu correo", "Ok");
                    return;
                }
                if (password.Length<6)
                {
                    await DisplayAlert("Warning", "La clave debe ser mayor a 6 digitos", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Escribe tu clave", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(confirmPassword))
                {
                    await DisplayAlert("Warning", "Asegurate de escribir tu clave", "Ok");
                    return;
                }
                if (password != confirmPassword)
                {
                    await DisplayAlert("Warning", "las claves no coinciden", "Ok");
                    return;
                }

                bool isSave = await _userRepository.Register(email, name, password);
                if (isSave)
                {
                    await DisplayAlert("Registro", "Perfil creado correctamente", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Registro", "Error al crear el nuevo perfil", "Ok");
                }
            }
            catch(Exception exception)
            {
               if(exception.Message.Contains("EMAIL_EXISTS"))
                {
                   await DisplayAlert("Repplica", "Este correo ya existe en la BD", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "Ok");
                }
                
            }
            

        }

        private async void imgpersona_Tapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Obtener fotografía", "Cancelar", null, "Seleccionar de galería", "Tomar foto");

            if (action == "Seleccionar de galería") { seleccionarfoto(); }
            if (action == "Tomar foto") { tomarfoto(); }
        }



        private async void tomarfoto()
        {
            FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Fotos_starbank",
                Name = "fotografia.jpg",
                SaveToAlbum = true,
                CompressionQuality = 10
            });
            // await DisplayAlert("Path directorio", FileFoto.Path, "OK");


            if (FileFoto != null)
            {
                USERImage.Source = ImageSource.FromStream(() =>
                {
                    return FileFoto.GetStream();
                });

                //Pasamos la foto a imagen a byte[] almacenandola en FileFotoBytes
                using (System.IO.MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    FileFotoBytes = memory.ToArray();
                    /*string base64Val = Convert.ToBase64String(FileFotoBytes);
                    FileFotoBytes = Convert.FromBase64String(base64Val);*/
                }
            }
        }

        private async void seleccionarfoto()
        {
            /*if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }*/

            FileFoto = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 10
            });


            if (FileFoto == null)
                return;

            USERImage.Source = ImageSource.FromStream(() =>
            {
                return FileFoto.GetStream();
            });

            //Pasamos la foto a imagen a byte[] almacenandola en FileFotoBytes
            using (System.IO.MemoryStream memory = new MemoryStream())
            {
                Stream stream = FileFoto.GetStream();
                stream.CopyTo(memory);
                FileFotoBytes = memory.ToArray();
                /*string base64Val = Convert.ToBase64String(FileFotoBytes);
                FileFotoBytes = Convert.FromBase64String(base64Val);*/
            }

            /*Imagen.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });*/
        }
    }
}