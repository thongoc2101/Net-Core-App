using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Application.ViewModels.Common;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Extensions;

namespace NetCoreApp.Models
{
    public class CheckoutViewModel: BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }

        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[]) Enum.GetValues(typeof(PaymentMethod))).Select(x => new EnumModel
                {
                    Value = (int) x,
                    Name = x.GetDescription()
                }).ToList();
            }
        }
    }
}
