﻿using System;
using System.Collections;
using System.Collections.Generic;
using Zirpl.AppEngine.Model;
using Zirpl.AppEngine.Model.Metadata.Constants;

namespace Zirpl.Examples.CodeGeneration.VS2013.Commerce.Model.Metadata.Constants.Membership
{
    public partial class LoginRequestMetadataConstants
    {
        public const string EmailAddress_Name = "EmailAddress";
		public const bool EmailAddress_IsRequired = true;
		public const bool EmailAddress_IsMaxLength = false;
        public const int EmailAddress_MinLength = 3;
		public const int EmailAddress_MaxLength = 1024;

        public const string Password_Name = "Password";
		public const bool Password_IsRequired = true;
		public const bool Password_IsMaxLength = false;
        public const int Password_MinLength = 6;
		public const int Password_MaxLength = 1024;

        public const string RememberMe_Name = "RememberMe";
		public const bool RememberMe_IsRequired = true;

	}
}
