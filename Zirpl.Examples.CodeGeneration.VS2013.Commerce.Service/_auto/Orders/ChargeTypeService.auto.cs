﻿using System;
using Zirpl.AppEngine.Service;
using Zirpl.AppEngine.Service.EntityFramework;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService.Orders;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Orders;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.Service.Orders
{
    public partial class ChargeTypeService  : DictionaryEntityService<CommerceDataContext, ChargeType, byte, ChargeTypeEnum>, IChargeTypeService
    {
    }
}
