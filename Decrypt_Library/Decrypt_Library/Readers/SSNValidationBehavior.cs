﻿using Decrypt_Library.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Decrypt_Library.Readers
{
    internal class SSNValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += BindAbleOnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.TextChanged -= BindAbleOnTextChanged;
        }

        private void BindAbleOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var IdPattern = "^[0-9]{10}$";
            var userEntry = sender as Entry;

            if (Regex.IsMatch(e.NewTextValue, IdPattern))
            {
                userEntry.BackgroundColor = Color.White;
                AdminPage.ProductAuthorNameCorrect = true;
                RegisterPage._correctSSN = true;

            }
            else
            {
                userEntry.BackgroundColor = Color.IndianRed;
                AdminPage.ProductAuthorNameCorrect = false;
                RegisterPage._correctSSN = false;

            }
        }
    }
}
