--missing setting
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.HideProductsOnCategoriesHomePage')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.HideProductsOnCategoriesHomePage', N'false', N'Determines whether to display products in category treeview within administration')
END
GO

--new resource strings
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Content.CopyrightNotice')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Content.CopyrightNotice', N'Copyright &copy; {0} {1}. All rights reserved.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Media.Product.ImageAlternateTextFormat')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Media.Product.ImageAlternateTextFormat', N'Picture of {0}')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Media.Category.ImageAlternateTextFormat')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Media.Category.ImageAlternateTextFormat', N'Picture for category {0}')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Media.Product.ImageLinkTitleFormat')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Media.Product.ImageLinkTitleFormat', N'Show details for {0}')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Media.Category.ImageLinkTitleFormat')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Media.Category.ImageLinkTitleFormat', N'Show products in category {0}')
END
GO



--tax display type
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='TaxDisplayTypeID')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD TaxDisplayTypeID INT NOT NULL CONSTRAINT [DF_Nop_Customer_TaxDisplayTypeID] DEFAULT ((1))
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsAdmin,
		IsGuest,
		Active,
		Deleted,
		RegistrationDate
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsAdmin,
		@IsGuest,
		@Active,
		@Deleted,
		@RegistrationDate
	)

	set @CustomerId=@@identity
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate
	WHERE
		[CustomerId] = @CustomerId

END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TaxInclusive')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TaxInclusive', N'Show Prices Tax Inclusive')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TaxExclusive')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TaxExclusive', N'Show Prices Tax Exclusive')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.InclTaxSuffix')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.InclTaxSuffix', N'{0} incl tax')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.ExclTaxSuffix')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.ExclTaxSuffix', N'{0} excl tax')
END
GO



IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotalInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderSubtotalInclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderSubtotalInclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotalExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderSubtotalExclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderSubtotalExclTax] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotal')
BEGIN
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderSubtotalInclTax=OrderSubtotal')
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderSubtotalExclTax=OrderSubtotal')
	
	ALTER TABLE [dbo].[Nop_Order] 
	DROP COLUMN OrderSubtotal
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotalInclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderSubtotalInclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderSubtotalInclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotalExclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderSubtotalExclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderSubtotalExclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderSubtotalInCustomerCurrency')
BEGIN
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderSubtotalInclTaxInCustomerCurrency=OrderSubtotalInCustomerCurrency')
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderSubtotalExclTaxInCustomerCurrency=OrderSubtotalInCustomerCurrency')
	
	ALTER TABLE [dbo].[Nop_Order] 
	DROP COLUMN OrderSubtotalInCustomerCurrency
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='CustomerTaxDisplayTypeID')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD CustomerTaxDisplayTypeID INT NOT NULL CONSTRAINT [DF_Nop_Order_CustomerTaxDisplayTypeID] DEFAULT ((1))
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShippingInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderShippingInclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderShippingInclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShippingExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderShippingExclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderShippingExclTax] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShipping')
BEGIN
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderShippingInclTax=OrderShipping')
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderShippingExclTax=OrderShipping')
	
	ALTER TABLE [dbo].[Nop_Order] 
	DROP COLUMN OrderShipping
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShippingInclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderShippingInclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderShippingInclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShippingExclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD OrderShippingExclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_OrderShippingExclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='OrderShippingInCustomerCurrency')
BEGIN
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderShippingInclTaxInCustomerCurrency=OrderShippingInCustomerCurrency')
	
	exec('UPDATE [dbo].[Nop_Order] SET OrderShippingExclTaxInCustomerCurrency=OrderShippingInCustomerCurrency')
	
	ALTER TABLE [dbo].[Nop_Order] 
	DROP COLUMN OrderShippingInCustomerCurrency
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderInsert]
(
	@OrderID int = NULL output,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Order]
	(
		OrderGUID,
		CustomerID,
		CustomerLanguageID,
		CustomerTaxDisplayTypeID,
		OrderSubtotalInclTax,
		OrderSubtotalExclTax,
		OrderShippingInclTax,
		OrderShippingExclTax,
		OrderTax,
		OrderTotal,
		OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency,
		OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency,
		CustomerCurrencyCode,
		OrderWeight,
		AffiliateID,
		OrderStatusID,
		CardType,
		CardName,
		CardNumber,
		CardCVV2,
		CardExpirationMonth,
		CardExpirationYear,
		PaymentMethodID,
		PaymentMethodName,
		AuthorizationTransactionID,
		AuthorizationTransactionCode,
		AuthorizationTransactionResult,
		CaptureTransactionID,
		CaptureTransactionResult,
		PurchaseOrderNumber,
		PaymentStatusID,
		BillingFirstName,
		BillingLastName,
		BillingPhoneNumber,
		BillingEmail,
		BillingFaxNumber,
		BillingCompany,
		BillingAddress1,
		BillingAddress2,
		BillingCity,
		BillingStateProvince,
		BillingStateProvinceID,
		BillingZipPostalCode,
		BillingCountry,
		BillingCountryID,
		ShippingStatusID,
		ShippingFirstName,
		ShippingLastName,
		ShippingPhoneNumber,
		ShippingEmail,
		ShippingFaxNumber,
		ShippingCompany,
		ShippingAddress1,
		ShippingAddress2,
		ShippingCity,
		ShippingStateProvince,
		ShippingZipPostalCode,
		ShippingStateProvinceID,
		ShippingCountry,
		ShippingCountryID,
		ShippingMethod,
		ShippingRateComputationMethodID,
		ShippedDate,
		Deleted,
		CreatedOn
	)
	VALUES
	(
		@OrderGUID,
		@CustomerID,
		@CustomerLanguageID,		
		@CustomerTaxDisplayTypeID,
		@OrderSubtotalInclTax,
		@OrderSubtotalExclTax,		
		@OrderShippingInclTax,
		@OrderShippingExclTax,
		@OrderTax,
		@OrderTotal,
		@OrderDiscount,		
		@OrderSubtotalInclTaxInCustomerCurrency,
		@OrderSubtotalExclTaxInCustomerCurrency,		
		@OrderShippingInclTaxInCustomerCurrency,
		@OrderShippingExclTaxInCustomerCurrency,
		@OrderTaxInCustomerCurrency,
		@OrderTotalInCustomerCurrency,
		@CustomerCurrencyCode,
		@OrderWeight,
		@AffiliateID,
		@OrderStatusID,
		@CardType,
		@CardName,
		@CardNumber,
		@CardCVV2,
		@CardExpirationMonth,
		@CardExpirationYear,
		@PaymentMethodID,
		@PaymentMethodName,
		@AuthorizationTransactionID,
		@AuthorizationTransactionCode,
		@AuthorizationTransactionResult,
		@CaptureTransactionID,
		@CaptureTransactionResult,
		@PurchaseOrderNumber,
		@PaymentStatusID,
		@BillingFirstName,
		@BillingLastName,
		@BillingPhoneNumber,
		@BillingEmail,
		@BillingFaxNumber,
		@BillingCompany,
		@BillingAddress1,
		@BillingAddress2,
		@BillingCity,
		@BillingStateProvince,
		@BillingStateProvinceID,
		@BillingZipPostalCode,
		@BillingCountry,
		@BillingCountryID,
		@ShippingStatusID,
		@ShippingFirstName,
		@ShippingLastName,
		@ShippingPhoneNumber,
		@ShippingEmail,
		@ShippingFaxNumber,
		@ShippingCompany,
		@ShippingAddress1,
		@ShippingAddress2,
		@ShippingCity,
		@ShippingStateProvince,
		@ShippingZipPostalCode,
		@ShippingStateProvinceID,
		@ShippingCountry,
		@ShippingCountryID,
		@ShippingMethod,
		@ShippingRateComputationMethodID,
		@ShippedDate,
		@Deleted,
		@CreatedOn
	)

	set @OrderID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderUpdate]
(
	@OrderID int,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Order]
	SET
		OrderGUID=@OrderGUID,
		CustomerID=@CustomerID,
		CustomerLanguageID=@CustomerLanguageID,		
		CustomerTaxDisplayTypeID=@CustomerTaxDisplayTypeID,
		OrderSubtotalInclTax=@OrderSubtotalInclTax,
		OrderSubtotalExclTax=@OrderSubtotalExclTax,		
		OrderShippingInclTax=@OrderShippingInclTax,
		OrderShippingExclTax=@OrderShippingExclTax,
		OrderTax=@OrderTax,
		OrderTotal=@OrderTotal,
		OrderDiscount=@OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency=@OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency=@OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency=@OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency=@OrderShippingExclTaxInCustomerCurrency,		
		OrderTaxInCustomerCurrency=@OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency=@OrderTotalInCustomerCurrency,
		CustomerCurrencyCode=@CustomerCurrencyCode,
		OrderWeight=@OrderWeight,
		AffiliateID=@AffiliateID,
		OrderStatusID=@OrderStatusID,
		CardType=@CardType,
		CardName=@CardName,
		CardNumber=@CardNumber,
		CardCVV2=@CardCVV2,
		CardExpirationMonth=@CardExpirationMonth,
		CardExpirationYear=@CardExpirationYear,
		PaymentMethodID=@PaymentMethodID,
		PaymentMethodName=@PaymentMethodName,
		AuthorizationTransactionID=@AuthorizationTransactionID,
		AuthorizationTransactionCode=@AuthorizationTransactionCode,
		AuthorizationTransactionResult=@AuthorizationTransactionResult,
		CaptureTransactionID=@CaptureTransactionID,
		CaptureTransactionResult=@CaptureTransactionResult,
		PurchaseOrderNumber=@PurchaseOrderNumber,
		PaymentStatusID=@PaymentStatusID,
		BillingFirstName=@BillingFirstName,
		BillingLastName=@BillingLastName,
		BillingPhoneNumber=@BillingPhoneNumber,
		BillingEmail=@BillingEmail,
		BillingFaxNumber=@BillingFaxNumber,
		BillingCompany=@BillingCompany,
		BillingAddress1=@BillingAddress1,
		BillingAddress2=@BillingAddress2,
		BillingCity=@BillingCity,
		BillingStateProvince=@BillingStateProvince,
		BillingStateProvinceID=@BillingStateProvinceID,
		BillingZipPostalCode=@BillingZipPostalCode,
		BillingCountry=@BillingCountry,
		BillingCountryID=@BillingCountryID,
		ShippingStatusID=@ShippingStatusID,
		ShippingFirstName=@ShippingFirstName,
		ShippingLastName=@ShippingLastName,
		ShippingPhoneNumber=@ShippingPhoneNumber,
		ShippingEmail=@ShippingEmail,
		ShippingFaxNumber=@ShippingFaxNumber,
		ShippingCompany=@ShippingCompany,
		ShippingAddress1=@ShippingAddress1,
		ShippingAddress2=@ShippingAddress2,
		ShippingCity=@ShippingCity,
		ShippingStateProvince=@ShippingStateProvince,
		ShippingStateProvinceID=@ShippingStateProvinceID,
		ShippingZipPostalCode=@ShippingZipPostalCode,
		ShippingCountry=@ShippingCountry,
		ShippingCountryID=@ShippingCountryID,
		ShippingMethod=@ShippingMethod,
		ShippingRateComputationMethodID=@ShippingRateComputationMethodID,
		ShippedDate=@ShippedDate,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn
	WHERE
		OrderID = @OrderID
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='UnitPriceInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD UnitPriceInclTax MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_UnitPriceInclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='UnitPriceExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD UnitPriceExclTax MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_UnitPriceExclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='PriceInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD PriceInclTax MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_PriceInclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='PriceExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD PriceExclTax MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_PriceExclTax] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='Price')
BEGIN
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET UnitPriceInclTax=Price/Quantity')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET UnitPriceExclTax=Price/Quantity')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET PriceInclTax=Price')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET PriceExclTax=Price')
	
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	DROP COLUMN Price
END
GO



IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='UnitPriceInclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD UnitPriceInclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_UnitPriceInclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='UnitPriceExclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD UnitPriceExclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_UnitPriceExclTaxInCustomerCurrency] DEFAULT ((0))
END
GO


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='PriceInclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD PriceInclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_PriceInclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='PriceExclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD PriceExclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_PriceExclTaxInCustomerCurrency] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='PriceInCustomerCurrency')
BEGIN
		
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET UnitPriceInclTaxInCustomerCurrency=PriceInCustomerCurrency/Quantity')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET UnitPriceExclTaxInCustomerCurrency=PriceInCustomerCurrency/Quantity')

	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET PriceInclTaxInCustomerCurrency=PriceInCustomerCurrency')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET PriceExclTaxInCustomerCurrency=PriceInCustomerCurrency')
	
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	DROP COLUMN PriceInCustomerCurrency
END
GO



IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='DiscountAmountInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD DiscountAmountInclTax decimal(18, 4) NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_DiscountAmountInclTax] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='DiscountAmountExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD DiscountAmountExclTax decimal(18, 4) NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_DiscountAmountExclTax] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='DiscountAmount')
BEGIN
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET DiscountAmountInclTax=DiscountAmount')
	
	exec('UPDATE [dbo].[Nop_OrderProductVariant] SET DiscountAmountExclTax=DiscountAmount')
	
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	DROP COLUMN DiscountAmount
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
(
	@OrderProductVariantID int = NULL output,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@TextOption nvarchar(400),
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4)
)
AS
BEGIN
	INSERT
	INTO [Nop_OrderProductVariant]
	(
		OrderID,
		ProductVariantID,
		UnitPriceInclTax,
		UnitPriceExclTax,
		PriceInclTax,
		PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency,
		AttributeDescription,
		TextOption,
		Quantity,
		DiscountAmountInclTax,
		DiscountAmountExclTax
	)
	VALUES
	(
		@OrderID,
		@ProductVariantID,
		@UnitPriceInclTax,
		@UnitPriceExclTax,
		@PriceInclTax,
		@PriceExclTax,
		@UnitPriceInclTaxInCustomerCurrency,
		@UnitPriceExclTaxInCustomerCurrency,
		@PriceInclTaxInCustomerCurrency,
		@PriceExclTaxInCustomerCurrency,
		@AttributeDescription,
		@TextOption,
		@Quantity,
		@DiscountAmountInclTax,
		@DiscountAmountExclTax
	)

	set @OrderProductVariantID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantReport]
GO

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT opv.ProductVariantID,
		(	
			select sum(opv2.PriceExclTax)
			from Nop_OrderProductVariant opv2
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or DATEDIFF(day, @StartTime, o2.CreatedOn) >= 0) and
				(@EndTime is NULL or DATEDIFF(day, @EndTime, o2.CreatedOn) <= 0) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) PriceExclTax, 
		(
			select count(1) 
			from Nop_OrderProductVariant opv2 
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or DATEDIFF(day, @StartTime, o2.CreatedOn) >= 0) and
				(@EndTime is NULL or DATEDIFF(day, @EndTime, o2.CreatedOn) <= 0) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) Total 
	FROM Nop_OrderProductVariant opv 
	INNER JOIN [Nop_Order] o 
	on o.OrderId = opv.OrderID
	WHERE
		(@StartTime is NULL or DATEDIFF(day, @StartTime, o.CreatedOn) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, o.CreatedOn) <= 0) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(o.Deleted=0)

END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'ShoppingCart.UnitPrice')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'ShoppingCart.UnitPrice', N'Price')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Wishlist.UnitPrice')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Wishlist.UnitPrice', N'Price')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Order.ProductsGrid.Total')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Order.ProductsGrid.Total', N'Total.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.PricesIncludeTax')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.PricesIncludeTax', N'false', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.TaxDisplayType')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.TaxDisplayType', N'2', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.TaxBasedOn')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.TaxBasedOn', N'1', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.AllowCustomersToSelectTaxDisplayType')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.AllowCustomersToSelectTaxDisplayType', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.DisplayTaxSuffix')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.DisplayTaxSuffix', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.HideZeroTax')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.HideZeroTax', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.HideTaxInOrderSummary')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.HideTaxInOrderSummary', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.DefaultTaxAddress.CountryID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.DefaultTaxAddress.CountryID', N'0', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.DefaultTaxAddress.StateProvinceID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.DefaultTaxAddress.StateProvinceID', N'0', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.DefaultTaxAddress.ZipPostalCode')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.DefaultTaxAddress.ZipPostalCode', N'', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.ShippingIsTaxable')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.ShippingIsTaxable', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.ShippingPriceIncludesTax')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.ShippingPriceIncludesTax', N'false', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.ShippingTaxClassID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.ShippingTaxClassID', N'0', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Shipping.ShippingOrigin.CountryID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Shipping.ShippingOrigin.CountryID', N'0', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Shipping.ShippingOrigin.StateProvinceID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Shipping.ShippingOrigin.StateProvinceID', N'0', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Shipping.ShippingOrigin.ZipPostalCode')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Shipping.ShippingOrigin.ZipPostalCode', N'', N'')
END
GO





--tier prices

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_TierPrice]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_TierPrice](
	[TierPriceID] [int] IDENTITY(1,1) NOT NULL,
	[ProductVariantID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [money] NOT NULL,
 CONSTRAINT [Nop_TierPrice_PK] PRIMARY KEY CLUSTERED 
(
	[TierPriceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TierPrice_Nop_ProductVariant'
           AND parent_obj = Object_id('Nop_TierPrice')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TierPrice
DROP CONSTRAINT FK_Nop_TierPrice_Nop_ProductVariant
GO

ALTER TABLE [dbo].[Nop_TierPrice]  WITH CHECK ADD  CONSTRAINT [FK_Nop_TierPrice_Nop_ProductVariant] FOREIGN KEY([ProductVariantID])
REFERENCES [dbo].[Nop_ProductVariant] ([ProductVariantID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TierPriceDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TierPriceDelete]
GO

CREATE PROCEDURE [dbo].[Nop_TierPriceDelete]
(
	@TierPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TierPrice]
	WHERE
		TierPriceID = @TierPriceID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TierPriceInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TierPriceInsert]
GO

CREATE PROCEDURE [dbo].[Nop_TierPriceInsert]
(
	@TierPriceID int = NULL output,
	@ProductVariantID int,
	@Quantity int,
	@Price money
)
AS
BEGIN
	INSERT
	INTO [Nop_TierPrice]
	(
		[ProductVariantID],
		[Quantity],
		[Price]
	)
	VALUES
	(
		@ProductVariantID,
		@Quantity,
		@Price
	)

	set @TierPriceID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TierPriceLoadAllByProductVariantID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TierPriceLoadAllByProductVariantID]
GO

CREATE PROCEDURE [dbo].[Nop_TierPriceLoadAllByProductVariantID]
(
	@ProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TierPrice]
	WHERE
		ProductVariantID=@ProductVariantID
	ORDER BY Quantity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TierPriceLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TierPriceLoadByPrimaryKey]
GO

CREATE PROCEDURE [dbo].[Nop_TierPriceLoadByPrimaryKey]
(
	@TierPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TierPrice]
	WHERE
		TierPriceID = @TierPriceID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TierPriceUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TierPriceUpdate]
GO

CREATE PROCEDURE [dbo].[Nop_TierPriceUpdate]
(
	@TierPriceID int,
	@ProductVariantID int,
	@Quantity int,
	@Price money
)
AS
BEGIN
	UPDATE [Nop_TierPrice]
	SET
	ProductVariantID=@ProductVariantID,
	[Quantity]=@Quantity,
	[Price]=@Price
	WHERE
		TierPriceID = @TierPriceID
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.BuyNforX')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.BuyNforX', N'Buy {0} for {1} each')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TierPricesTitle')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TierPricesTitle', N'PRICE BREAKS - The more you buy, the more you save.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TierPricesQuantityTitle')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TierPricesQuantityTitle', N'Quantity')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TierPricesPriceTitle')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TierPricesPriceTitle', N'Price')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.TierPricesQuantityFormat')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.TierPricesQuantityFormat', N'{0} +')
END
GO

--new tax rates model
IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxByCountry_Nop_Country'
           AND parent_obj = Object_id('Nop_TaxByCountry')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxByCountry
DROP CONSTRAINT FK_Nop_TaxByCountry_Nop_Country
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxByCountry_Nop_TaxCategory'
           AND parent_obj = Object_id('Nop_TaxByCountry')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxByCountry
DROP CONSTRAINT FK_Nop_TaxByCountry_Nop_TaxCategory
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxByStateProvince_Nop_StateProvince'
           AND parent_obj = Object_id('Nop_TaxByStateProvince')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxByStateProvince
DROP CONSTRAINT FK_Nop_TaxByStateProvince_Nop_StateProvince
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxByStateProvince_Nop_TaxCategory'
           AND parent_obj = Object_id('Nop_TaxByStateProvince')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxByStateProvince
DROP CONSTRAINT FK_Nop_TaxByStateProvince_Nop_TaxCategory
GO


if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_TaxRate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[Nop_TaxRate](
	[TaxRateID] [int] IDENTITY(1,1) NOT NULL,
	[TaxCategoryID] [int] NOT NULL,
	[CountryID] [int] NOT NULL,
	[StateProvinceID] [int] NOT NULL,
	[Zip] [nvarchar](50) NOT NULL,
	[Percentage] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Nop_TaxRate] PRIMARY KEY CLUSTERED 
(
	[TaxRateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxRate_Nop_Country'
           AND parent_obj = Object_id('Nop_TaxRate')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxRate
DROP CONSTRAINT FK_Nop_TaxRate_Nop_Country
GO

ALTER TABLE [dbo].[Nop_TaxRate]  WITH CHECK ADD  CONSTRAINT [FK_Nop_TaxRate_Nop_Country] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Nop_Country] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TaxRate_Nop_TaxCategory'
           AND parent_obj = Object_id('Nop_TaxRate')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TaxRate
DROP CONSTRAINT FK_Nop_TaxRate_Nop_TaxCategory
GO

ALTER TABLE [dbo].[Nop_TaxRate]  WITH CHECK ADD  CONSTRAINT [FK_Nop_TaxRate_Nop_TaxCategory] FOREIGN KEY([TaxCategoryID])
REFERENCES [dbo].[Nop_TaxCategory] ([TaxCategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

if exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_TaxByCountry]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    	INSERT INTO [dbo].[Nop_TaxRate] ([TaxCategoryID], [CountryID], [StateProvinceID], [Zip], [Percentage])
	SELECT tbc.TaxCategoryID, tbc.CountryID, 0, N'', tbc.Percentage
	FROM [dbo].[Nop_TaxByCountry] AS tbc

	DROP TABLE [dbo].[Nop_TaxByCountry]
END
GO

if exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_TaxByStateProvince]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN

	--UNDONE insert rows to [Nop_TaxRate] before dropping [Nop_TaxByStateProvince]
	DROP TABLE [dbo].[Nop_TaxByStateProvince]
END
GO







IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByCountryDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByCountryDelete]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByCountryInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByCountryInsert]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByCountryLoadAllByCountryID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByCountryLoadAllByCountryID]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByCountryLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByCountryLoadByPrimaryKey]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByCountryUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByCountryUpdate]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByStateProvinceDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByStateProvinceDelete]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByStateProvinceInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByStateProvinceInsert]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByStateProvinceLoadAllByStateProvinceID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByStateProvinceLoadAllByStateProvinceID]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByStateProvinceLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByStateProvinceLoadByPrimaryKey]
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxByStateProvinceUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxByStateProvinceUpdate]
GO











IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxRateDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxRateDelete]
GO

CREATE PROCEDURE [dbo].[Nop_TaxRateDelete]
(
	@TaxRateID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TaxRate]
	WHERE
		TaxRateID = @TaxRateID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxRateInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxRateInsert]
GO

CREATE PROCEDURE [dbo].[Nop_TaxRateInsert]
(
	@TaxRateID int = NULL output,
	@TaxCategoryID int,
	@CountryID int,
	@StateProvinceID int,
	@Zip nvarchar(50),
	@Percentage decimal(18,4)
)
AS
BEGIN
	INSERT
	INTO [Nop_TaxRate]
	(
		[TaxCategoryID],
		[CountryID],
		[StateProvinceID],
		[Zip],
		[Percentage]
	)
	VALUES
	(
		@TaxCategoryID,
		@CountryID,
		@StateProvinceID,
		@Zip,
		@Percentage
	)

	set @TaxRateID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxRateLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxRateLoadByPrimaryKey]
GO

CREATE PROCEDURE [dbo].[Nop_TaxRateLoadByPrimaryKey]
(
	@TaxRateID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TaxRate]
	WHERE
		TaxRateID = @TaxRateID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxRateUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxRateUpdate]
GO

CREATE PROCEDURE [dbo].[Nop_TaxRateUpdate]
(
	@TaxRateID int,
	@TaxCategoryID int,
	@CountryID int,
	@StateProvinceID int,
	@Zip nvarchar(50),
	@Percentage decimal(18,4)
)
AS
BEGIN
	UPDATE [Nop_TaxRate]
	SET
		TaxCategoryID=@TaxCategoryID,
		CountryID=@CountryID,
		StateProvinceID=@StateProvinceID,
		Zip=@Zip,
		Percentage=@Percentage
	WHERE
		TaxRateID = @TaxRateID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TaxRateLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TaxRateLoadAll]
GO

CREATE PROCEDURE [dbo].[Nop_TaxRateLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		 tr.*
	FROM Nop_TaxRate tr
	LEFT OUTER JOIN Nop_Country c
	ON tr.CountryID = c.CountryID
	LEFT OUTER JOIN Nop_StateProvince sp
	ON tr.StateProvinceID = sp.StateProvinceID
	ORDER BY c.DisplayOrder,c.Name, sp.DisplayOrder,sp.Name, sp.StateProvinceID, Zip, TaxCategoryID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceDelete]
GO

CREATE PROCEDURE [dbo].[Nop_StateProvinceDelete]
(
	@StateProvinceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_StateProvince]
	WHERE
		StateProvinceID = @StateProvinceID

	DELETE 
	FROM Nop_TaxRate
	WHERE
		StateProvinceID = @StateProvinceID
END
GO




