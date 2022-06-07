﻿using Decrypt_Library.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Decrypt_Library
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
