﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decrypt_Library.Models;
using Decrypt_Library.Readers;

namespace Decrypt_Library
{
    public class Cart
    {
        public static List<CartList> cartList = new List<CartList>();
        public static bool CheckUserId(int userId, out string UserName)
        {
            UserName = null;
            using (var db = new Models.Decrypt_LibraryContext())
            {
                var userList = db.Users;
                var user = userList.SingleOrDefault(u => u.Id == userId);
            
                if (user == null) return false;
                
                UserName = user.UserName;
                
                return true;
            }
        }

        public static void DeleteCart(int userId)
        {
            using (var db = new Models.Decrypt_LibraryContext())
            {
                var cartList = db.Carts;
                var usercartList = cartList.Where(p => p.UserId == userId);

                foreach (var item in usercartList)
                {
                    cartList.Remove(item);
                }
                db.SaveChanges();
            }
        }

        public static void DeleteItemInCart(int productId)
        {
         cartList.Remove(cartList.SingleOrDefault(x => x.Id == productId));

            using (var db = new Models.Decrypt_LibraryContext())
            {
                var cartList = db.Carts;
                var product = cartList.SingleOrDefault(x => x.ProductId == productId);

                cartList.Remove(product);
                
                db.SaveChanges();
            }
         
        }

        public static bool CheckProductId(int productId)
        {

            using (var db = new Models.Decrypt_LibraryContext())
            {
                var productList = db.Products;
                var product = productList.SingleOrDefault(p => p.Id == productId);

                if (product == null || product.Status == false) return false;

                cartList.Add(new CartList { Id = product.Id, Title = product.Title, ReturnDate = $"Återlämningsdatum: {DateTime.Now.AddDays(30):d}" });
                return true;
            }
        }



        public static bool AddABookToCart(int userId, int productId)
        {
            using (var db = new Models.Decrypt_LibraryContext())
            {
                var cart = db.Carts;
                cart.Add(new Models.Cart() { ProductId = productId, UserId = userId });
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
                
                return true;
            }
        }

        

        public static void UpdateBookStatus(int? productId)
        {
            using (var db = new Models.Decrypt_LibraryContext())
            {
                var productList = db.Products;
                var product = productList.SingleOrDefault(p => p.Id == productId);
                product.Status = false;
                db.SaveChanges();
            }

        }


        public static bool CheckoutCart(int userId)
        {
            using (var db = new Models.Decrypt_LibraryContext())
            {

                var cart = db.Carts;
                var bookHistory = db.BookHistories;

               
                foreach (var item in cart)
                {
                    if (item.UserId == userId)
                    {
                    bookHistory.Add(new Models.BookHistory() { StartDate = DateTime.Now, ProductId = item.ProductId, EventId = 2, UserId = userId });
                    cart.Remove(item);
                    UpdateBookStatus(item.ProductId);
                    }
                }
                
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        //public static void TestLoanABook()
        //{
        //    var userId =  0;
       
        //    while (true)
        //    {
        //    Console.WriteLine("Ange UserId:");
        //    userId = Convert.ToInt32(Console.ReadLine());
        //    var sucess = CheckUserId(userId);
        //        if (sucess == true) break;
        //        Console.WriteLine("Användare finns inte!!");
        //    }


        //    Console.WriteLine("Hur många böcker vill du låna?");
        //    var numberOfBooks = Convert.ToInt32(Console.ReadLine());

        //    for (int i = 0; i < numberOfBooks; i++)
        //    {
        //        Console.WriteLine("Ange produktId på bok:");
        //        var produktId = Convert.ToInt32(Console.ReadLine());
        //        var sucess = AddABookToCart(userId, produktId);
        //        if (!sucess) { Console.WriteLine("Boken finns inte eller går inte att låna"); i--; }
            
        //    }

        //    CheckoutCart(userId);
        //}

    }

    public class CartList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReturnDate { get; set; }
    }
}