--topics
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Topic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Topic](
		[TopicID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
	 CONSTRAINT [PK_Nop_Topic] PRIMARY KEY CLUSTERED 
	(
		[TopicID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_TopicLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_TopicLocalized](
		[TopicLocalizedID] [int] IDENTITY(1,1) NOT NULL,
		[TopicID] [int] NOT NULL,
		[LanguageID] [int] NOT NULL,
		[Title] [nvarchar](200) NOT NULL,
		[Body] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Nop_TopicLocalized] PRIMARY KEY CLUSTERED 
	(
		[TopicLocalizedID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TopicLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_TopicLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TopicLocalized
DROP CONSTRAINT FK_Nop_TopicLocalized_Nop_Language
GO

ALTER TABLE [dbo].[Nop_TopicLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_TopicLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_TopicLocalized_Nop_Topic'
           AND parent_obj = Object_id('Nop_TopicLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_TopicLocalized
DROP CONSTRAINT FK_Nop_TopicLocalized_Nop_Topic
GO

ALTER TABLE [dbo].[Nop_TopicLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_TopicLocalized_Nop_Topic] FOREIGN KEY([TopicID])
REFERENCES [dbo].[Nop_Topic] ([TopicID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Topic]
	ORDER BY [Name]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLoadByPrimaryKey]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Topic]
	WHERE
		TopicID = @TopicID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicDelete]
GO
CREATE PROCEDURE [dbo].[Nop_TopicDelete]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Topic]
	WHERE
		[TopicID] = @TopicID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicInsert]
GO
CREATE PROCEDURE [dbo].[Nop_TopicInsert]
(
	@TopicID int = NULL output,
	@Name nvarchar(200)
)
AS
BEGIN
	INSERT
	INTO [Nop_Topic]
	(
		[Name]
	)
	VALUES
	(
		@Name
	)

	set @TopicID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_TopicUpdate]
(
	@TopicID int,
	@Name nvarchar(200)
)
AS
BEGIN
	UPDATE [Nop_Topic]
	SET
		[Name]=@Name
	WHERE
		TopicID = @TopicID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedDelete]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedDelete]
(
	@TopicLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_TopicLocalized]
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedInsert]
(
	@TopicLocalizedID int = NULL output,
	@TopicID int,
	@LanguageID int,
	@Title nvarchar(200),
	@Body nvarchar(MAX)
)
AS
BEGIN
	INSERT
	INTO [Nop_TopicLocalized]
	(
		TopicID,
		LanguageID,
		[Title],
		Body
	)
	VALUES
	(
		@TopicID,
		@LanguageID,
		@Title,
		@Body
	)

	set @TopicLocalizedID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadAllByName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadAllByName]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadAllByName]
(
	@Name nvarchar(200)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT tl.*
	FROM [Nop_TopicLocalized] tl
	INNER JOIN [Nop_Topic] t
	ON tl.TopicID = t.TopicID
	WHERE t.[Name] = @Name
	ORDER BY tl.LanguageID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]
(
	@Name nvarchar(200),
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		tl.*
	FROM [Nop_TopicLocalized] tl
	INNER JOIN [Nop_Topic] t
	ON tl.TopicID = t.TopicID
	WHERE tl.LanguageID=@LanguageID and t.[Name] = @Name
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByPrimaryKey]
(
	@TopicLocalizedID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TopicLocalized]
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedUpdate]
(
	@TopicLocalizedID int,
	@TopicID int,
	@LanguageID int,	
	@Title nvarchar(200),
	@Body nvarchar(MAX)
)
AS
BEGIN

	UPDATE [Nop_TopicLocalized]
	SET
		TopicID=@TopicID,
		LanguageID=@LanguageID,
		[Title]=@Title,
		Body=@Body
	WHERE
		TopicLocalizedID = @TopicLocalizedID

END
GO





-- ContactUs topic
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'ContactUs')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'ContactUs')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'ContactUs' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'', N'Put your contact information here.')
	END
END
GO

-- HomePageText
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'HomePageText')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'HomePageText')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'HomePageText' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'Welcome to our store', N'Online shopping is the process consumers go through to purchase products or services
        over the Internet. An online shop, eshop, e-store, internet shop, webshop, webstore,
        online store, or virtual store evokes the physical analogy of buying products or
        services at a bricks-and-mortar retailer or in a shopping mall.')
	END
END
GO

-- ConditionsInfo
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'ConditionsOfUse')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'ConditionsOfUse')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'ConditionsOfUse' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'Conditions of use', N'<p>
                Put your conditions of use information here
            </p>
            <p>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere pretium diam.
                Morbi adipiscing lectus vitae urna. Sed at nisl ut velit imperdiet tristique. Vivamus
                posuere neque eget augue. Proin aliquet aliquam magna. Maecenas molestie est vel
                est. Maecenas ante mi, adipiscing eget, consectetur quis, viverra nec, nisl. Aenean
                ipsum urna, imperdiet sed, vulputate non, commodo feugiat, ipsum. Lorem ipsum dolor
                sit amet, consectetur adipiscing elit. Sed elementum. Donec ut pede elementum justo
                commodo elementum. Quisque commodo. Proin dictum turpis id sapien. Nullam convallis
                gravida dui. Proin metus leo, laoreet sed, ornare sed, tristique suscipit, nisi.
                Maecenas enim.
            </p>
            <p>
                Nam ut sapien. Phasellus a erat. Cras ut sapien. In rutrum est ac nisl. Phasellus
                erat. Pellentesque tempor placerat enim. Curabitur placerat, sem id pellentesque
                mollis, elit leo vehicula dolor, non gravida lectus sem non erat. Proin venenatis
                dui. Integer tristique massa vitae libero. In nibh purus, condimentum ac, elementum
                non, rutrum ut, leo.
            </p>')
	END
END
GO

-- PrivacyInfo
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'PrivacyInfo')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'PrivacyInfo')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'PrivacyInfo' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'Privacy policy', N'<p>
                Put your privacy policy information here
            </p>
            <p>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere pretium diam.
                Morbi adipiscing lectus vitae urna. Sed at nisl ut velit imperdiet tristique. Vivamus
                posuere neque eget augue. Proin aliquet aliquam magna. Maecenas molestie est vel
                est. Maecenas ante mi, adipiscing eget, consectetur quis, viverra nec, nisl. Aenean
                ipsum urna, imperdiet sed, vulputate non, commodo feugiat, ipsum. Lorem ipsum dolor
                sit amet, consectetur adipiscing elit. Sed elementum. Donec ut pede elementum justo
                commodo elementum. Quisque commodo. Proin dictum turpis id sapien. Nullam convallis
                gravida dui. Proin metus leo, laoreet sed, ornare sed, tristique suscipit, nisi.
                Maecenas enim.
            </p>
            <p>
                Nam ut sapien. Phasellus a erat. Cras ut sapien. In rutrum est ac nisl. Phasellus
                erat. Pellentesque tempor placerat enim. Curabitur placerat, sem id pellentesque
                mollis, elit leo vehicula dolor, non gravida lectus sem non erat. Proin venenatis
                dui. Integer tristique massa vitae libero. In nibh purus, condimentum ac, elementum
                non, rutrum ut, leo.
            </p>')
	END
END
GO

-- shippingInfo
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'ShippingInfo')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'ShippingInfo')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'ShippingInfo' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'Shipping & Returns', N'<p>
                Put your shipping & returns information here</p>
            <p>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras posuere pretium diam.
                Morbi adipiscing lectus vitae urna. Sed at nisl ut velit imperdiet tristique. Vivamus
                posuere neque eget augue. Proin aliquet aliquam magna. Maecenas molestie est vel
                est. Maecenas ante mi, adipiscing eget, consectetur quis, viverra nec, nisl. Aenean
                ipsum urna, imperdiet sed, vulputate non, commodo feugiat, ipsum. Lorem ipsum dolor
                sit amet, consectetur adipiscing elit. Sed elementum. Donec ut pede elementum justo
                commodo elementum. Quisque commodo. Proin dictum turpis id sapien. Nullam convallis
                gravida dui. Proin metus leo, laoreet sed, ornare sed, tristique suscipit, nisi.
                Maecenas enim.
            </p>
            <p>
                Nam ut sapien. Phasellus a erat. Cras ut sapien. In rutrum est ac nisl. Phasellus
                erat. Pellentesque tempor placerat enim. Curabitur placerat, sem id pellentesque
                mollis, elit leo vehicula dolor, non gravida lectus sem non erat. Proin venenatis
                dui. Integer tristique massa vitae libero. In nibh purus, condimentum ac, elementum
                non, rutrum ut, leo.
            </p>')
	END
END
GO

-- About Us Text
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Content.AboutUs')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Content.AboutUs', N'About Us')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'AboutUs')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'AboutUs')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'AboutUs' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'About Us', N'<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam interdum, 
		enim ut fermentum rutrum, ante orci posuere eros, nec eleifend dolor justo in tellus. 
		Donec sed nibh lacus, in aliquam erat. Lorem ipsum dolor sit amet, consectetur 
		adipiscing elit. Donec at sollicitudin quam. Phasellus magna leo, fermentum vel 
		pharetra eu, commodo nec erat. Vestibulum nec ipsum dolor. Morbi dapibus sagittis 
		sodales. Morbi sit amet nisi orci, ac blandit ligula. Vivamus rutrum dapibus fringilla. 
		Nulla congue leo id enim porttitor vestibulum. Duis orci tellus, ultricies eget vehicula 
		eget, aliquet ut diam. Proin lacinia sagittis ultrices. Ut id urna tellus, interdum egestas dui. 
		Integer tellus ligula, porta ut vulputate nec, accumsan et tellus.</p>
		<p>Ut magna lorem, mattis ut pellentesque id, ornare quis nisi. Phasellus accumsan 
		vehicula est, a egestas mauris congue eu. Mauris eu nisl nibh, non fermentum dolor. 
		Etiam viverra mattis ligula eget condimentum. Nunc molestie mauris nec enim tempor 
		molestie. Sed convallis sodales leo, ac vehicula est faucibus vitae. Cum sociis natoque 
		penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam at justo nec 
		augue blandit vestibulum vitae nec orci. Fusce metus dui, fermentum eu mattis eu, 
		tincidunt eu velit. Fusce lorem purus, auctor sed interdum non, egestas vitae neque. 
		Maecenas feugiat eros vitae dui luctus consequat. Donec lobortis blandit erat, sit amet 
		congue enim euismod at. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices 
		posuere cubilia Curae; Nam et adipiscing turpis. Nulla gravida pretium venenatis. Cras 
		vehicula molestie nisl vel lacinia. Duis sodales posuere nulla, id dapibus eros dapibus 
		tincidunt.</p>')
	END
END
GO



--pricelist missed stored procedure

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PricelistDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PricelistDelete]
GO
CREATE PROCEDURE [dbo].[Nop_PricelistDelete]
(
	@PricelistID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_PricelistDelete]
	WHERE
		PricelistID = @PricelistID
END
GO





--timezones

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='TimeZoneID')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD TimeZoneID nvarchar(200) NOT NULL CONSTRAINT [DF_Nop_Customer_TimeZoneID] DEFAULT ('')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200)
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsAdmin,
		IsGuest,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsAdmin,
		@IsGuest,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID
	)

	set @CustomerId=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200)
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID
	WHERE
		[CustomerId] = @CustomerId

END
GO



