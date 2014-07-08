﻿using System;
using System.Linq;
using Zirpl.AppEngine.DataService;
using Zirpl.AppEngine.DataService.EntityFramework;
using Zirpl.AppEngine.DataService.EntityFramework.Mapping;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Orders;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService.Mapping.Orders
{
    public partial class SubscriptionOrderItemTypeMapping : DictionaryEntityMapping<SubscriptionOrderItemType, byte, SubscriptionOrderItemTypeEnum>
    {
		protected override void MapProperties()
        {

			this.Property(o => o.Name).IsRequired(SubscriptionOrderItemTypeMetadata.Name_IsRequired).HasMaxLength(SubscriptionOrderItemTypeMetadata.Name_MaxLength, SubscriptionOrderItemTypeMetadata.Name_IsMaxLength);

			this.MapCustomProperties();

            base.MapProperties();
        }
		
		partial void MapCustomProperties();
		
    }
}