﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zirpl.AppEngine.Model;
using Zirpl.AppEngine.Model.Customization;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Orders
{
    public partial class ShoppingCartItem  : EntityBase<int>
    {
		public virtual int Quantity { get; set; }
		public virtual Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Catalog.DisplayProduct DisplayProduct { get; set; }
		public virtual int DisplayProductId { get; set; }
		public virtual Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Membership.Visitor Visitor { get; set; }
		public virtual int VisitorId { get; set; }
		public virtual Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Catalog.SubscriptionChoice SubscriptionChoice { get; set; }
		public virtual int? SubscriptionChoiceId { get; set; }
		public virtual bool AddedWhileAnonymous { get; set; }
    }
}