--time zones & UTC

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Warehouse_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Warehouse]'))
ALTER TABLE [dbo].[Nop_Warehouse] DROP CONSTRAINT [DF_Nop_Warehouse_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Warehouse_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Warehouse]'))
ALTER TABLE [dbo].[Nop_Warehouse] DROP CONSTRAINT [DF_Nop_Warehouse_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_TaxCategory_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_TaxCategory]'))
ALTER TABLE [dbo].[Nop_TaxCategory] DROP CONSTRAINT [DF_Nop_TaxCategory_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_TaxCategory_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_TaxCategory]'))
ALTER TABLE [dbo].[Nop_TaxCategory] DROP CONSTRAINT [DF_Nop_TaxCategory_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductTemplate_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductTemplate]'))
ALTER TABLE [dbo].[Nop_ProductTemplate] DROP CONSTRAINT [DF_Nop_ProductTemplate_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductTemplate_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductTemplate]'))
ALTER TABLE [dbo].[Nop_ProductTemplate] DROP CONSTRAINT [DF_Nop_ProductTemplate_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductType_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductType]'))
ALTER TABLE [dbo].[Nop_ProductType] DROP CONSTRAINT [DF_Nop_ProductType_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductType_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductType]'))
ALTER TABLE [dbo].[Nop_ProductType] DROP CONSTRAINT [DF_Nop_ProductType_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ManufacturerTemplate_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ManufacturerTemplate]'))
ALTER TABLE [dbo].[Nop_ManufacturerTemplate] DROP CONSTRAINT [DF_Nop_ManufacturerTemplate_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ManufacturerTemplate_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ManufacturerTemplate]'))
ALTER TABLE [dbo].[Nop_ManufacturerTemplate] DROP CONSTRAINT [DF_Nop_ManufacturerTemplate_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_CategoryTemplate_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_CategoryTemplate]'))
ALTER TABLE [dbo].[Nop_CategoryTemplate] DROP CONSTRAINT [DF_Nop_CategoryTemplate_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_CategoryTemplate_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_CategoryTemplate]'))
ALTER TABLE [dbo].[Nop_CategoryTemplate] DROP CONSTRAINT [DF_Nop_CategoryTemplate_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Customer_RegisterDate' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Customer]'))
ALTER TABLE [dbo].[Nop_Customer] DROP CONSTRAINT [DF_Nop_Customer_RegisterDate]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Currency_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Currency]'))
ALTER TABLE [dbo].[Nop_Currency] DROP CONSTRAINT [DF_Nop_Currency_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Currency_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Currency]'))
ALTER TABLE [dbo].[Nop_Currency] DROP CONSTRAINT [DF_Nop_Currency_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ShoppingCart_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ShoppingCartItem]'))
ALTER TABLE [dbo].[Nop_ShoppingCartItem] DROP CONSTRAINT [DF_Nop_ShoppingCart_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ShoppingCart_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ShoppingCartItem]'))
ALTER TABLE [dbo].[Nop_ShoppingCartItem] DROP CONSTRAINT [DF_Nop_ShoppingCart_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductVariant_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductVariant]'))
ALTER TABLE [dbo].[Nop_ProductVariant] DROP CONSTRAINT [DF_Nop_ProductVariant_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_ProductVariant_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_ProductVariant]'))
ALTER TABLE [dbo].[Nop_ProductVariant] DROP CONSTRAINT [DF_Nop_ProductVariant_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Product_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Product]'))
ALTER TABLE [dbo].[Nop_Product] DROP CONSTRAINT [DF_Nop_Product_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Product_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Product]'))
ALTER TABLE [dbo].[Nop_Product] DROP CONSTRAINT [DF_Nop_Product_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Category_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Category]'))
ALTER TABLE [dbo].[Nop_Category] DROP CONSTRAINT [DF_Category_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Category_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Category]'))
ALTER TABLE [dbo].[Nop_Category] DROP CONSTRAINT [DF_Category_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Order_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Order]'))
ALTER TABLE [dbo].[Nop_Order] DROP CONSTRAINT [DF_Nop_Order_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Manufacturer_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Manufacturer]'))
ALTER TABLE [dbo].[Nop_Manufacturer] DROP CONSTRAINT [DF_Nop_Manufacturer_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Manufacturer_UpdatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Manufacturer]'))
ALTER TABLE [dbo].[Nop_Manufacturer] DROP CONSTRAINT [DF_Nop_Manufacturer_UpdatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE [NAME]=N'DF_Nop_Address_CreatedOn' and parent_obj=OBJECT_ID(N'[dbo].[Nop_Address]'))
ALTER TABLE [dbo].[Nop_Address] DROP CONSTRAINT [DF_Nop_Address_CreatedOn]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DiscountLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DiscountLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_DiscountLoadAll]
(
	@ShowHidden bit = 0,	
	@DiscountTypeID int		/*null or 0 to load all discounts*/
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Discount] d
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1)
	and (@DiscountTypeID IS NULL or @DiscountTypeID=0 or d.DiscountTypeID = @DiscountTypeID) 
	and d.Deleted=0
	order by d.StartDate desc
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DiscountLoadByCategoryID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DiscountLoadByCategoryID]
GO
CREATE PROCEDURE [dbo].[Nop_DiscountLoadByCategoryID]
(
	@CategoryID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		d.*
	FROM [Nop_Discount] d
	INNER JOIN Nop_Category_Discount_Mapping cdm
	ON d.DiscountID = cdm.DiscountID
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1) and d.Deleted=0 and cdm.CategoryID=@CategoryID
	order by d.StartDate desc
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DiscountLoadByProductVariantID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DiscountLoadByProductVariantID]
GO
CREATE PROCEDURE [dbo].[Nop_DiscountLoadByProductVariantID]
(
	@ProductVariantID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		d.*
	FROM [Nop_Discount] d
	INNER JOIN Nop_ProductVariant_Discount_Mapping pdm
	ON d.DiscountID = pdm.DiscountID
	WHERE ((getutcdate() between d.StartDate and d.EndDate) or @ShowHidden = 1) and d.Deleted=0 and pdm.ProductVariantID=@ProductVariantID
	order by d.StartDate desc
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.Preferences')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.Preferences', N'Preferences')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.TimeZone')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.TimeZone', N'Time Zone')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.DefaultStoreTimeZoneID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.DefaultStoreTimeZoneID', N'', N'Determines default store time zone')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.AllowCustomersToSetTimeZone')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.AllowCustomersToSetTimeZone', N'false', N'Determines whether customers are allowed to select theirs time zone')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int,
	@ManufacturerID		int,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)

	--display order
	CREATE TABLE #DisplayOrder
	(
		ProductID int not null PRIMARY KEY,
		DisplayOrder int not null
	)	

	IF @CategoryID IS NOT NULL AND @CategoryID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pcm.ProductID, pcm.DisplayOrder 
			FROM [Nop_Product_Category_Mapping] pcm WHERE pcm.CategoryID = @CategoryID
		END
    ELSE IF @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pmm.ProductID, pmm.Displayorder 
			FROM [Nop_Product_Manufacturer_Mapping] pmm WHERE pmm.ManufacturerID = @ManufacturerID
		END
	ELSE
		BEGIN
			INSERT #DisplayOrder 
			SELECT p.ProductID, 1 
			FROM [Nop_Product] p
			ORDER BY p.[Name]
		END


	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		DisplayOrder int NOT NULL,
	)

	INSERT INTO #PageIndex (ProductID, DisplayOrder)
	SELECT DISTINCT
		p.ProductID, do.DisplayOrder
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID        
	JOIN #DisplayOrder do on p.ProductID = do.ProductID 
	WHERE 
		(
			(
				p.Published = 1 OR @ShowHidden = 1
			)
		AND 
			(
				pv.Published = 1 or @ShowHidden = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)
			)
		)
	ORDER BY do.DisplayOrder

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		p.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
END
GO


--Fix password recovery message template bug
UPDATE [Nop_MessageTemplateLocalized]
	SET
		[Subject]=N'%Store.Name%. Password recovery',
		Body=N'<a href="%Store.URL%">%Store.Name%</a>  <br>  <br>  To change your password <a href="%Customer.PasswordRecoveryURL%">click here</a>.     <br>  <br>  %Store.Name%'
	WHERE
		MessageTemplateLocalizedID 
	in (	SELECT mtl.MessageTemplateLocalizedID 
			FROM Nop_MessageTemplate mt
			INNER JOIN Nop_MessageTemplateLocalized mtl 
			ON mt.MessageTemplateID = mtl.MessageTemplateID
			WHERE mt.Name = 'Customer.PasswordRecovery')
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadAll]
GO

CREATE PROCEDURE [dbo].[Nop_CustomerLoadAll]
(
	@StartTime	datetime = NULL,
	@EndTime	datetime = NULL,
	@Email	nvarchar(MAX),
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN

	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'


	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		CustomerID int NOT NULL,
		RegistrationDate datetime NOT NULL,
	)

	INSERT INTO #PageIndex (CustomerID, RegistrationDate)
	SELECT DISTINCT
		c.CustomerID, c.RegistrationDate
	FROM [Nop_Customer] c with (NOLOCK)
	WHERE 
		(@StartTime is NULL or DATEDIFF(day, @StartTime, c.RegistrationDate) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, c.RegistrationDate) <= 0) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		deleted=0
	order by RegistrationDate desc 

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		c.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_Customer] c on c.CustomerID = [pi].CustomerID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
	
END
GO
--account activation
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.AccountHasBeenActivated')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.AccountHasBeenActivated', N'Your account has been activated')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.AccountActivation')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.AccountActivation', N'Account activation')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.ActivationEmailHasBeenSent')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.ActivationEmailHasBeenSent', N'Your registration has been successfully completed. <br /> You have just been sent an email containing membership activation instructions.')
END
GO

UPDATE [Nop_MessageTemplateLocalized]
	SET
		[Subject]=N'%Store.Name%. Email validation',
		Body=N'<a href="%Store.URL%">%Store.Name%</a>  <br>  <br>  To activate your account <a href="%Customer.AccountActivationURL%">click here</a>.     <br>  <br>  %Store.Name%'
	WHERE
		MessageTemplateLocalizedID 
	in (	SELECT mtl.MessageTemplateLocalizedID 
			FROM Nop_MessageTemplate mt
			INNER JOIN Nop_MessageTemplateLocalized mtl 
			ON mt.MessageTemplateID = mtl.MessageTemplateID
			WHERE mt.Name = 'Customer.EmailValidationMessage')
GO


--username support

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='Username')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD Username nvarchar(100) NOT NULL CONSTRAINT [DF_Nop_Customer_Username] DEFAULT ((''))

	exec('UPDATE [dbo].[Nop_Customer] SET Username=Email')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsAdmin,
		IsGuest,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsAdmin,
		@IsGuest,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username
	)

	set @CustomerId=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100)
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username
	WHERE
		[CustomerId] = @CustomerId

END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.UsernamesEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.UsernamesEnabled', N'false', N'Determines whether usernames are used instead of emails')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadByUsername]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadByUsername]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerLoadByUsername]
(
	@Username nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([Username] = @Username)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.Username')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.Username', N'Username')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Login.Username')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Login.Username', N'Username')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Login.UserNameRequired')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Login.UserNameRequired', N'Username is required')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Login.E-MailRequired')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Login.E-MailRequired', N'E-Mail is required')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Login.PasswordRequired')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Login.PasswordRequired', N'Password is required')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.E-MailRequired')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.E-MailRequired', N'E-Mail is required')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.UserNameRequired')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.UserNameRequired', N'Username is required')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.InvalidEmail')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.InvalidEmail', N'Invalid email')
END
GO


--Allow store owner to specify password format (SHA1 or MD5)
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Security.PasswordFormat')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Security.PasswordFormat', N'SHA1', N'Password format used for storing passwords')
END
GO

-- new shopping cart changes

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Cart')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Cart', N'Cart')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Address')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Address', N'Address')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Shipping')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Shipping', N'Shipping')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Payment')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Payment', N'Payment')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Confirm')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Confirm', N'Confirm')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'OrderProgress.Complete')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'OrderProgress.Complete', N'Complete')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ShowProductImagesOnShoppingCart')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.ShowProductImagesOnShoppingCart', N'True', N'Determines whether to show product images on the shopping cart.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.ShoppingCart.ThumbnailImageSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.ShoppingCart.ThumbnailImageSize', N'80', N'The size of thumbnail images to be displayed on the shopping cart.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ShowProductImagesOnWishList')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.ShowProductImagesOnWishList', N'True', N'Determines whether to show product images on the wish list.')
END
GO

-- Log in / register info

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'LoginRegistrationInfo')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'LoginRegistrationInfo')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'LoginRegistrationInfo' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'About login / registration', 
		N'<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam interdum, 
		enim ut fermentum rutrum, ante orci posuere eros, nec eleifend dolor justo in tellus. 
		Donec sed nibh lacus, in aliquam erat. Lorem ipsum dolor sit amet, consectetur 
		adipiscing elit. Donec at sollicitudin quam. Phasellus magna leo, fermentum vel 
		pharetra eu, commodo nec erat. Vestibulum nec ipsum dolor. Morbi dapibus sagittis 
		sodales. Morbi sit amet nisi orci, ac blandit ligula. Vivamus rutrum dapibus fringilla. 
		Nulla congue leo id enim porttitor vestibulum. Duis orci tellus, ultricies eget vehicula 
		eget, aliquet ut diam. Proin lacinia sagittis ultrices. Ut id urna tellus, interdum egestas dui. 
		Integer tellus ligula, porta ut vulputate nec, accumsan et tellus.</p>
		<p>Ut magna lorem, mattis ut pellentesque id, ornare quis nisi. Phasellus accumsan 
		vehicula est, a egestas mauris congue eu. Mauris eu nisl nibh, non fermentum dolor. 
		Etiam viverra mattis ligula eget condimentum. Nunc molestie mauris nec enim tempor 
		molestie. Sed convallis sodales leo, ac vehicula est faucibus vitae. Cum sociis natoque 
		penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam at justo nec 
		augue blandit vestibulum vitae nec orci. Fusce metus dui, fermentum eu mattis eu, 
		tincidunt eu velit. Fusce lorem purus, auctor sed interdum non, egestas vitae neque. 
		Maecenas feugiat eros vitae dui luctus consequat. Donec lobortis blandit erat, sit amet 
		congue enim euismod at. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices 
		posuere cubilia Curae; Nam et adipiscing turpis. Nulla gravida pretium venenatis. Cras 
		vehicula molestie nisl vel lacinia. Duis sodales posuere nulla, id dapibus eros dapibus 
		tincidunt.</p>')
	END
END
GO

-- theme selector
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.PublicStoreTheme')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.PublicStoreTheme', N'darkOrange', N'The public store theme')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.SystemThemes')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.SystemThemes', N'administration,install,print', N'Comma separated list of system themes that should not included in the theme selector within administration')
END
GO

-- default image setting

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.DefaultImageName')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.DefaultImageName', N'noDefaultImage.gif', N'The default image name. Must be stored in the /images directory')
END
GO


