﻿using System;
using System.Linq;
using Zirpl.AppEngine.DataService;
using Zirpl.AppEngine.DataService.EntityFramework;
using Zirpl.AppEngine.DataService.EntityFramework.Mapping;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Orders;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService.Mapping.Orders
{
    public partial class DiscountUsageMapping : CoreEntityMappingBase<DiscountUsage, int>
    {
		protected override void MapProperties()
        {

			this.Property(o => o.DateUsed).IsRequired(DiscountUsageMetadata.DateUsed_IsRequired).IsDateTime();

            this.HasNavigationProperty(o => o.Discount,
                                        o => o.DiscountId,
                                        DiscountUsageMetadata.Discount_IsRequired,
                                        CascadeOnDeleteOption.No);

            this.HasNavigationProperty(o => o.Order,
                                        o => o.OrderId,
                                        DiscountUsageMetadata.Order_IsRequired,
                                        CascadeOnDeleteOption.No,
										o => o.DiscountUsages);

            this.HasNavigationProperty(o => o.OrderItem,
                                        o => o.OrderItemId,
                                        DiscountUsageMetadata.OrderItem_IsRequired,
                                        CascadeOnDeleteOption.No,
										o => o.DiscountUsages);

			this.MapCustomProperties();

            base.MapProperties();
        }
		
		partial void MapCustomProperties();
		
    }
}