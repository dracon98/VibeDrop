﻿//Create by Tammy Le, 14/9/2018
using System;
using System.Collections.Generic;
using VibeDrop.Model;
using Xamarin.Forms;

namespace VibeDrop.View
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            lbl_username.TextColor = Constants.MainTextColor;
            lbl_password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            entry_username.Completed += (s, e) => entry_password.Focus();
            entry_password.Completed += (s, e) => SignIn(s, e);

        }

        /* Login Page with Logo*/
        void SignIn(object sender, EventArgs e)
        {
            User user = new User(entry_username.Text, entry_password.Text);
            if (user.CheckInfo())
            {
                DisplayAlert("Login", "Login Success", "OK");
            }
            else
            {
                DisplayAlert("Login", "Login Failed", "OK");
            }
        }
    }
}