--forum support

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_Group](
		[ForumGroupID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
		[Description] [nvarchar](MAX) NOT NULL,
		[DisplayOrder] [int] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_Group] PRIMARY KEY CLUSTERED 
	(
		[ForumGroupID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO




if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_Forum]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_Forum](
		[ForumID] [int] IDENTITY(1,1) NOT NULL,
		[ForumGroupID] [int] NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
		[Description] [nvarchar](MAX) NULL,
		[NumTopics] [int] NOT NULL,
		[NumPosts] [int] NOT NULL,
		[LastTopicID] [int] NOT NULL,
		[LastPostID] [int] NOT NULL,
		[LastPostUserID] [int] NOT NULL,
		[LastPostTime] [datetime] NULL,
		[DisplayOrder] [int] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_Forum] PRIMARY KEY CLUSTERED 
	(
		[ForumID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Forums_Forum_Nop_Forums_Group'
           AND parent_obj = Object_id('Nop_Forums_Forum')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Forums_Forum
DROP CONSTRAINT FK_Nop_Forums_Forum_Nop_Forums_Group
GO
ALTER TABLE [dbo].[Nop_Forums_Forum]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Forums_Forum_Nop_Forums_Group] FOREIGN KEY([ForumGroupID])
REFERENCES [dbo].[Nop_Forums_Group] ([ForumGroupID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_Topic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_Topic](
		[TopicID] [int] IDENTITY(1,1) NOT NULL,
		[ForumID] [int] NOT NULL,
		[UserID] [int] NOT NULL,
		[TopicTypeID] [int] NOT NULL,
		[Subject] [nvarchar](450) NOT NULL,
		[NumPosts] [int] NOT NULL,
		[Views] [int] NOT NULL,
		[LastPostID] [int] NOT NULL,
		[LastPostUserID] [int] NOT NULL,
		[LastPostTime] [datetime] NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_Topic] PRIMARY KEY CLUSTERED 
	(
		[TopicID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Forums_Topic_Nop_Forums_Forum'
           AND parent_obj = Object_id('Nop_Forums_Topic')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Forums_Topic
DROP CONSTRAINT FK_Nop_Forums_Topic_Nop_Forums_Forum
GO
ALTER TABLE [dbo].[Nop_Forums_Topic]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Forums_Topic_Nop_Forums_Forum] FOREIGN KEY([ForumID])
REFERENCES [dbo].[Nop_Forums_Forum] ([ForumID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_Post]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_Post](
		[PostID] [int] IDENTITY(1,1) NOT NULL,
		[TopicID] [int] NOT NULL,
		[UserID] [int] NOT NULL,
		[Text] [nvarchar](MAX) NOT NULL,
		[IPAddress] [nvarchar](100) NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_Post] PRIMARY KEY CLUSTERED 
	(
		[PostID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Forums_Post_Nop_Forums_Topic'
           AND parent_obj = Object_id('Nop_Forums_Post')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Forums_Post
DROP CONSTRAINT FK_Nop_Forums_Post_Nop_Forums_Topic
GO
ALTER TABLE [dbo].[Nop_Forums_Post]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Forums_Post_Nop_Forums_Topic] FOREIGN KEY([TopicID])
REFERENCES [dbo].[Nop_Forums_Topic] ([TopicID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_Subscription]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_Subscription](
		[SubscriptionID] [int] IDENTITY(1,1) NOT NULL,
		[SubscriptionGUID] [uniqueidentifier] NOT NULL,
		[UserID] [int] NOT NULL,
		[ForumID] [int] NOT NULL,
		[TopicID] [int] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_Subscription] PRIMARY KEY CLUSTERED 
	(
		[SubscriptionID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_Forums_PrivateMessage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_Forums_PrivateMessage](
		[PrivateMessageID] [int] IDENTITY(1,1) NOT NULL,
		[FromUserID] [int] NOT NULL,
		[ToUserID] [int] NOT NULL,
		[Subject] [nvarchar](450) NOT NULL,
		[Text] [nvarchar](MAX) NOT NULL,
		[IsRead] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Nop_Forums_PrivateMessage] PRIMARY KEY CLUSTERED 
	(
		[PrivateMessageID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO



IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='IsForumModerator')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD IsForumModerator bit NOT NULL CONSTRAINT [DF_Nop_Customer_IsForumModerator] DEFAULT ((0))

	exec('UPDATE [dbo].[Nop_Customer] SET IsForumModerator=IsAdmin')
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='TotalForumPosts')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD TotalForumPosts int NOT NULL CONSTRAINT [DF_Nop_Customer_TotalForumPosts] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdateCounts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdateCounts]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdateCounts]
(
	@CustomerID int
)
AS
BEGIN

	DECLARE @NumPosts int

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Post] fp
	WHERE
		fp.UserID = @CustomerID

	SET @NumPosts = isnull(@NumPosts, 0)
	
	SET NOCOUNT ON
	UPDATE 
		[Nop_Customer]
	SET
		[TotalForumPosts] = @NumPosts
	WHERE
		CustomerID = @CustomerID
END
GO




IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumUpdateCounts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumUpdateCounts]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumUpdateCounts]
(
	@ForumID int
)
AS
BEGIN

	DECLARE @NumTopics int
	DECLARE @NumPosts int
	DECLARE @LastTopicID int
	DECLARE @LastPostID int
	DECLARE @LastPostUserID int
	DECLARE @LastPostTime datetime

	SELECT 
		@NumTopics =COUNT(1) 
	FROM [Nop_Forums_Topic]
	WHERE [ForumID] = @ForumID

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Topic] ft
		INNER JOIN [Nop_Forums_Post] fp on ft.TopicID=fp.TopicID
	WHERE
		ft.ForumID = @ForumID


	SELECT TOP 1
		@LastTopicID = ft.TopicID,
		@LastPostID = fp.PostID,
		@LastPostUserID = fp.UserID,
		@LastPostTime = fp.CreatedOn
	FROM
		[Nop_Forums_Topic] ft
		LEFT OUTER JOIN [Nop_Forums_Post] fp on ft.TopicID=fp.TopicID
	WHERE
		ft.ForumID = @ForumID
	ORDER BY fp.CreatedOn desc, ft.CreatedOn desc

	SET @NumTopics = isnull(@NumTopics, 0)
	SET @NumPosts = isnull(@NumPosts, 0)
	SET @LastTopicID = isnull(@LastTopicID, 0)
	SET @LastPostID = isnull(@LastPostID, 0)
	SET @LastPostUserID = isnull(@LastPostUserID, 0)

	SET NOCOUNT ON
	UPDATE 
		[Nop_Forums_Forum]
	SET
		[NumTopics] = @NumTopics,
		[NumPosts] = @NumPosts,
		[LastTopicID] = @LastTopicID,
		[LastPostID] = @LastPostID,
		[LastPostUserID] = @LastPostUserID,
		[LastPostTime] = @LastPostTime
	WHERE
		ForumID = @ForumID
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicUpdateCounts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicUpdateCounts]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicUpdateCounts]
(
	@TopicID int
)
AS
BEGIN

	DECLARE @NumPosts int
	DECLARE @LastPostID int
	DECLARE @LastPostUserID int
	DECLARE @LastPostTime datetime

	SELECT 
		@NumPosts = COUNT(1)
	FROM
		[Nop_Forums_Post] fp
	WHERE
		fp.TopicID = @TopicID


	SELECT TOP 1
		@LastPostID = fp.PostID,
		@LastPostUserID = fp.UserID,
		@LastPostTime = fp.CreatedOn
	FROM [Nop_Forums_Post] fp
	WHERE
		fp.TopicID = @TopicID
	ORDER BY fp.CreatedOn desc

	SET @NumPosts = isnull(@NumPosts, 0)
	SET @LastPostID = isnull(@LastPostID, 0)
	SET @LastPostUserID = isnull(@LastPostUserID, 0)

	SET NOCOUNT ON
	UPDATE 
		[Nop_Forums_Topic]
	SET
		[NumPosts] = @NumPosts,
		[LastPostID] = @LastPostID,
		[LastPostUserID] = @LastPostUserID,
		[LastPostTime] = @LastPostTime
	WHERE
		TopicID = @TopicID
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_GroupDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_GroupDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_GroupDelete]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_Group]
	WHERE
		ForumGroupID = @ForumGroupID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_GroupInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_GroupInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_GroupInsert]
(
	@ForumGroupID int = NULL output,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Group]
	(
		[Name],
		[Description],
		DisplayOrder,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@Description,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @ForumGroupID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_GroupLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_GroupLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_GroupLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Forums_Group]
	order by DisplayOrder
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_GroupLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_GroupLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_GroupLoadByPrimaryKey]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Group]
	WHERE
		(ForumGroupID = @ForumGroupID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_GroupUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_GroupUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_GroupUpdate]
(
	@ForumGroupID int,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Group]
	SET
		[Name]=@Name,
		[Description]=@Description,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ForumGroupID = @ForumGroupID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumDelete]
(
	@ForumID int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM [Nop_Forums_Subscription]	
	WHERE
		TopicID in (	SELECT ft.TopicID 
						FROM [Nop_Forums_Topic] ft
						WHERE ft.ForumID=@ForumID)

	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		ForumID = @ForumID

	DELETE
	FROM [Nop_Forums_Forum]
	WHERE
		ForumID = @ForumID
	
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumInsert]
(
	@ForumID int = NULL output,	
	@ForumGroupID int,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@NumTopics int,
	@NumPosts int,
	@LastTopicID int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Forum]
	(
		[ForumGroupID],
		[Name],
		[Description],
		[NumTopics],
		[NumPosts],
		[LastTopicID],
		[LastPostID],
		[LastPostUserID],
		[LastPostTime],
		[DisplayOrder],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@ForumGroupID,
		@Name,
		@Description,
		@NumTopics,
		@NumPosts,
		@LastTopicID,
		@LastPostID,
		@LastPostUserID,
		@LastPostTime,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @ForumID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumLoadByPrimaryKey]
(
	@ForumID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Forum]
	WHERE
		(ForumID = @ForumID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumUpdate]
(
	@ForumID int,	
	@ForumGroupID int,
	@Name nvarchar(200),
	@Description nvarchar(MAX),
	@NumTopics int,
	@NumPosts int,
	@LastTopicID int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Forum]
	SET
		ForumGroupID=@ForumGroupID,
		[Name]=@Name,
		[Description]=@Description,
		NumTopics=@NumTopics,
		NumPosts=@NumPosts,
		LastTopicID=@LastTopicID,
		LastPostID=@LastPostID,
		LastPostUserID=@LastPostUserID,
		LastPostTime=@LastPostTime,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ForumID = @ForumID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_ForumLoadAllByForumGroupID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_ForumLoadAllByForumGroupID]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_ForumLoadAllByForumGroupID]
(
	@ForumGroupID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Forums_Forum]
	where ForumGroupID=@ForumGroupID
	order by DisplayOrder
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicDelete]
(
	@TopicID int
)
AS
BEGIN
	SET NOCOUNT ON

	declare @UserID int
	declare @ForumID int
	SELECT 
		@UserID = UserID,
		@ForumID = ForumID
	FROM
		[Nop_Forums_Topic]
	WHERE
		TopicID = @TopicID 

	DELETE
	FROM [Nop_Forums_Topic]
	WHERE
		TopicID = @TopicID

	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		TopicID = @TopicID

	--update stats/info
	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
	exec [dbo].[Nop_CustomerUpdateCounts] @UserID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicInsert]
(
	@TopicID int = NULL output,
	@ForumID int,
	@UserID int,
	@TopicTypeID int,
	@Subject nvarchar(450),
	@NumPosts int,
	@Views int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Topic]
	(
		[ForumID],
		[UserID],
		[TopicTypeID],
		[Subject],
		[NumPosts],
		[Views],
		[LastPostID],
		[LastPostUserID],
		[LastPostTime],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@ForumID,
		@UserID,
		@TopicTypeID,
		@Subject,
		@NumPosts,
		@Views,
		@LastPostID,
		@LastPostUserID,
		@LastPostTime,
		@CreatedOn,
		@UpdatedOn
	)

	set @TopicID=@@identity

	--update stats/info
	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadByPrimaryKey]
(
	@TopicID int,
	@IncreaseViews bit = 0
)
AS
BEGIN
	SET NOCOUNT ON

	IF (@IncreaseViews = 1)
	BEGIN
		UPDATE [Nop_Forums_Topic]
		SET 
			[Views]=[Views]+1
		WHERE
			TopicID = @TopicID 
	END

	SELECT
		*
	FROM [Nop_Forums_Topic]
	WHERE
		(TopicID = @TopicID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicUpdate]
(
	@TopicID int,
	@ForumID int,
	@UserID int,
	@TopicTypeID int,
	@Subject nvarchar(450),
	@NumPosts int,
	@Views int,
	@LastPostID int,
	@LastPostUserID int,
	@LastPostTime datetime,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Topic]
	SET
		[ForumID]=@ForumID,
		[UserID]=@UserID,
		[TopicTypeID]=@TopicTypeID,
		[Subject]=@Subject,
		[NumPosts]=@NumPosts,
		[Views]=@Views,
		LastPostID=@LastPostID,
		LastPostUserID=@LastPostUserID,
		LastPostTime=@LastPostTime,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		TopicID = @TopicID
	
	--update stats/info
	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
(
	@ForumID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),	
	@SearchPosts		bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		TopicID int NOT NULL,
		TopicTypeID int NOT NULL,
		LastPostTime datetime NOT NULL,
	)

	INSERT INTO #PageIndex (TopicID, TopicTypeID, LastPostTime)
	SELECT DISTINCT
		ft.TopicID, ft.TopicTypeID, ft.LastPostTime
	FROM Nop_Forums_Topic ft with (NOLOCK) 
	LEFT OUTER JOIN Nop_Forums_Post fp with (NOLOCK) ON ft.TopicID = fp.TopicID
	WHERE  (
				@ForumID IS NULL OR @ForumID=0
				OR (ft.ForumID=@ForumID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (ft.UserID=@UserID)
			)
		AND	(
				(patindex(@Keywords, isnull(ft.Subject, '')) > 0)
				or (@SearchPosts = 1 and patindex(@Keywords, isnull(fp.Text, '')) > 0)
			)		
	ORDER BY ft.TopicTypeID desc, ft.LastPostTime desc, ft.TopicID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		ft.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Topic ft on ft.TopicID = [pi].TopicID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PostDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PostDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PostDelete]
(
	@PostID int
)
AS
BEGIN
	SET NOCOUNT ON

	declare @UserID int
	declare @ForumID int
	declare @TopicID int
	SELECT 
		@UserID = ft.UserID,
		@ForumID = ft.ForumID,
		@TopicID = ft.TopicID
	FROM
		[Nop_Forums_Topic] ft
		INNER JOIN 
		[Nop_Forums_Post] fp
		ON ft.TopicID=fp.TopicID
	WHERE
		fp.PostID = @PostID 
	
	DELETE
	FROM [Nop_Forums_Post]
	WHERE
		PostID = @PostID

	--update stats/info
	exec [dbo].[Nop_Forums_TopicUpdateCounts] @TopicID
	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
	exec [dbo].[Nop_CustomerUpdateCounts] @UserID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PostInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PostInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PostInsert]
