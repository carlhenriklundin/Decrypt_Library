﻿using Decrypt_Library.EntityFrameworkCode;
using Decrypt_Library.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Decrypt_Library.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedProductView : ContentPage
    {
        public SelectedProductView()
        {
            InitializeComponent();
        }
        public SelectedProductView(int selectedId)
        {
            InitializeComponent();
            BindingContext = EntityframeworkProducts.ShowProductInformation(selectedId);
            reviewList.ItemsSource = EntityframeworkReview.ShowBookReview(Title);
            starPicker.ItemsSource = EntityframeworkReview.StarValues();

            if (UserLogin.thisUser == null)
            {
                LoanOrReserveButton.IsVisible = false;
                favoriteButton.IsVisible = false;
                PlsLoginReservelbl.IsVisible = true;
                PlsloginReviewlbl.IsVisible = true;

                ShowOrNot.Text = "Populära produkter:";
                Recommendations.ItemsSource = EntityframeworkUsers.ShowTopFiveMostReadNoHistory();
            }
            else
            {
                LoanOrReserveButton.IsVisible = true;
                PlsLoginReservelbl.IsVisible = false;
                PlsloginReviewlbl.IsVisible = false;
                reviewButton.IsVisible = true;
                reviewEntry.IsVisible = true;
                starPicker.IsVisible = true;
                favoriteButton.IsVisible = true;

                if (EntityframeworkUsers.ShowRecommendations().Count == 0)
                {
                    ShowOrNot.Text = "Detta är populärt:";
                    Recommendations.ItemsSource = EntityframeworkUsers.ShowTopFiveMostReadNoHistory();
                }
                else
                {
                    ShowOrNot.Text = "Baserat på vad du har lånat tidigare:";
                    Recommendations.ItemsSource = EntityframeworkUsers.ShowRecommendations();
                }
            }

            if (LoanOrReserveButton.Text == "True")
            {
                LoanOrReserveButton.IsVisible = false;
                statuslbl.Text = "Produkten finns att låna";
            }
            else
            {
                LoanOrReserveButton.Text = "Reservera";
                statuslbl.Text = "Produkten är utlånad, försök igen senare eller reservera produkten!";
            }

            if (mediaTypelbl.Text == "Format: Bok" || mediaTypelbl.Text == "Format: E-Bok")
            {
                pageslbl.IsVisible = true;
                narratorlbl.IsVisible = false;
                playtimelbl.IsVisible = false;
            }
            else if (mediaTypelbl.Text == "Format: Ljudbok")
            {
                pageslbl.IsVisible = false;
                narratorlbl.IsVisible = true;
                playtimelbl.IsVisible = true;
            }
        }

        private async void LoanOrReserveButton_Clicked(object sender, EventArgs e)
        {
            if (LoanOrReserveButton.Text == "Reservera")
            {
                var reservationCheck = EntityframeworkBookHistory.ReserveProduct(Title);
                if (reservationCheck == false)
                {
                    await DisplayAlert("Error!", "Du har redan reserverat denna produkt!", "Alrighty Then..");
                }
                else
                {
                    await DisplayAlert("Hurra!", "Produkten är nu reserverad", "Fortsätt bläddra");
                }
            }
        }

        private async void reviewEntry_Completed(object sender, EventArgs e)
        {
            var review = new Review();

            if (string.IsNullOrWhiteSpace(reviewEntry.Text))
            {
                await DisplayAlert("Tomt fält!", "Textfältet får inte vara tomt, skriv in något!", "Ok");
            }
            else if (starPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Tomt fält!", "Vänligen ange betyget du vill ge produkten!", "Ok");
            }
            else
            {
                review.ReviewText = reviewEntry.Text;
                review.Stars = (int?)starPicker.SelectedItem;
                review.UserId = UserLogin.thisUser.Id;
                review.ProduktId = Convert.ToInt32(Idlbl.Text);

                EntityframeworkReview.ReviewEntry(review);
                reviewList.ItemsSource = null;
                reviewList.ItemsSource = reviewList.ItemsSource = EntityframeworkReview.ShowBookReview(Title);
                reviewEntry.Text = null;
                starPicker.SelectedIndex = -1;
            }
        }

        private void reviewButton_Clicked(object sender, EventArgs e)
        {
            reviewEntry_Completed(sender, e);
        }

        private async void Recommendations_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItemFromList = (Product)e.Item;
            await Navigation.PushAsync(new SelectedProductView(selectedItemFromList.Id));
            ((ListView)sender).SelectedItem = null;
        }

        private async void favoriteButton_Clicked(object sender, EventArgs e)
        {
            var favoriteCheck = EntityframeworkUsers.SetProductAsFavorite(Title);
            if (favoriteCheck == false)
            {
                await DisplayAlert("Error!", "Du har redan denna bland dina favoriter", "Oki");
            }
            else
            {
                await DisplayAlert("Sådär", "Nu är produkten tillagd! Du hittar den i listan \"Favoriter\" i min profil", "Tack!");
            }
        }
    }
}