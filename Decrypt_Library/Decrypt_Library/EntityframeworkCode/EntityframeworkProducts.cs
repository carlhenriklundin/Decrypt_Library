﻿using Decrypt_Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Decrypt_Library.EntityFrameworkCode
{
    internal class EntityframeworkProducts
    {
        public static List<Product> ShowAllProducts()
        {
            var products = new List<Product>();

            using (var db = new Decrypt_LibraryContext())
            {
                products = db.Products.ToList();
                return products;
            }
        }
        public static List<Product> ShowSearchedProduct(string selectedTitle)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                var products = (from prod in db.Products
                                join cate in db.Categories on prod.CategoryId equals cate.Id
                                select new Product
                                {
                                    Id = prod.Id,
                                    Title = prod.Title,
                                    AuthorName = prod.AuthorName,
                                    Isbn = prod.Isbn

                                }).ToList();



                var findProduct = products.Where(p => p.Title.ToLower().Contains(selectedTitle.ToLower()) ||
                p.AuthorName.ToLower().Contains(selectedTitle.ToLower())).ToList();


                var returnList = new List<Product>(findProduct);

                foreach (var item in returnList)
                {
                    var checkList = returnList.Where(p => p.Isbn == item.Isbn);
                    if (checkList.Count() > 1 || item.HiddenProduct == true) { returnList.Remove(item); break; }
                };

                return returnList;
            }
        }

        public static SelectedProduct ShowProductInformation(int productId)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                var products = (from prod in db.Products
                                join age in db.Audiences on prod.AudienceId equals age.Id
                                join lang in db.Languages on prod.LanguageId equals lang.Id
                                join cate in db.Categories on prod.CategoryId equals cate.Id
                                join media in db.MediaTypes on prod.MediaId equals media.Id
                                join shelf in db.Shelves on prod.ShelfId equals shelf.Id
                                where prod.Id == productId
                                select new SelectedProduct
                                {
                                    Id = prod.Id,
                                    Audience = age.AgeGroup,
                                    Language = lang.Languages,
                                    Category = cate.CategoriesName,
                                    Shelf = shelf.Shelfname,
                                    MediaType = media.FormatName,
                                    Title = prod.Title,
                                    AuthorName = prod.AuthorName,
                                    Description = prod.Description,
                                    Narrator = prod.Narrator,
                                    Isbn = prod.Isbn,
                                    Publisher = prod.Publisher,
                                    Pages = prod.Pages,
                                    Playtime = prod.Playtime,
                                    PublishDate = prod.PublishDate,
                                    Status = SetProductStatus(prod.Id),
                                    NumberInStock = SetNumberInStock(prod.Id),
                                    NumberInAvailable = SetNumberInStockAvailable(prod.Id),
                                });

                var selectProduct = products.SingleOrDefault(sp => sp.Id == productId);

                return selectProduct;
            }
        }
        public static Product ShowSpecificProductID(int selectedID)
        {
            var products = ShowAllProducts();

            using (var db = new Decrypt_LibraryContext())
            {
                var product = products.SingleOrDefault(p => p.Id == selectedID);

                return product;
            }
        }
        public static List<Product> ShowSpecificProductAuthor(string selectedAuthor)
        {
            var products = ShowAllProducts();

            using (var db = new Decrypt_LibraryContext())
            {
                var product = products.Where(p => p.AuthorName.Contains(selectedAuthor));

                return products;
            }
        }

        public static int SetNumberInStock(int productId)
        {
            int numberInStock = 0;
            var productList = EntityframeworkProducts.ShowAllProducts();
            var specificProduct = productList.SingleOrDefault(p => p.Id == productId);
            var isbn = specificProduct.Isbn;

            foreach (var product in productList)
            {
                if (product.Isbn == isbn) numberInStock++;
            }

            return numberInStock;
        }

        public static int SetNumberInStockAvailable(int productId)
        {
            int numberInStockAvailable = 0;
            var productList = EntityframeworkProducts.ShowAllProducts();
            var specificProduct = productList.SingleOrDefault(p => p.Id == productId);
            var isbn = specificProduct.Isbn;

            foreach (var product in productList)
            {
                if (product.Isbn == isbn && product.Status == true) numberInStockAvailable++;
            }

            return numberInStockAvailable;
        }

        public static bool SetProductStatus(int productId)
        {
            var productList = EntityframeworkProducts.ShowAllProducts();
            var specificProduct = productList.SingleOrDefault(p => p.Id == productId);
            var isbn = specificProduct.Isbn;

            foreach (var product in productList)
            {
                if (product.Isbn == isbn && product.Status == true) return true;

            }

            return false;
        }

        public static List<Product> ShowAllProductsOnSpecificShelf(int shelfId)
        {
            var products = ShowAllProducts();

            using (var db = new Decrypt_LibraryContext())
            {
                var product = products.Where(p => p.Id == shelfId);

                return products;
            }
        }
        public static void CreateProduct(Product product)
        {
            var newProduct = new Product() { MediaId = product.MediaId, Status = product.Status, Isbn = product.Isbn, Title = product.Title, Description = product.Description, Pages = product.Pages, Playtime = product.Playtime, Publisher = product.Publisher, LanguageId = product.LanguageId, AuthorName = product.AuthorName, PublishDate = product.PublishDate, CategoryId = product.CategoryId, ShelfId = product.ShelfId, Narrator = product.Narrator, NewProduct = product.NewProduct, AudienceId = product.AudienceId, HiddenProduct = product.HiddenProduct };

            using (var db = new Decrypt_LibraryContext())
            {
                var productList = db.Products;

                productList.Add(newProduct);
                db.SaveChanges();
            }
        }

        public static void UpdateProduct(Product product)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                db.Products.Update(product);
                db.SaveChanges();
            }
        }

        /*
        public static List<MyPagesProductList> LoanAgain()
        {
            //product.EndDate = product.EndDate.Value.AddDays(30);

            if (UserLogin.thisUser == null) return null;

            var books = EntityframeworkBookHistory.ShowUserLoanHistory();
            var loanList = new List<MyPagesProductList>();

            foreach (var book in books)
            {
                if (book.EndDate == null)
                {
                    book.EndDate = book.StartDate.Value.AddDays(60);
                    loanList.Add(book);
                }
            }

            return loanList;


        
        */

        public static void HideProduct(Product product)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                product = db.Products.Where(x => x.Id == product.Id).SingleOrDefault();
                if (product.HiddenProduct == false)
                {
                    product.HiddenProduct = true;
                }
                //db.Remove(product);
                db.SaveChanges();
            }
        }

        public static void DeleteReservation2(BookHistory bookhistory)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                db.Remove(bookhistory);
                db.SaveChanges();
            }
        }

        public static void DeleteReservation(int selectedId)
        {
            using (var db = new Decrypt_LibraryContext())
            {
                var book = db.BookHistories.Where(b => b.ProductId == selectedId)
                    .Where(u => u.UserId == UserLogin.thisUser.Id).Where(e => e.EventId == 3).FirstOrDefault();
                db.BookHistories.Remove(book);
                db.SaveChanges();

                //var updateQuantityProduct = book.SingleOrDefault(p => p.Id == selectedId);

                /* if (book != null)
                 {
                     book.Remove(updateQuantityProduct);
                 }
                */

                /* if (updateQuantityProduct == null)
                 {
                     Console.WriteLine("Finns ingen produkt med det artikelnumret och därför tas inget bort.");
                 }
                 else updateQuantityProduct.StockUnit -= Quantity; 
                */

            }
        }
    }
}