(
	@PostID int = NULL output,
	@TopicID int,
	@UserID int,
	@Text nvarchar(max),
	@IPAddress nvarchar(100),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Post]
	(
		[TopicID],
		[UserID],
		[Text],
		[IPAddress],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@TopicID,
		@UserID,
		@Text,
		@IPAddress,
		@CreatedOn,
		@UpdatedOn
	)

	set @PostID=@@identity

	--update stats/info
	exec [dbo].[Nop_Forums_TopicUpdateCounts] @TopicID
	
	declare @ForumID int
	SELECT 
		@ForumID = ft.ForumID
	FROM
		[Nop_Forums_Topic] ft
	WHERE
		ft.TopicID = @TopicID 

	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
	
	exec [dbo].[Nop_CustomerUpdateCounts] @UserID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PostLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PostLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PostLoadByPrimaryKey]
(
	@PostID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Post]
	WHERE
		(PostID = @PostID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PostUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PostUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PostUpdate]
(
	@PostID int,
	@TopicID int,
	@UserID int,
	@Text nvarchar(max),
	@IPAddress nvarchar(100),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Post]
	SET
		[TopicID]=@TopicID,
		[UserID]=@UserID,
		[Text]=@Text,
		[IPAddress]=@IPAddress,
		[CreatedOn]=@CreatedOn,
		[UpdatedOn]=@UpdatedOn
	WHERE
		PostID = @PostID

	--update stats/info
	exec [dbo].[Nop_Forums_TopicUpdateCounts] @TopicID
	
	declare @ForumID int
	SELECT 
		@ForumID = ft.ForumID
	FROM
		[Nop_Forums_Topic] ft
	WHERE
		ft.TopicID = @TopicID 
		
	exec [dbo].[Nop_Forums_ForumUpdateCounts] @ForumID
	
	exec [dbo].[Nop_CustomerUpdateCounts] @UserID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PostLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PostLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PostLoadAll]
(
	@TopicID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		PostID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (PostID, CreatedOn)
	SELECT DISTINCT
		fp.PostID, fp.CreatedOn
	FROM Nop_Forums_Post fp with (NOLOCK)
	WHERE   (
				@TopicID IS NULL OR @TopicID=0
				OR (fp.TopicID=@TopicID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (fp.UserID=@UserID)
			)
		AND	(
				patindex(@Keywords, isnull(fp.Text, '')) > 0
			)
	ORDER BY fp.CreatedOn, fp.PostID

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fp.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Post fp on fp.PostID = [pi].PostID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionDelete]
(
	@SubscriptionID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_Subscription]
	WHERE
		SubscriptionID = @SubscriptionID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionInsert]
(
	@SubscriptionID int = NULL output,
	@SubscriptionGUID uniqueidentifier,
	@UserID int,
	@ForumID int,
	@TopicID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_Subscription]
	(
		[SubscriptionGUID],
		[UserID],
		[ForumID],
		[TopicID],
		[CreatedOn]
	)
	VALUES
	(
		@SubscriptionGUID,
		@UserID,
		@ForumID,
		@TopicID,
		@CreatedOn
	)

	set @SubscriptionID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByPrimaryKey]
(
	@SubscriptionID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Subscription]
	WHERE
		(SubscriptionID = @SubscriptionID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionLoadByGUID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByGUID]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadByGUID]
(
	@SubscriptionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_Subscription]
	WHERE
		(SubscriptionGUID = @SubscriptionGUID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionUpdate]
(
	@SubscriptionID int,
	@SubscriptionGUID uniqueidentifier,
	@UserID int,
	@ForumID int,
	@TopicID int,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_Subscription]
	SET
		[SubscriptionGUID]=@SubscriptionGUID,
		[UserID]=@UserID,
		[ForumID]=@ForumID,
		[TopicID]=@TopicID,
		[CreatedOn]=@CreatedOn
	WHERE
		SubscriptionID = @SubscriptionID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadAll]
(
	@UserID				int,
	@ForumID			int,
	@TopicID			int,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		SubscriptionID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (SubscriptionID, CreatedOn)
	SELECT DISTINCT
		fs.SubscriptionID, fs.CreatedOn
	FROM Nop_Forums_Subscription fs with (NOLOCK)
	WHERE   (
				@UserID IS NULL OR @UserID=0
				OR (fs.UserID=@UserID)
			)
		AND (
				@ForumID IS NULL OR @ForumID=0
				OR (fs.ForumID=@ForumID)
			)
		AND (
				@TopicID IS NULL OR @TopicID=0
				OR (fs.TopicID=@TopicID)
			)
	ORDER BY fs.CreatedOn desc, fs.SubscriptionID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fs.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Subscription fs on fs.SubscriptionID = [pi].SubscriptionID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageDelete]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageDelete]
(
	@PrivateMessageID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_PrivateMessage]
	WHERE
		PrivateMessageID = @PrivateMessageID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageInsert]
(
	@PrivateMessageID int = NULL output,
	@FromUserID int,
	@ToUserID int,
	@Subject nvarchar(450),
	@Text nvarchar(max),
	@IsRead bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_PrivateMessage]
	(
		[FromUserID],
		[ToUserID],
		[Subject],
		[Text],
		[IsRead],
		[CreatedOn]
	)
	VALUES
	(
		@FromUserID,
		@ToUserID,
		@Subject,
		@Text,
		@IsRead,
		@CreatedOn
	)

	set @PrivateMessageID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadByPrimaryKey]
(
	@PrivateMessageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_PrivateMessage]
	WHERE
		(PrivateMessageID = @PrivateMessageID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageUpdate]
(
	@PrivateMessageID int,
	@FromUserID int,
	@ToUserID int,
	@Subject nvarchar(450),
	@Text nvarchar(max),
	@IsRead bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_PrivateMessage]
	SET
		[FromUserID]=@FromUserID,
		[ToUserID]=@ToUserID,
		[Subject]=@Subject,
		[Text]=@Text,
		[IsRead]=@IsRead,
		[CreatedOn]=@CreatedOn
	WHERE
		PrivateMessageID = @PrivateMessageID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadAll]
(
	@FromUserID			int,
	@ToUserID			int,
	@IsRead				bit = null,	--0 not read only, 1 read only, null - load all messages
	@Keywords			nvarchar(MAX),
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		PrivateMessageID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (PrivateMessageID, CreatedOn)
	SELECT DISTINCT
		fpm.PrivateMessageID, fpm.CreatedOn
	FROM Nop_Forums_PrivateMessage fpm with (NOLOCK)
	WHERE   (
				@FromUserID IS NULL OR @FromUserID=0
				OR (fpm.FromUserID=@FromUserID)
			)
		AND (
				@ToUserID IS NULL OR @ToUserID=0
				OR (fpm.ToUserID=@ToUserID)
			)
		AND (
				@IsRead IS NULL OR fpm.IsRead=@IsRead
			)
		AND	(
				(patindex(@Keywords, isnull(fpm.Subject, '')) > 0)
				or (patindex(@Keywords, isnull(fpm.Text, '')) > 0)
			)
	ORDER BY fpm.CreatedOn desc, fpm.PrivateMessageID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fpm.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_PrivateMessage fpm on fpm.PrivateMessageID = [pi].PrivateMessageID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO



IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'ForumWelcomeMessage')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'ForumWelcomeMessage')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'ForumWelcomeMessage' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'Forums', N'Put your welcome message here.')
	END
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Announcement')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Announcement', N'Announcement')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Author')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Author', N'Author')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.By')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.By', N'By')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Cancel')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Cancel', N'Cancel')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.DeletePost')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.DeletePost', N'Delete Post')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.DeleteTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.DeleteTopic', N'Delete Topic')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.EditPost')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.EditPost', N'Edit Post')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.EditTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.EditTopic', N'Edit Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Forum')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Forum', N'Forum')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.ForumName')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.ForumName', N'Forum Name')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Forums')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Forums', N'Forums')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Home')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Home', N'Home')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Joined')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Joined', N'Joined')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.In')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.In', N'In')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.LatestPost')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.LatestPost', N'Latest Post')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Location')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Location', N'Location')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.NewPost')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.NewPost', N'New Post')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.NewTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.NewTopic', N'New Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.NoPosts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.NoPosts', N'No Posts')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Normal')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Normal', N'Normal')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.NotifyWhenSomeonePostsInThisTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.NotifyWhenSomeonePostsInThisTopic', N'Notify me via email when someone posts in this topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Options')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Options', N'Options')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Priority')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Priority', N'Priority')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Posted')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Posted', N'Posted')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Posts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Posts', N'Posts')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Replies')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Replies', N'Replies')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Reply')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Reply', N'Reply')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Subject')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Subject', N'Subject')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Submit')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Submit', N'Submit')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Topics')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Topics', N'Topics')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.TopicTitle')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.TopicTitle', N'Topic Title')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.TotalPosts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.TotalPosts', N'Total Posts')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.UnwatchForum')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.UnwatchForum', N'Unwatch Forum')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.UnwatchTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.UnwatchTopic', N'Unwatch Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Views')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Views', N'Views')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.WatchForum')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.WatchForum', N'Watch Forum')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.WatchTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.WatchTopic', N'Watch Topic')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsAdmin,
		IsGuest,
		IsForumModerator,
		TotalForumPosts,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsAdmin,
		@IsGuest,
		@IsForumModerator,
		@TotalForumPosts,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username
	)

	set @CustomerId=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100)
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		IsForumModerator=@IsForumModerator,
		TotalForumPosts=@TotalForumPosts,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username
	WHERE
		[CustomerId] = @CustomerId

END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.ForumsEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.ForumsEnabled', N'false', N'Determines whether the forums are enabled')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.TopicSubjectMaxLength')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.TopicSubjectMaxLength', N'0', N'Determines maximum length of topic subject')
END
GO



IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.PostMaxLength')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.PostMaxLength', N'0', N'Determines maximum length of post')
END
GO

-- search customer by usernames

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadAll]
GO

CREATE  PROCEDURE [dbo].[Nop_CustomerLoadAll]
(
	@StartTime	datetime = NULL,
	@EndTime	datetime = NULL,
	@Email		nvarchar(200),
	@Username	nvarchar(200),
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN

	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'


	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	DECLARE @TotalThreads int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		CustomerID int NOT NULL,
		RegistrationDate datetime NOT NULL,
	)

	INSERT INTO #PageIndex (CustomerID, RegistrationDate)
	SELECT DISTINCT
		c.CustomerID, c.RegistrationDate
	FROM [Nop_Customer] c with (NOLOCK)
	WHERE 
		(@StartTime is NULL or DATEDIFF(day, @StartTime, c.RegistrationDate) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, c.RegistrationDate) <= 0) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		deleted=0
	order by RegistrationDate desc 

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		c.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_Customer] c on c.CustomerID = [pi].CustomerID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
	
END
GO
-- end search customer by usernames


IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='AvatarID')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD AvatarID INT NOT NULL CONSTRAINT [DF_Nop_Customer_AvatarID] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsAdmin,
		IsGuest,
		IsForumModerator,
		TotalForumPosts,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username,
		AvatarID
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsAdmin,
		@IsGuest,
		@IsForumModerator,
		@TotalForumPosts,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username,
		@AvatarID
	)

	set @CustomerId=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		IsForumModerator=@IsForumModerator,
		TotalForumPosts=@TotalForumPosts,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username,
		AvatarID=@AvatarID
	WHERE
		[CustomerId] = @CustomerId

END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.Customer.AvatarSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.Customer.AvatarSize', N'85', N'The customer avatar image size')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.Customer.AvatarMaxSizeBytes')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.Customer.AvatarMaxSizeBytes', N'20000', N'The customer avatar maximum size in bytes')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.Avatar')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.Avatar', N'Avatar')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.UploadAvatar')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.UploadAvatar', N'Upload avatar')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.RemoveAvatar')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.RemoveAvatar', N'Remove avatar')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.UploadAvatarRules')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.UploadAvatarRules', N'Avatar must be in GIF or JPEG format with the maximum size of 20 KB')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.Customer.DefaultAvatarImageName')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.Customer.DefaultAvatarImageName', N'defaultAvatar.jpg', N'The default avatar image name. Must be stored in the /images directory')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.ProfileOf')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.ProfileOf', N'Profile: {0}')
END
GO



IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.PersonalInfo')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.PersonalInfo', N'Personal Info')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.FullName')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.FullName', N'Full Name')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.Country')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.Country', N'Country')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.Statistics')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.Statistics', N'Statistics')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.TotalPosts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.TotalPosts', N'Total Posts')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.JoinDate')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.JoinDate', N'Join Date')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.DateOfBirth')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.DateOfBirth', N'Date Of Birth')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.LatestPosts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.LatestPosts', N'Latest Posts')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.PostedOn')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.PostedOn', N'Posted On')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Profile.Topic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Profile.Topic', N'Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.TextCannotBeEmpty')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.TextCannotBeEmpty', N'Text cannot be empty')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.TopicSubjectCannotBeEmpty')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.TopicSubjectCannotBeEmpty', N'Topic subject cannot be empty')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.TopicsPageSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.TopicsPageSize', N'10', N'Determines the page size for topics in forums')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.PostsPageSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.PostsPageSize', N'10', N'Determines the page size for posts in topics')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.CustomersAllowedToEditPosts')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.CustomersAllowedToEditPosts', N'false', N'Determines whether customers are allowed to edit posts that they created')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.CustomersAllowedToDeletePosts')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.CustomersAllowedToDeletePosts', N'false', N'Determines whether customers are allowed to delete posts that they created')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
(
	@ForumID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),	
	@SearchPosts		bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		TopicID int NOT NULL,
		TopicTypeID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (TopicID, TopicTypeID, CreatedOn)
	SELECT DISTINCT
		ft.TopicID, ft.TopicTypeID, ft.CreatedOn
	FROM Nop_Forums_Topic ft with (NOLOCK) 
	LEFT OUTER JOIN Nop_Forums_Post fp with (NOLOCK) ON ft.TopicID = fp.TopicID
	WHERE  (
				@ForumID IS NULL OR @ForumID=0
				OR (ft.ForumID=@ForumID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (ft.UserID=@UserID)
			)
		AND	(
				(patindex(@Keywords, isnull(ft.Subject, '')) > 0)
				or (@SearchPosts = 1 and patindex(@Keywords, isnull(fp.Text, '')) > 0)
			)		
	ORDER BY ft.TopicTypeID desc, ft.CreatedOn desc, ft.TopicID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		ft.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Topic ft on ft.TopicID = [pi].TopicID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.SearchResultsPageSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.SearchResultsPageSize', N'10', N'Determines the page size for search results in forums')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.SearchTermCouldNotBeEmpty')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.SearchTermCouldNotBeEmpty', N'Search term could not be empty')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.SearchTermMinimumLengthIs3Characters')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.SearchTermMinimumLengthIs3Characters', N'Search term minimum length is 3 characters')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.SearchButton')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.SearchButton', N'Search')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.SearchNoResultsText')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.SearchNoResultsText', N'No posts were found that matched your criteria.')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Search')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Search', N'Search Forums')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Moderator')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Moderator', N'Moderator')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.Status')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.Status', N'Status')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'News.NewsEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'News.NewsEnabled', N'true', N'Determines whether news are enabled')
END
GO

--new message templates
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'Forums.NewForumTopic')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'Forums.NewForumTopic')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = 'Forums.NewForumTopic' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'%Store.Name%. New Topic Notification.', N'<p><a href="%Store.URL%">%Store.Name%</a> <br />
		<br />
		A new topic <a href="%Forums.TopicURL%">"%Forums.TopicName%"</a> has been created at <a href="%Forums.ForumURL%">"%Forums.ForumName%"</a> forum.
		<br />
		<br />
		Click <a href="%Forums.TopicURL%">here</a> for more info.</p>')
	END
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'Forums.NewForumPost')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'Forums.NewForumPost')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = 'Forums.NewForumPost' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'%Store.Name%. New Post Notification.', N'<p><a href="%Store.URL%">%Store.Name%</a> <br />
		<br />
		A new post has been created in the topic <a href="%Forums.TopicURL%">"%Forums.TopicName%"</a> at <a href="%Forums.ForumURL%">"%Forums.ForumName%"</a> forum.
		<br />
		<br />
		Click <a href="%Forums.TopicURL%">here</a> for more info.</p>')
	END
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Content.AllowedUserHtmlTags')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Content.AllowedUserHtmlTags', N'br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address', N'Allowed HTML tags entered by customers')
END
GO



--Bug: Related Products are still showing after they were deleted.
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RelatedProductLoadByProductID1]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RelatedProductLoadByProductID1]
GO
CREATE PROCEDURE [dbo].[Nop_RelatedProductLoadByProductID1]
(
	@ProductID1 int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		rp.*
	FROM Nop_RelatedProduct rp
	INNER JOIN Nop_Product p ON rp.ProductID2=p.ProductID
	WHERE rp.ProductID1=@ProductID1
		AND (p.Published = 1 or @ShowHidden = 1) and p.Deleted=0
	ORDER BY rp.DisplayOrder
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.LatestUserPostsPageSize')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.LatestUserPostsPageSize', N'5', N'Determines the page size for latest user post')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.ShowCustomersPostCount')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.ShowCustomersPostCount', N'true', N'Determines whether to show customers post count')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.CustomerNameFormatting')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.CustomerNameFormatting', N'1', N'Determines whether the customer name formatting')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.CustomersAllowedToUploadAvatars')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.CustomersAllowedToUploadAvatars', N'false', N'Determines whether customers are allowed to upload avatars')
END
GO



IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.DefaultAvatarEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.DefaultAvatarEnabled', N'false', N'Determines whether to display default user avatar')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.ShowCustomersLocation')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.ShowCustomersLocation', N'false', N'Determines whether customers location is shown')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.ShowCustomersJoinDate')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.ShowCustomersJoinDate', N'true', N'Determines whether to show customers join date')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.AllowViewingProfiles')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.AllowViewingProfiles', N'false', N'Determines whether customers are allowed to view profiles of other customers.')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
(
	@ForumID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),	
	@SearchPosts		bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		TopicID int NOT NULL,
		TopicTypeID int NOT NULL,
		LastPostTime datetime NULL,
	)

	INSERT INTO #PageIndex (TopicID, TopicTypeID, LastPostTime)
	SELECT DISTINCT
		ft.TopicID, ft.TopicTypeID, ft.LastPostTime
	FROM Nop_Forums_Topic ft with (NOLOCK) 
	LEFT OUTER JOIN Nop_Forums_Post fp with (NOLOCK) ON ft.TopicID = fp.TopicID
	WHERE  (
				@ForumID IS NULL OR @ForumID=0
				OR (ft.ForumID=@ForumID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (ft.UserID=@UserID)
			)
		AND	(
				(patindex(@Keywords, isnull(ft.Subject, '')) > 0)
				or (@SearchPosts = 1 and patindex(@Keywords, isnull(fp.Text, '')) > 0)
			)		
	ORDER BY ft.TopicTypeID desc, ft.LastPostTime desc, ft.TopicID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		ft.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Topic ft on ft.TopicID = [pi].TopicID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO







-- download support for other file types
IF NOT EXISTS (
	SELECT 1 FROM syscolumns 
	WHERE id=object_id('[dbo].[Nop_Download]') 
	AND NAME='ContentType')
BEGIN
	ALTER TABLE [dbo].[Nop_Download]
	ADD ContentType NVARCHAR(20) NOT NULL 
		CONSTRAINT [DF_Nop_Download_ContentType] DEFAULT (N'')
		
	exec('UPDATE [dbo].[Nop_Download] SET [ContentType] = N''application/zip''')
	
	exec('UPDATE [dbo].[Nop_Download] SET [Extension] = N''.zip''')
	
	
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadInsert]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadInsert]
(
	@DownloadID int = NULL output,
	@DownloadBinary image,	
	@ContentType nvarchar(20),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Download]
	(
		DownloadBinary,
		ContentType,
		Extension,
		IsNew
	)
	VALUES
	(
		@DownloadBinary,
		@ContentType,
		@Extension,
		@IsNew
	)

	set @DownloadID=@@identity
END
GO
	
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadUpdate]
(
	@DownloadID int,
	@DownloadBinary image,
	@ContentType nvarchar(20),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN

	UPDATE [Nop_Download]
	SET
		DownloadBinary=@DownloadBinary,
		ContentType = @ContentType,
		Extension=@Extension,
		IsNew=@IsNew
	WHERE
		DownloadID = @DownloadID

END
GO

-- end download changes



-- enhanced topics feature

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]
(
	@TopicID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_TopicLocalized]
	WHERE (TopicID = @TopicID) AND (LanguageID = @LanguageID)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.Topic.UrlRewriteFormat')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.Topic.UrlRewriteFormat', N'{0}Topic/{1}-{2}.aspx', N'The Url rewrite format for topic pages')
END
GO

-- end enhanced topics feature



--database optimization
IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Address_CustomerID' 
	AND id=object_id(N'[dbo].[Nop_Address]'))
CREATE INDEX [IX_Nop_Address_CustomerID] 
ON [dbo].[Nop_Address]([CustomerID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_BlogComment_BlogPostID' 
	AND id=object_id(N'[dbo].[Nop_BlogComment]'))
CREATE INDEX [IX_Nop_BlogComment_BlogPostID] 
ON [dbo].[Nop_BlogComment]([BlogPostID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_BlogPost_LanguageID' 
	AND id=object_id(N'[dbo].[Nop_BlogPost]'))
CREATE INDEX [IX_Nop_BlogPost_LanguageID] 
ON [dbo].[Nop_BlogPost]([LanguageID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Category_ParentCategoryID' 
	AND id=object_id(N'[dbo].[Nop_Category]'))
CREATE INDEX [IX_Nop_Category_ParentCategoryID] 
ON [dbo].[Nop_Category]([ParentCategoryID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Category_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Category]'))
CREATE INDEX [IX_Nop_Category_DisplayOrder] 
ON [dbo].[Nop_Category]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Country_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Country]'))
CREATE INDEX [IX_Nop_Country_DisplayOrder] 
ON [dbo].[Nop_Country]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Currency_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Currency]'))
CREATE INDEX [IX_Nop_Currency_DisplayOrder] 
ON [dbo].[Nop_Currency]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Currency_CurrencyCode' 
	AND id=object_id(N'[dbo].[Nop_Currency]'))
CREATE INDEX [IX_Nop_Currency_CurrencyCode] 
ON [dbo].[Nop_Currency]([CurrencyCode])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Customer_Email' 
	AND id=object_id(N'[dbo].[Nop_Customer]'))
CREATE INDEX [IX_Nop_Customer_Email] 
ON [dbo].[Nop_Customer]([Email])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Customer_AffiliateID' 
	AND id=object_id(N'[dbo].[Nop_Customer]'))
CREATE INDEX [IX_Nop_Customer_AffiliateID] 
ON [dbo].[Nop_Customer]([AffiliateID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Customer_Username' 
	AND id=object_id(N'[dbo].[Nop_Customer]'))
CREATE INDEX [IX_Nop_Customer_Username] 
ON [dbo].[Nop_Customer]([Username])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_CustomerAttribute_CustomerId' 
	AND id=object_id(N'[dbo].[Nop_CustomerAttribute]'))
CREATE INDEX [IX_Nop_CustomerAttribute_CustomerId] 
ON [dbo].[Nop_CustomerAttribute]([CustomerId])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_CustomerSession_CustomerID' 
	AND id=object_id(N'[dbo].[Nop_CustomerSession]'))
CREATE INDEX [IX_Nop_CustomerSession_CustomerID] 
ON [dbo].[Nop_CustomerSession]([CustomerID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Forum_ForumGroupID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Forum]'))
CREATE INDEX [IX_Nop_Forums_Forum_ForumGroupID] 
ON [dbo].[Nop_Forums_Forum]([ForumGroupID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Forum_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Forums_Forum]'))
CREATE INDEX [IX_Nop_Forums_Forum_DisplayOrder] 
ON [dbo].[Nop_Forums_Forum]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Group_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Forums_Group]'))
CREATE INDEX [IX_Nop_Forums_Group_DisplayOrder] 
ON [dbo].[Nop_Forums_Group]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Post_TopicID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Post]'))
CREATE INDEX [IX_Nop_Forums_Post_TopicID] 
ON [dbo].[Nop_Forums_Post]([TopicID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Post_UserID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Post]'))
CREATE INDEX [IX_Nop_Forums_Post_UserID] 
ON [dbo].[Nop_Forums_Post]([UserID])
GO
IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Subscription_ForumID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Subscription]'))
CREATE INDEX [IX_Nop_Forums_Subscription_ForumID] 
ON [dbo].[Nop_Forums_Subscription]([ForumID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Subscription_TopicID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Subscription]'))
CREATE INDEX [IX_Nop_Forums_Subscription_TopicID] 
ON [dbo].[Nop_Forums_Subscription]([TopicID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Forums_Topic_ForumID' 
	AND id=object_id(N'[dbo].[Nop_Forums_Topic]'))
CREATE INDEX [IX_Nop_Forums_Topic_ForumID] 
ON [dbo].[Nop_Forums_Topic]([ForumID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Language_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Language]'))
CREATE INDEX [IX_Nop_Language_DisplayOrder] 
ON [dbo].[Nop_Language]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_LocaleStringResource_LanguageID' 
	AND id=object_id(N'[dbo].[Nop_LocaleStringResource]'))
CREATE INDEX [IX_Nop_LocaleStringResource_LanguageID] 
ON [dbo].[Nop_LocaleStringResource]([LanguageID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Log_CreatedOn' 
	AND id=object_id(N'[dbo].[Nop_Log]'))
CREATE INDEX [IX_Nop_Log_CreatedOn] 
ON [dbo].[Nop_Log]([CreatedOn])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Manufacturer_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_Manufacturer]'))
CREATE INDEX [IX_Nop_Manufacturer_DisplayOrder] 
ON [dbo].[Nop_Manufacturer]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_MessageTemplate_Name' 
	AND id=object_id(N'[dbo].[Nop_MessageTemplate]'))
CREATE INDEX [IX_Nop_MessageTemplate_Name] 
ON [dbo].[Nop_MessageTemplate]([Name])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_MessageTemplateLocalized_LanguageID' 
	AND id=object_id(N'[dbo].[Nop_MessageTemplateLocalized]'))
