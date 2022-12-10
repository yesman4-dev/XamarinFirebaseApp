using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFirebaseApp.Views.Student
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentEntry : ContentPage
    {
        StudentRepository repository = new StudentRepository();
        public StudentEntry()
        {
            InitializeComponent();
        }

        private async void ButtonSave_Clicked(object sender, EventArgs e)
        {
            string name = TxtName.Text;
            string email = TxtEmail.Text;
            if(string.IsNullOrEmpty(name))
            {
               await DisplayAlert("Warning", "Ingrese el nombre", "Cancel");
            }
            if (string.IsNullOrEmpty(email))
            {
               await DisplayAlert("Warning", "Ingrese el correo", "Cancel");
            }

            StudentModel student = new StudentModel();
            student.Name = name;
            student.Email = email;

           var isSaved=await repository.Save(student);
            if(isSaved)
            {
                await DisplayAlert("Information", "Guardado correctamente", "Ok");
                Clear();
            }
            else
            {
                await DisplayAlert("Error", "Error al guardar", "Ok");
            }

        }

        public void Clear()
        {
            TxtName.Text = string.Empty;
            TxtEmail.Text = string.Empty;
        }
    }
}