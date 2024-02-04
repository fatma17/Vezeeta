using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core;

namespace Vezeeta.Serivce
{
    public static class CouponService
    {
        public static int CalculationFinalPrice(int Price ,int value , DiscountType Type )
        {
            if(Type ==DiscountType.Percentage )
            {
                var OfferValue = (Price * value)/100;
                return Price - OfferValue ;
            }
            var finalprice = Price - value;

            return  ( finalprice >= 0 )? finalprice : 0 ;
        }
    }
}
