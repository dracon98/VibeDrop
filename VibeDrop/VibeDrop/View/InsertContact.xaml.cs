//created by TAMMY LE, 15/9/2018
using System;
using System.Collections.Generic;
using VibeDrop.Model;
using Xamarin.Forms;

namespace VibeDrop.View
{
    public partial class InsertContact : ContentPage
    {
        public InsertContact()
        {
            InitializeComponent();
            Setup();
        }

        void Setup()
        {
            BackgroundColor = Constants.BackgroundColor;
            //set the color for the labels
            lbl_Rusername.TextColor = Constants.MainTextColor;
            lbl_Name.TextColor = Constants.MainTextColor;
            lbl_Email.TextColor = Constants.MainTextColor;
            lbl_Phone.TextColor = Constants.MainTextColor;

            //set the font size
            lbl_Registerlabel.FontSize = 24;
            lbl.FontSize = 26;

            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            entry_Rusername.Completed += (s, e) => entry_Name.Focus();
            entry_Name.Completed += (s, e) => entry_Email.Focus();
            entry_Email.Completed += (s, e) => entry_Phone.Focus();
            entry_Phone.Completed += (s, e) => SignUp(s, e);

        }

        void SignUp(object sender, EventArgs e)
        {
            DisplayAlert("Sign Up", "Signed up Success", "OK");
        }

    }
}
