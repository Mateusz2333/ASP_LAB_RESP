﻿using Data.Entities;
using Laboratorium_3.Models;

namespace Laboratorium_3.Models
{
    public interface IBookService
    {
        int Add(Book book);

        Book FindById(int id);

        List<Book> FindAll();

        void DeleteById(int id);

        void Update(Book book);

        List<RentalEntity> FindAllRentalsForVieModel();

        PagingList<Book> FindPage(int page, int size);

    }
}
