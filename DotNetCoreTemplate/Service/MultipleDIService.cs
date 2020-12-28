using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreTemplate.Service
{
    // public class MultipleDIService
    // {
    // }

    public class CashOnDeliveryService : IPayService
    {
        public PayType PayType { get; } = PayType.CashOnDelivery;
        public void Deduction(int amount)
        {
        }
    }

    public class CreditCardService : IPayService
    {
        public PayType PayType { get; } = PayType.CreditCard;
        public void Deduction(int amount)
        {
        }
    }

    public class WhateverPayService : IPayService
    {
        public PayType PayType { get; } = PayType.WhateverPay;
        public void Deduction(int amount)
        {
        }
    }
}
