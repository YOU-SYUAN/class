using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App3
{
    public partial class teacherClassInfo : ContentPage
    {
        public teacherClassInfo()
        {
            InitializeComponent();
        }

        public async void GotoNewClassPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewClassPage());
        }
    }
}
