using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Implementations
{
    public class BillService: IBillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(BillViewModel billVm)
        {
            var order = Mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            // Get price of product
            foreach (var detail in orderDetails)
            {
                var product = _unitOfWork.ProductRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _unitOfWork.BillRepository.Add(order);
            _unitOfWork.Commit();
        }

        public void Update(BillViewModel billVm)
        {
            //Mapping to order domain
            var order = Mapper.Map<BillViewModel, Bill>(billVm);

            //Get order Detail
            var newDetails = order.BillDetails;

            //new details added
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

            //get updated details
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

            //Existed details
            var existedDetails = _unitOfWork.BillDetailRepository.FindAll(x => x.BillId == billVm.Id);

            //Clear db
            order.BillDetails.Clear();

            foreach (var detail in updatedDetails)
            {
                // Day vao khi update, add
                var product = _unitOfWork.ProductRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _unitOfWork.BillDetailRepository.Update(detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _unitOfWork.ProductRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _unitOfWork.BillDetailRepository.Add(detail);
            }

            _unitOfWork.BillDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList());

            _unitOfWork.BillRepository.Update(order);
            _unitOfWork.Commit();
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword, int pageIndex, int pageSize)
        {
            var query = _unitOfWork.BillRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BillViewModel>()
                .ToList();
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public BillViewModel GetDetail(int billId)
        {
            var bill = _unitOfWork.BillRepository.FindSingle(x => x.Id == billId);
            var billVm = Mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _unitOfWork.BillDetailRepository.FindAll(x => x.BillId == billId).ProjectTo<BillDetailViewModel>().ToList();
            billVm.BillDetails = billDetailVm;
            return billVm;
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = Mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _unitOfWork.BillDetailRepository.Add(billDetail);
            _unitOfWork.Commit();
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var detail = _unitOfWork.BillDetailRepository.FindSingle(x => x.ProductId == productId
                                                                          && x.BillId == billId && x.ColorId == colorId && x.SizeId == sizeId);
            _unitOfWork.BillDetailRepository.Remove(detail);
            _unitOfWork.Commit();
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var order = _unitOfWork.BillRepository.FindById(billId);
            order.BillStatus = status;
            _unitOfWork.BillRepository.Update(order);

            _unitOfWork.Commit();
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            return _unitOfWork.BillDetailRepository
                .FindAll(x => x.BillId == billId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            return _unitOfWork.ColorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public List<SizeViewModel> GetSizes()
        {
            return _unitOfWork.SizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }
    }
}
