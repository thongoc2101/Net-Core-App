using System.Collections.Generic;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Dtos;

namespace NetCoreApp.Application.Interfaces
{
    public interface IBillService
    {
        void Create(BillViewModel billVm);

        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);

        BillViewModel GetDetail(int billId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        void DeleteDetail(int productId, int billId, int colorId, int sizeId);

        void UpdateStatus(int billId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();
    }
}
