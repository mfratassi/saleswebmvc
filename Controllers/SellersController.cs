﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService; 

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller) //Não é necessário Department. Framework reconhece DepartmentId
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Seller seller = _sellerService.FindById(id.Value); 
            if (seller == null)
                return NotFound();

            return View(seller);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            Seller seller = _sellerService.FindById(id.Value);
            if (seller == null)
                return NotFound();

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            
            Seller seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return NotFound();

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
                return NotFound();
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}