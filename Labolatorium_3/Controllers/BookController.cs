﻿using Microsoft.AspNetCore.Mvc;
using Labolatorium_3.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Laboratorium_3.Models;

namespace Labolatorium_3.Controllers
{
    public class BookController: Controller
    {
        private readonly IBookService _bookService;

        private readonly IDateTimeProvider _dateTimeProvider;

        public BookController(IDateTimeProvider timeProvider, IBookService bookService)
        {
            _dateTimeProvider = timeProvider;
            _bookService = bookService;
        }

        //static int id = 0;

        public IActionResult Index()
        {
            return View(_bookService.FindAll());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                // zapisanie modelu do bazy lub kolekcji.
                _bookService.Add(model);
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_bookService.FindById(id));
        }


        [HttpPost]
        public IActionResult Update(Book model)
        {
            if (ModelState.IsValid)
            {
                // zapisanie modelu do bazy lub kolekcji
                _bookService.Update(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_bookService.FindById(id));
        }


        [HttpPost]
        public IActionResult Delete(Book model)
        {
            _bookService.DeleteById(model.Id);
            return RedirectToAction("Index");
        }

    
        //[HttpGet]
        //public IActionResult Details(int id)
        //{
        //    if (_books.ContainsKey(id))
        //    {
        //        var book = _books[id];
        //        return View(book);
        //    }
        //    return NotFound(); // Jeśli książka o podanym ID nie istnieje
        //}




    }
}
