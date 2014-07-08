﻿using System;
using Zirpl.AppEngine.Service;
using Zirpl.AppEngine.Service.EntityFramework;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService.Promotions;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.DataService;
using Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Promotions;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.Service.Promotions
{
    public partial class DiscountAmountTypeService  : DictionaryEntityService<CommerceDataContext, DiscountAmountType, byte, DiscountAmountTypeEnum>, IDiscountAmountTypeService
    {
    }
}
