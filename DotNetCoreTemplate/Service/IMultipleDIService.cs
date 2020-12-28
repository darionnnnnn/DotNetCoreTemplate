using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreTemplate.Service
{
    // public interface IMultipleDIService
    // {
    // }

    public enum PayType
    {
        CashOnDelivery,
        CreditCard,
        WhateverPay
    }

    public interface IPayService
    {
        PayType PayType { get; }
        void Deduction(int amount);
    }
}
