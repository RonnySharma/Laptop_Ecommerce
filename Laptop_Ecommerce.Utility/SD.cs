using Laptop_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Utility
{
    public static class SD
    {
                  //Roles For Authentication And Authorization
        public const string Role_Admin = "Admin";
        public const string Role_Individual = "Individual User";
        public const string Role_Company = "Company User";
        public const string Role_Employee = "Employee User";
        //Session
        public const string Ss_CartSessionCount = "Cart Count Session";
        //OrderStatus
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProgress = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";
        //PaymentStatus
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDelay";
        public const string PaymentStatusRejected = "Rejected";

        //PriceBasedOnQuantity
        public static double GetPriceBasedOnQuentity(double quantity,double price,double price10,double price20)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price10;
            else return price20;
        }

        //ConvertToRawHtml
        public static string ConverToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            for(int i=0;i<source.Length;i++)
            {
                char let = source[i];
                if(let=='<')
                {
                    inside = true;
                    continue;
                }
                if(let=='>')
                {
                    inside = false;
                    continue;
                }
                if(!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }


    }
}
