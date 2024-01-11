using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_Uitility
{
    public static class SD
    {
        //store procedure covertype
        public const string Proc_GetCoverTypes = "GetCovertTypes";
        public const string Proc_GetCoverType = "GetCoverType";       
        public const string Proc_CreateCoverType = "CreateCoverType";
        public const string Proc_UpdateCoverType = "UpdateCoverType";
        public const string Proc_DeleteCoverType = "DeleteCoverType";

        //store procedure category
        public const string Proc_GetCategories = "Getcategories";
        public const string Proc_GetCategory = "GetCategory";
        public const string Proc_CreateCategory = "CreateCategory";
        public const string Proc_UpdateCategory = "UpdateCategory";
        public const string Proc_DeleteCategory = "DeleteCategory";

        //Add Roles
        public const string Role_Admin = "Admin";
        public const string Role_Employe = "EmployeUser";
        public const string Role_Company = "CompanyUser";
        public const string Role_Individual = "IndividualUser";

        //Session
        public const string Ss_CartSessionCount = "Cart Count Session";

        //order status
        public const string OrderStatusPending="Pending";
        public const string OrderStatusApproved= "Approved";
        public const string OrderStatusInProgress= "InProgress";
        public const string OrderStatusShipped= "Shipped";
        public const string OrderStatusCancelled= "Cancelled";
        public const string OrderStatusRefunded= "Refunded"; 

        //payment status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDealy";
        public const string PaymentStatusRejected = "Rejected";

        public static double GetPriceBasedOnQuantity(double Quantity,double Price,double Price50,double Price100)
        {
            if (Quantity < 50)
                return Price;
            else if (Quantity < 100)
                return Price50;
            else
                return Price100;
        }
        public static string ConvertToRawHtml(string source)
        {
            char[]array=new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;
            for(int i=0;i<source.Length;i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