CREATE INDEX [IX_Nop_MessageTemplateLocalized_LanguageID] 
ON [dbo].[Nop_MessageTemplateLocalized]([LanguageID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_News_LanguageID' 
	AND id=object_id(N'[dbo].[Nop_News]'))
CREATE INDEX [IX_Nop_News_LanguageID] 
ON [dbo].[Nop_News]([LanguageID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_NewsComment_NewsID' 
	AND id=object_id(N'[dbo].[Nop_NewsComment]'))
CREATE INDEX [IX_Nop_NewsComment_NewsID] 
ON [dbo].[Nop_NewsComment]([NewsID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Order_CustomerID' 
	AND id=object_id(N'[dbo].[Nop_Order]'))
CREATE INDEX [IX_Nop_Order_CustomerID] 
ON [dbo].[Nop_Order]([CustomerID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Order_AffiliateID' 
	AND id=object_id(N'[dbo].[Nop_Order]'))
CREATE INDEX [IX_Nop_Order_AffiliateID] 
ON [dbo].[Nop_Order]([AffiliateID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Order_CreatedOn' 
	AND id=object_id(N'[dbo].[Nop_Order]'))
CREATE INDEX [IX_Nop_Order_CreatedOn] 
ON [dbo].[Nop_Order]([CreatedOn])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_OrderNote_OrderID' 
	AND id=object_id(N'[dbo].[Nop_OrderNote]'))
CREATE INDEX [IX_Nop_OrderNote_OrderID] 
ON [dbo].[Nop_OrderNote]([OrderID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_OrderProductVariant_OrderID' 
	AND id=object_id(N'[dbo].[Nop_OrderProductVariant]'))
CREATE INDEX [IX_Nop_OrderProductVariant_OrderID] 
ON [dbo].[Nop_OrderProductVariant]([OrderID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_PollAnswer_PollID' 
	AND id=object_id(N'[dbo].[Nop_PollAnswer]'))
CREATE INDEX [IX_Nop_PollAnswer_PollID] 
ON [dbo].[Nop_PollAnswer]([PollID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Product_SpecificationAttribute_Mapping_ProductID' 
	AND id=object_id(N'[dbo].[Nop_Product_SpecificationAttribute_Mapping]'))
CREATE INDEX [IX_Nop_Product_SpecificationAttribute_Mapping_ProductID] 
ON [dbo].[Nop_Product_SpecificationAttribute_Mapping]([ProductID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ProductReview_ProductID' 
	AND id=object_id(N'[dbo].[Nop_ProductReview]'))
CREATE INDEX [IX_Nop_ProductReview_ProductID] 
ON [dbo].[Nop_ProductReview]([ProductID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ProductVariant_ProductID' 
	AND id=object_id(N'[dbo].[Nop_ProductVariant]'))
CREATE INDEX [IX_Nop_ProductVariant_ProductID] 
ON [dbo].[Nop_ProductVariant]([ProductID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ProductVariant_DisplayOrder' 
	AND id=object_id(N'[dbo].[Nop_ProductVariant]'))
CREATE INDEX [IX_Nop_ProductVariant_DisplayOrder] 
ON [dbo].[Nop_ProductVariant]([DisplayOrder])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ProductVariantAttributeValue_ProductVariantAttributeID' 
	AND id=object_id(N'[dbo].[Nop_ProductVariantAttributeValue]'))
CREATE INDEX [IX_Nop_ProductVariantAttributeValue_ProductVariantAttributeID] 
ON [dbo].[Nop_ProductVariantAttributeValue]([ProductVariantAttributeID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ProductVariant_ProductAttribute_Mapping_ProductVariantID' 
	AND id=object_id(N'[dbo].[Nop_ProductVariant_ProductAttribute_Mapping]'))
CREATE INDEX [IX_Nop_ProductVariant_ProductAttribute_Mapping_ProductVariantID] 
ON [dbo].[Nop_ProductVariant_ProductAttribute_Mapping]([ProductVariantID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_QueuedEmail_CreatedOn' 
	AND id=object_id(N'[dbo].[Nop_QueuedEmail]'))
CREATE INDEX [IX_Nop_QueuedEmail_CreatedOn] 
ON [dbo].[Nop_QueuedEmail]([CreatedOn])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_RelatedProduct_ProductID1' 
	AND id=object_id(N'[dbo].[Nop_RelatedProduct]'))
CREATE INDEX [IX_Nop_RelatedProduct_ProductID1] 
ON [dbo].[Nop_RelatedProduct]([ProductID1])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_ShoppingCartItem_ShoppingCartTypeID_CustomerSessionGUID' 
	AND id=object_id(N'[dbo].[Nop_ShoppingCartItem]'))
CREATE INDEX [IX_Nop_ShoppingCartItem_ShoppingCartTypeID_CustomerSessionGUID] 
ON [dbo].[Nop_ShoppingCartItem]([ShoppingCartTypeID], [CustomerSessionGUID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_StateProvince_CountryID' 
	AND id=object_id(N'[dbo].[Nop_StateProvince]'))
CREATE INDEX [IX_Nop_StateProvince_CountryID] 
ON [dbo].[Nop_StateProvince]([CountryID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_TierPrice_ProductVariantID' 
	AND id=object_id(N'[dbo].[Nop_TierPrice]'))
CREATE INDEX [IX_Nop_TierPrice_ProductVariantID] 
ON [dbo].[Nop_TierPrice]([ProductVariantID])
GO

IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_Topic_Name' 
	AND id=object_id(N'[dbo].[Nop_Topic]'))
CREATE INDEX [IX_Nop_Topic_Name] 
ON [dbo].[Nop_Topic]([Name])
GO


IF NOT EXISTS (SELECT 1 FROM dbo.sysindexes WHERE [NAME]=N'IX_Nop_TopicLocalized_LanguageID' 
	AND id=object_id(N'[dbo].[Nop_TopicLocalized]'))
CREATE INDEX [IX_Nop_TopicLocalized_LanguageID] 
ON [dbo].[Nop_TopicLocalized]([LanguageID])
GO





--customer ratings
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Product]') and NAME='AllowCustomerRatings')
BEGIN
	ALTER TABLE [dbo].[Nop_Product] 
	ADD AllowCustomerRatings bit NOT NULL CONSTRAINT [DF_Nop_Product_AllowCustomerRatings] DEFAULT ((1))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductInsert]
(
	@ProductID int = NULL output,
	@Name nvarchar(400),
	@ShortDescription ntext,
	@FullDescription ntext,
	@AdminComment ntext,
	@ProductTypeID int,
	@TemplateID int,
	@ShowOnHomePage bit,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@AllowCustomerReviews bit,
	@AllowCustomerRatings bit,
	@RatingSum int,
	@TotalRatingVotes int,
	@Published bit,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Product]
	(
		[Name],
		ShortDescription,
		FullDescription,
		AdminComment,
		ProductTypeID,
		TemplateID,
		ShowOnHomePage,
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName,
		AllowCustomerReviews,
		AllowCustomerRatings,
		RatingSum,
		TotalRatingVotes,
		Published,
		Deleted,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@ShortDescription,
		@FullDescription,
		@AdminComment,
		@ProductTypeID,
		@TemplateID,
		@ShowOnHomePage,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName,
		@AllowCustomerReviews,
		@AllowCustomerRatings,
		@RatingSum,
		@TotalRatingVotes,
		@Published,
		@Deleted,
		@CreatedOn,
		@UpdatedOn
	)

	set @ProductID=@@identity
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductUpdate]
(
	@ProductID int,
	@Name nvarchar(400),
	@ShortDescription ntext,
	@FullDescription ntext,
	@AdminComment ntext,
	@ProductTypeID int,
	@TemplateID int,
	@ShowOnHomePage bit,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@AllowCustomerReviews bit,
	@AllowCustomerRatings bit,
	@RatingSum int,
	@TotalRatingVotes int,
	@Published bit,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Product]
	SET
		[Name]=@Name,
		ShortDescription=@ShortDescription,
		FullDescription=@FullDescription,
		AdminComment=@AdminComment,
		ProductTypeID=@ProductTypeID,
		TemplateID=@TemplateID,
		ShowOnHomePage=@ShowOnHomePage,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		AllowCustomerReviews=@AllowCustomerReviews,
		AllowCustomerRatings=@AllowCustomerRatings,
		RatingSum=@RatingSum,
		TotalRatingVotes=@TotalRatingVotes,
		Published=@Published,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		[ProductID] = @ProductID
END
GO








--cache settings

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CategoryManager.CategoriesCacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CategoryManager.CategoriesCacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CategoryManager.MappingsCacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CategoryManager.MappingsCacheEnabled', N'false', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.BlogManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.BlogManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ForumManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ForumManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.NewsManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.NewsManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.PollManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.PollManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CustomerManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CustomerManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CountryManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CountryManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CurrencyManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CurrencyManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.LanguageManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.LanguageManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.StateProvinceManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.StateProvinceManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.LocaleStringResourceManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.LocaleStringResourceManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ManufacturerManager.ManufacturersCacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ManufacturerManager.ManufacturersCacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ManufacturerManager.MappingsCacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ManufacturerManager.MappingsCacheEnabled', N'false', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.MeasureManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.MeasureManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.OrderManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.OrderManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.CreditCardTypeManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.CreditCardTypeManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.PaymentMethodManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.PaymentMethodManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.PaymentStatusManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.PaymentStatusManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ProductAttributeManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ProductAttributeManager.CacheEnabled', N'false', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.SpecificationAttributeManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.SpecificationAttributeManager.CacheEnabled', N'false', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ProductManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ProductManager.CacheEnabled', N'false', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.DiscountManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.DiscountManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ShippingMethodManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ShippingMethodManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ShippingRateComputationMethodManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ShippingRateComputationMethodManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.ShippingStatusManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.ShippingStatusManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.TaxCategoryManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.TaxCategoryManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.TaxProviderManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.TaxProviderManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.TaxRateManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.TaxRateManager.CacheEnabled', N'true', N'')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.TemplateManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.TemplateManager.CacheEnabled', N'true', N'')
END
GO
--end cache settings


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadByEmail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadByEmail]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerLoadByEmail]
(
	@Email nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([Email] = @Email)
	ORDER BY CustomerID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadByUsername]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadByUsername]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerLoadByUsername]
(
	@Username nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([Username] = @Username)
	ORDER BY CustomerID
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.EditorType')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.EditorType', N'20', N'Determines a forum editor type.')
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SalesBestSellersReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SalesBestSellersReport]
GO
CREATE PROCEDURE [dbo].[Nop_SalesBestSellersReport]
(
	@LastDays int = 360,
	@RecordsToReturn int = 10,
	@OrderBy int = 1
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @cmd varchar(500)
	
	CREATE TABLE #tmp (
		ID int not null identity,
		ProductVariantID int,
		SalesTotalCount int,
		SalesTotalAmount money)
	INSERT #tmp (
		ProductVariantID,
		SalesTotalCount,
		SalesTotalAmount)
	SELECT 
		s.ProductVariantID,
		s.SalesTotalCount, 
		s.SalesTotalAmount 
	FROM (SELECT opv.ProductVariantID, SUM(opv.Quantity) AS SalesTotalCount, SUM(opv.PriceExclTax) AS SalesTotalAmount
		  FROM [Nop_OrderProductVariant] opv
				INNER JOIN [Nop_Order] o on opv.OrderID = o.OrderID 
				WHERE o.CreatedOn >= dateadd(dy, -@LastDays, getdate())
				AND o.Deleted=0
		  GROUP BY opv.ProductVariantID 
		 ) s
		INNER JOIN [Nop_ProductVariant] pv with (nolock) on s.ProductVariantID = pv.ProductVariantID
		INNER JOIN [Nop_Product] p with (nolock) on pv.ProductID = p.ProductID
	WHERE p.Deleted = 0 
		AND p.Published = 1  
		AND pv.Published = 1 
		AND pv.Deleted = 0
	ORDER BY case @OrderBy when 1 then s.SalesTotalCount when 2 then s.SalesTotalAmount else s.SalesTotalCount end desc

	SET @cmd = 'SELECT TOP ' + convert(varchar(10), @RecordsToReturn ) + ' * FROM #tmp Order By ID'

	EXEC (@cmd)
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderAverageReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderAverageReport]
GO
CREATE PROCEDURE [dbo].[Nop_OrderAverageReport]
(
	@OrderStatusID int
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT 
		SUM(CASE WHEN datediff(dy, CreatedOn, getutcdate()) = 0 THEN OrderTotal ELSE 0 END) SumTodayOrders,
		SUM(CASE WHEN datediff(dy, CreatedOn, getutcdate()) = 0 THEN 1 ELSE 0 END) CountTodayOrders,
		SUM(CASE WHEN datediff(wk, CreatedOn, getutcdate()) = 0 THEN OrderTotal ELSE 0 END) SumThisWeekOrders,
		SUM(CASE WHEN datediff(wk, CreatedOn, getutcdate()) = 0 THEN 1 ELSE 0 END) CountThisWeekOrders,
		SUM(CASE WHEN datediff(mm, CreatedOn, getutcdate()) = 0 THEN OrderTotal ELSE 0 END) SumThisMonthOrders,
		SUM(CASE WHEN datediff(mm, CreatedOn, getutcdate()) = 0 THEN 1 ELSE 0 END) CountThisMonthOrders,
		SUM(CASE WHEN datediff(yy, CreatedOn, getutcdate()) = 0 THEN OrderTotal ELSE 0 END) SumThisYearOrders,
		SUM(CASE WHEN datediff(yy, CreatedOn, getutcdate()) = 0 THEN 1 ELSE 0 END) CountThisYearOrders,
		SUM(OrderTotal) SumAllTimeOrders,
		COUNT(1) CountAllTimeOrders
	FROM [Nop_Order]
	WHERE OrderTotal > 0 AND OrderStatusID=@OrderStatusID AND Deleted=0
	
END
GO