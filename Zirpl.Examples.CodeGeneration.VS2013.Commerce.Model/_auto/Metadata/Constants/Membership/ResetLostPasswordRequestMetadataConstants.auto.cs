﻿using System;
using System.Collections;
using System.Collections.Generic;
using Zirpl.AppEngine.Model;
using Zirpl.AppEngine.Model.Metadata.Constants;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Metadata.Constants.Membership
{
    public partial class ResetLostPasswordRequestMetadataConstants
    {
        public const string NewPassword_Name = "NewPassword";
		public const bool NewPassword_IsRequired = true;
		public const bool NewPassword_IsMaxLength = false;
        public const int NewPassword_MinLength = 6;
		public const int NewPassword_MaxLength = 1024;

        public const string UserId_Name = "UserId";
		public const bool UserId_IsRequired = true;

	}
}
