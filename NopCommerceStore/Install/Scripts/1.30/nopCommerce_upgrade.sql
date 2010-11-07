--upgrade scripts from nopCommerce 1.20 to 1.30






-- stripping topic subject (forums)
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.StrippedTopicMaxLength')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.StrippedTopicMaxLength', N'45', N'')
END
GO






-- fixed pricelist stored procedure
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
	FROM [Nop_Pricelist]
	WHERE
		PricelistID = @PricelistID
END
GO

-- make configurable whether anonymous customers can leave blog/news comments
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Blog.AllowNotRegisteredUsersToLeaveComments')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Blog.AllowNotRegisteredUsersToLeaveComments', N'true', N'Determines whether not registered user can leave blog comments')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'News.AllowNotRegisteredUsersToLeaveComments')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'News.AllowNotRegisteredUsersToLeaveComments', N'true', N'Determines whether not registered user can leave news comments')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Blog.OnlyRegisteredUsersCanLeaveComments')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Blog.OnlyRegisteredUsersCanLeaveComments', N'Only registered users can leave comments.')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'News.OnlyRegisteredUsersCanLeaveComments')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'News.OnlyRegisteredUsersCanLeaveComments', N'Only registered users can leave comments.')
END
GO


-- deleting expired sessions

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerSessionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerSessionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerSessionLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CustomerSession]
	order by LastAccessed desc
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerSessionDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerSessionDelete]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerSessionDelete]
(
	@CustomerSessionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerSession]
	WHERE
		CustomerSessionGUID = @CustomerSessionGUID
END
GO

--added createdon, updatedon properties for localized topics

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_TopicLocalized]') and NAME='CreatedOn')
BEGIN
	ALTER TABLE [dbo].[Nop_TopicLocalized] 
	ADD CreatedOn DATETIME NOT NULL CONSTRAINT [DF_Nop_TopicLocalized_CreatedOn] DEFAULT ((getutcdate()))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_TopicLocalized]') and NAME='UpdatedOn')
BEGIN
	ALTER TABLE [dbo].[Nop_TopicLocalized] 
	ADD UpdatedOn DATETIME NOT NULL CONSTRAINT [DF_Nop_TopicLocalized_UpdatedOn] DEFAULT ((getutcdate()))
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
	@Body nvarchar(MAX),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_TopicLocalized]
	(
		TopicID,
		LanguageID,
		[Title],
		Body,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@TopicID,
		@LanguageID,
		@Title,
		@Body,
		@CreatedOn,
		@UpdatedOn
	)

	set @TopicLocalizedID=@@identity
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
	@Body nvarchar(MAX),
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_TopicLocalized]
	SET
		TopicID=@TopicID,
		LanguageID=@LanguageID,
		[Title]=@Title,
		Body=@Body,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		TopicLocalizedID = @TopicLocalizedID
END
GO


-- Recently viewed products
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.RecentlyViewedProductsEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.RecentlyViewedProductsEnabled', N'true', N'Determines whether "Recently viewed products" feature is enabled')
END
GO
-- Recently added products
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.RecentlyAddedProductsEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.RecentlyAddedProductsEnabled', N'true', N'Determines whether "Recently added products" feature is enabled')
END
GO


--new resource strings for user input validation of product reviews, news comments, blog comments
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.PleaseEnterReviewTitle')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.PleaseEnterReviewTitle', N'Please enter review title.')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Products.PleaseEnterReviewText')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Products.PleaseEnterReviewText', N'Please enter review text.')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'News.PleaseEnterCommentText')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'News.PleaseEnterCommentText', N'Please enter comment text.')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Blog.PleaseEnterCommentText')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Blog.PleaseEnterCommentText', N'Please enter comment text.')
END
GO



-- Extended sitemaps
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.Sitemaps.IncludeTopics')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.Sitemaps.IncludeTopics', N'false', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.Sitemaps.OtherPages')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.Sitemaps.OtherPages', N'', N'Comma separated page list')
END
GO

-- Don't allow registration of new customers
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Customer.NewCustomerRegistrationDisabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Customer.NewCustomerRegistrationDisabled', N'false', N'Determines whether new customer registration is not allowed')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'RegistrationNotAllowed')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'RegistrationNotAllowed')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'RegistrationNotAllowed' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'', N'Registration not allowed. You can edit this in the admin area.')
	END
END
GO

--password recovery locale resource
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.PasswordRecovery.EmailNotFound')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.PasswordRecovery.EmailNotFound', N'Email Not Found.')
END
GO

--Fix user latest posts (sort order) on profile page
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
	@AscSort			bit = 1,
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
	)

	INSERT INTO #PageIndex (PostID)
	SELECT
		fp.PostID
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
	ORDER BY
		CASE @AscSort WHEN 0 THEN fp.CreatedOn END DESC,
		CASE @AscSort WHEN 1 THEN fp.CreatedOn END,
		fp.PostID


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



--localization of page titles

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.Default')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.Default', N'Forums')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.PostEdit')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.PostEdit', N'Edit Post')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.PostNew')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.PostNew', N'New Post')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.Search')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.Search', N'Search Forums')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.TopicEdit')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.TopicEdit', N'Edit Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.TopicNew')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.TopicNew', N'New Topic')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.AboutUs')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.AboutUs', N'About Us')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Account')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Account', N'Account')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.AccountActivation')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.AccountActivation', N'Account Activation')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.AddressEdit')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.AddressEdit', N'Edit Address')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Blog')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Blog', N'Blog')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Checkout')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Checkout', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutBillingAddress')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutBillingAddress', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutCompleted')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutCompleted', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutConfirm')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutConfirm', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutPaymentInfo')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutPaymentInfo', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutPaymentMethod')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutPaymentMethod', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutShippingAddress')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutShippingAddress', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CheckoutShippingMethod')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CheckoutShippingMethod', N'Checkout')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.CompareProducts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.CompareProducts', N'Compare Products')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ConditionsInfo')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ConditionsInfo', N'Conditions')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ContactUs')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ContactUs', N'Contact Us')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Login')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Login', N'Login')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.OrderDetails')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.OrderDetails', N'Order Details')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.PasswordRecovery')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.PasswordRecovery', N'Password Recovery')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.PrivacyInfo')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.PrivacyInfo', N'Privacy Info')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ProductEmailAFriend')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ProductEmailAFriend', N'Email A Friend')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ProductReviewAdd')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ProductReviewAdd', N'Product Review')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Profile')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Profile', N'Profile')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.RecentlyAddedProducts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.RecentlyAddedProducts', N'Recently Added Products')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.RecentlyViewedProducts')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.RecentlyViewedProducts', N'Recently Viewed Products')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Register')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Register', N'Register')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Search')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Search', N'Search')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ShippingInfo')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ShippingInfo', N'Shipping Info')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.ShoppingCart')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.ShoppingCart', N'Shopping Cart')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Wishlist')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Wishlist', N'Wishlist')
END
GO



--advanced message queue search
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
(	
	@FromEmail nvarchar(255) = NULL,
	@ToEmail nvarchar(255) = NULL,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@QueuedEmailCount int,
	@LoadNotSentItemsOnly bit,
	@MaxSendTries int
)
AS
BEGIN
	IF (@QueuedEmailCount > 0)
	SET ROWCOUNT @QueuedEmailCount

	SELECT qu.*
	FROM [Nop_QueuedEmail] qu
	WHERE 
		(@FromEmail IS NULL or LEN(@FromEmail)=0 or (qu.[From] like '%' + COALESCE(@FromEmail,qu.[From]) + '%')) AND
		(@ToEmail IS NULL or LEN(@ToEmail)=0 or (qu.[To] like '%' + COALESCE(@ToEmail,qu.[To]) + '%')) AND
		(@StartTime is NULL or DATEDIFF(day, @StartTime, qu.CreatedOn) >= 0) AND
		(@EndTime is NULL or DATEDIFF(day, @EndTime, qu.CreatedOn) <= 0) AND 
		((@LoadNotSentItemsOnly IS NULL OR @LoadNotSentItemsOnly=0) OR (qu.SentOn IS NULL)) AND
		(qu.SendTries < @MaxSendTries)
	ORDER BY qu.Priority desc, qu.CreatedOn ASC
	
	SET ROWCOUNT 0
END
GO



--Checkout as Guest or Register question
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Checkout.CheckoutAsGuestOrRegister')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Checkout.CheckoutAsGuestOrRegister', N'Checkout as a guest or register')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Checkout.CheckoutAsGuest')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Checkout.CheckoutAsGuest', N'Checkout as Guest')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Topic]
		WHERE [Name] = N'CheckoutAsGuestOrRegister')
BEGIN
	INSERT [dbo].[Nop_Topic] ([Name])
	VALUES (N'CheckoutAsGuestOrRegister')

	DECLARE @TopicID INT 
	SELECT @TopicID = t.TopicID FROM Nop_Topic t
							WHERE t.Name = 'CheckoutAsGuestOrRegister' 

	IF (@TopicID > 0)
	BEGIN
		INSERT [dbo].[Nop_TopicLocalized] ([TopicID], [LanguageID], [Title], [Body]) 
		VALUES (@TopicID, 7, N'', N' <strong>Register and save time!</strong><br /> 
            Register with us for future convenience:
			<ul> 
                <li>Fast and easy check out</li> 
                <li>Easy access to your order history and status</li> 
            </ul> ')
	END
END
GO




--IP blacklist

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_BannedIpAddress]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_BannedIpAddress](
	[BannedIpAddressID] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	CONSTRAINT [PK_Nop_BannedIpAddress] PRIMARY KEY CLUSTERED 
	(
		[BannedIpAddressID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpAddressLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpAddressLoadAll]
GO
CREATE PROCEDURE [Nop_BannedIpAddressLoadAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM Nop_BannedIpAddress
	ORDER BY BannedIpAddressID
	
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpAddressLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpAddressLoadByPrimaryKey]
GO
CREATE PROCEDURE [Nop_BannedIpAddressLoadByPrimaryKey]
	@BannedIpAddressID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM Nop_BannedIpAddress
	WHERE BannedIpAddressID = @BannedIpAddressID
	ORDER BY BannedIpAddressID
	
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpAddressInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpAddressInsert]
GO
CREATE PROCEDURE [Nop_BannedIpAddressInsert]
	@BannedIpAddressID int = NULL output,
	@Address nvarchar(50),
	@Comment nvarchar(500),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	INSERT 
	INTO Nop_BannedIpAddress 
	(
		[Address],
		[Comment], 
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES 
	(
		@Address,
		@Comment,
		@CreatedOn,
		@UpdatedOn
	)
	
	SET @BannedIpAddressID = SCOPE_IDENTITY()

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpAddressUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpAddressUpdate]
GO
CREATE PROCEDURE [Nop_BannedIpAddressUpdate]
	@BannedIpAddressID int,
	@Address nvarchar(50),
	@Comment nvarchar(500),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	UPDATE Nop_BannedIpAddress 
	SET
		[Address] = @Address,
		[Comment] = @Comment,
		[CreatedOn] = @CreatedOn,
		[UpdatedOn] = @UpdatedOn
	WHERE 
		BannedIpAddressID = @BannedIpAddressID

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpAddressDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpAddressDelete]
GO
CREATE PROCEDURE [Nop_BannedIpAddressDelete]
	@BannedIpAddressID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_BannedIpAddress 
	WHERE BannedIpAddressID = @BannedIpAddressID
	
END
GO




if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_BannedIpNetwork]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_BannedIpNetwork](
	[BannedIpNetworkID] [int] IDENTITY(1,1) NOT NULL,
	[StartAddress] [nvarchar](50) NOT NULL,
	[EndAddress] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](500) NULL,
	[IpException] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	CONSTRAINT [PK_Nop_BannedNetworkIP] PRIMARY KEY CLUSTERED 
	(
		[BannedIpNetworkID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpNetworkLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpNetworkLoadAll]
GO
CREATE PROCEDURE [Nop_BannedIpNetworkLoadAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM Nop_BannedIpNetwork 
	ORDER BY BannedIpNetworkID

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpNetworkLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpNetworkLoadByPrimaryKey]
GO
CREATE PROCEDURE [Nop_BannedIpNetworkLoadByPrimaryKey]
	@BannedIpNetworkID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM Nop_BannedIpNetwork
	WHERE BannedIpNetworkID = @BannedIpNetworkID
	ORDER BY BannedIpNetworkID

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpNetworkInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpNetworkInsert]
GO
CREATE PROCEDURE [Nop_BannedIpNetworkInsert]
	@BannedIpNetworkID int = NULL output,
	@StartAddress nvarchar(50),
	@EndAddress nvarchar(50),
	@Comment nvarchar(500),
	@IpException nvarchar(max),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	INSERT 
	INTO Nop_BannedIpNetwork 
	(
		StartAddress,
		EndAddress,
		Comment,
		IpException,
		CreatedOn,
		UpdatedOn
	)
	VALUES 
	(
		@StartAddress,
		@EndAddress,
		@Comment,
		@IpException,
		@CreatedOn,
		@UpdatedOn
	)
	
	SET @BannedIpNetworkID = SCOPE_IDENTITY()
	
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpNetworkUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpNetworkUpdate]
GO
CREATE PROCEDURE [Nop_BannedIpNetworkUpdate]
	@BannedIpNetworkID int,
	@StartAddress nvarchar(50),
	@EndAddress nvarchar(50),
	@Comment nvarchar(500),
	@IpException nvarchar(max),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	UPDATE Nop_BannedIpNetwork
	 SET
		StartAddress = @StartAddress,
		EndAddress = @EndAddress,
		Comment = @Comment,
		IpException = @IpException,
		CreatedOn = @CreatedOn,
		UpdatedOn = @UpdatedOn
	WHERE 
		BannedIpNetworkID = @BannedIpNetworkID

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BannedIpNetworkDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BannedIpNetworkDelete]
GO
CREATE PROCEDURE [Nop_BannedIpNetworkDelete]
	@BannedIpNetworkID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_BannedIpNetwork
	WHERE BannedIpNetworkID = @BannedIpNetworkID

END
GO



-- Forum signature
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.SignatureEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.SignatureEnabled', N'true', N'Determines whether user signatures are enabled')
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='Signature')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD [Signature] nvarchar(300) NOT NULL CONSTRAINT [DF_Nop_Customer_Signature] DEFAULT ((''))
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
	@Signature nvarchar(300),
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
		[Signature],
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
		@Signature,
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
	@Signature nvarchar(300),
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
		[Signature]=@Signature,
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
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Account.Signature')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Account.Signature', N'Signature')
END
GO




-- Poll system keyword
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Poll]') and NAME='SystemKeyword')
BEGIN
	ALTER TABLE [dbo].[Nop_Poll] 
	ADD [SystemKeyword] nvarchar(400) NOT NULL CONSTRAINT [DF_Nop_Poll_SystemKeyword] DEFAULT ((''))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PollInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PollInsert]
GO
CREATE PROCEDURE [dbo].[Nop_PollInsert]
(
	@PollID int = NULL output,
	@LanguageID int,
	@Name nvarchar(400),
	@SystemKeyword nvarchar(400),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Poll]
	(
		LanguageID,
		[Name],
		SystemKeyword,
		Published,
		DisplayOrder		
	)
	VALUES
	(
		@LanguageID,
		@Name,
		@SystemKeyword,
		@Published,
		@DisplayOrder
	)

	set @PollID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PollUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PollUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_PollUpdate]
(
	@PollID int,
	@LanguageID int,
	@Name nvarchar(400),
	@SystemKeyword nvarchar(400),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_Poll]
	SET
	LanguageID=@LanguageID,
	[Name]=@Name,
	SystemKeyword=@SystemKeyword,
	Published=@Published,
	DisplayOrder=@DisplayOrder
	WHERE
		PollID = @PollID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PollLoadBySystemKeyword]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PollLoadBySystemKeyword]
GO
CREATE PROCEDURE [dbo].[Nop_PollLoadBySystemKeyword]
(
	@SystemKeyword nvarchar(400)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Poll]
	WHERE
		SystemKeyword = @SystemKeyword
END
GO

-- Blacklist cache enabled
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Cache.IpBlacklistManager.CacheEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Cache.IpBlacklistManager.CacheEnabled', N'true', N'Blacklist cache enabled')
END
GO


--additional shipping charge

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='AdditionalShippingCharge')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD AdditionalShippingCharge money NOT NULL CONSTRAINT [DF_Nop_ProductVariant_AdditionalShippingCharge] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantInsert]
(
    @ProductVariantID int = NULL output,
    @ProductId int,
    @Name nvarchar(400),
    @SKU nvarchar (400),
    @Description nvarchar(4000),
    @AdminComment nvarchar(4000),
    @ManufacturerPartNumber nvarchar(100),
    @IsDownload bit,
    @DownloadID int,
    @IsShipEnabled bit,
    @IsFreeShipping bit,
	@AdditionalShippingCharge money,
    @IsTaxExempt bit,
    @TaxCategoryID int,
	@ManageInventory bit,
    @StockQuantity int,
    @MinStockQuantity int,
    @LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
    @WarehouseId int,
    @DisableBuyButton int,
    @RequiresTextOption bit,
    @TextOptionPrompt nvarchar(400),
    @Price money,
    @OldPrice money,
    @Weight float,
    @Length decimal(18, 4),
    @Width decimal(18, 4),
    @Height decimal(18, 4),
    @PictureID int,
    @Published bit,
    @Deleted bit,
    @DisplayOrder int,
    @CreatedOn datetime,
    @UpdatedOn datetime
)
AS
BEGIN
    INSERT
    INTO [Nop_ProductVariant]
    (
        ProductId,
        [Name],
        SKU,
        [Description],
        AdminComment,
        ManufacturerPartNumber,
        IsDownload,
        DownloadID,
        IsShipEnabled,
        IsFreeShipping,
		AdditionalShippingCharge,
        IsTaxExempt,
        TaxCategoryID,
		ManageInventory,
        StockQuantity,
        MinStockQuantity,
        LowStockActivityID,
		NotifyAdminForQuantityBelow,
		OrderMinimumQuantity,
		OrderMaximumQuantity,
        WarehouseId,
        DisableBuyButton,
        RequiresTextOption,
        TextOptionPrompt,
        Price,
        OldPrice,
        Weight,
        [Length],
        Width,
        Height,
        PictureID,
        Published,
        Deleted,
        DisplayOrder,
        CreatedOn,
        UpdatedOn
    )
    VALUES
    (
        @ProductId,
        @Name,
        @SKU,
        @Description,
        @AdminComment,
        @ManufacturerPartNumber,
        @IsDownload,
        @DownloadID,
        @IsShipEnabled,
        @IsFreeShipping,
		@AdditionalShippingCharge,
        @IsTaxExempt,
        @TaxCategoryID,
		@ManageInventory,
        @StockQuantity,
        @MinStockQuantity,
        @LowStockActivityID,
		@NotifyAdminForQuantityBelow,
		@OrderMinimumQuantity,
		@OrderMaximumQuantity,
        @WarehouseId,
        @DisableBuyButton,
        @RequiresTextOption,
        @TextOptionPrompt,
        @Price,
        @OldPrice,
        @Weight,
        @Length,
        @Width,
        @Height,
        @PictureID,
        @Published,
        @Deleted,
        @DisplayOrder,
        @CreatedOn,
        @UpdatedOn
    )

    set @ProductVariantID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantUpdate]
(
	@ProductVariantID int,
	@ProductId int,
	@Name nvarchar(400),
	@SKU nvarchar (400),
	@Description nvarchar(4000),
	@AdminComment nvarchar(4000),
	@ManufacturerPartNumber nvarchar(100),
	@IsDownload bit,
	@DownloadID int,
	@IsShipEnabled bit,
	@IsFreeShipping bit,
	@AdditionalShippingCharge money,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@ManageInventory bit,
	@StockQuantity int,
	@MinStockQuantity int,
	@LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
	@WarehouseId int,
	@DisableBuyButton bit,
	@RequiresTextOption bit,
	@TextOptionPrompt nvarchar(400),
	@Price money,
	@OldPrice money,
	@Weight float,
	@Length decimal(18, 4),
	@Width decimal(18, 4),
	@Height decimal(18, 4),
	@PictureID int, 
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant]
	SET
		ProductId=@ProductId,
		[Name]=@Name,
		[SKU]=@SKU,
		[Description]=@Description,
		AdminComment=@AdminComment,
		ManufacturerPartNumber=@ManufacturerPartNumber,
		IsDownload=@IsDownload,
		DownloadID=@DownloadID,
		IsShipEnabled=@IsShipEnabled,
		IsFreeShipping=@IsFreeShipping,
		AdditionalShippingCharge=@AdditionalShippingCharge,
		IsTaxExempt=@IsTaxExempt,
		TaxCategoryID=@TaxCategoryID,
		ManageInventory=@ManageInventory,
		StockQuantity=@StockQuantity,
		MinStockQuantity=@MinStockQuantity,
		LowStockActivityID=@LowStockActivityID,
		NotifyAdminForQuantityBelow=@NotifyAdminForQuantityBelow,
		OrderMinimumQuantity=@OrderMinimumQuantity,
		OrderMaximumQuantity=@OrderMaximumQuantity,
		WarehouseId=@WarehouseId,
		DisableBuyButton=@DisableBuyButton,
		RequiresTextOption=@RequiresTextOption,
		TextOptionPrompt=@TextOptionPrompt,
		Price=@Price,
		OldPrice=@OldPrice,
		Weight=@Weight,
		[Length]=@Length,
		Width=@Width,
		Height=@Height,
		PictureID=@PictureID,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ProductVariantID = @ProductVariantID
END
GO


--payment method additional fee
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.PaymentMethodAdditionalFeeIsTaxable')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.PaymentMethodAdditionalFeeIsTaxable', N'false', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.PaymentMethodAdditionalFeeIncludesTax')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.PaymentMethodAdditionalFeeIncludesTax', N'false', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.PaymentMethodAdditionalFeeTaxClassID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.PaymentMethodAdditionalFeeTaxClassID', N'0', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'ShoppingCart.PaymentMethodAdditionalFee')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'ShoppingCart.PaymentMethodAdditionalFee', N'Payment method additional fee')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Order.PaymentMethodAdditionalFee')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Order.PaymentMethodAdditionalFee', N'Payment method additional fee')
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='PaymentMethodAdditionalFeeInclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD PaymentMethodAdditionalFeeInclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_PaymentMethodAdditionalFeeInclTax] DEFAULT ((0))
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='PaymentMethodAdditionalFeeExclTax')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD PaymentMethodAdditionalFeeExclTax MONEY NOT NULL CONSTRAINT [DF_Nop_Order_PaymentMethodAdditionalFeeExclTax] DEFAULT ((0))
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='PaymentMethodAdditionalFeeInclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD PaymentMethodAdditionalFeeInclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_PaymentMethodAdditionalFeeInclTaxInCustomerCurrency] DEFAULT ((0))
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='PaymentMethodAdditionalFeeExclTaxInCustomerCurrency')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD PaymentMethodAdditionalFeeExclTaxInCustomerCurrency MONEY NOT NULL CONSTRAINT [DF_Nop_Order_PaymentMethodAdditionalFeeExclTaxInCustomerCurrency] DEFAULT ((0))
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
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
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
		PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax,
		OrderTax,
		OrderTotal,
		OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
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
		@PaymentMethodAdditionalFeeInclTax,
		@PaymentMethodAdditionalFeeExclTax,
		@OrderTax,
		@OrderTotal,
		@OrderDiscount,		
		@OrderSubtotalInclTaxInCustomerCurrency,
		@OrderSubtotalExclTaxInCustomerCurrency,		
		@OrderShippingInclTaxInCustomerCurrency,
		@OrderShippingExclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
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
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
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
		PaymentMethodAdditionalFeeInclTax=@PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax=@PaymentMethodAdditionalFeeExclTax,
		OrderTax=@OrderTax,
		OrderTotal=@OrderTotal,
		OrderDiscount=@OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency=@OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency=@OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency=@OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency=@OrderShippingExclTaxInCustomerCurrency,	
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,	
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


-- google checkout logging
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'PaymentMethod.GoogleCheckout.DebugModeEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'PaymentMethod.GoogleCheckout.DebugModeEnabled', N'false', N'')
END
GO


--product available dates
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='AvailableStartDateTime')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD AvailableStartDateTime DATETIME NULL
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='AvailableEndDateTime')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD AvailableEndDateTime DATETIME NULL
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantInsert]
(
    @ProductVariantID int = NULL output,
    @ProductId int,
    @Name nvarchar(400),
    @SKU nvarchar (400),
    @Description nvarchar(4000),
    @AdminComment nvarchar(4000),
    @ManufacturerPartNumber nvarchar(100),
    @IsDownload bit,
    @DownloadID int,
    @IsShipEnabled bit,
    @IsFreeShipping bit,
	@AdditionalShippingCharge money,
    @IsTaxExempt bit,
    @TaxCategoryID int,
	@ManageInventory bit,
    @StockQuantity int,
    @MinStockQuantity int,
    @LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
    @WarehouseId int,
    @DisableBuyButton int,
    @RequiresTextOption bit,
    @TextOptionPrompt nvarchar(400),
    @Price money,
    @OldPrice money,
    @Weight float,
    @Length decimal(18, 4),
    @Width decimal(18, 4),
    @Height decimal(18, 4),
    @PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
    @Published bit,
    @Deleted bit,
    @DisplayOrder int,
	@CreatedOn datetime,
    @UpdatedOn datetime
)
AS
BEGIN
    INSERT
    INTO [Nop_ProductVariant]
    (
        ProductId,
        [Name],
        SKU,
        [Description],
        AdminComment,
        ManufacturerPartNumber,
        IsDownload,
        DownloadID,
        IsShipEnabled,
        IsFreeShipping,
		AdditionalShippingCharge,
        IsTaxExempt,
        TaxCategoryID,
		ManageInventory,
        StockQuantity,
        MinStockQuantity,
        LowStockActivityID,
		NotifyAdminForQuantityBelow,
		OrderMinimumQuantity,
		OrderMaximumQuantity,
        WarehouseId,
        DisableBuyButton,
        RequiresTextOption,
        TextOptionPrompt,
        Price,
        OldPrice,
        Weight,
        [Length],
        Width,
        Height,
        PictureID,
		AvailableStartDateTime,
		AvailableEndDateTime,
        Published,
        Deleted,
        DisplayOrder,
        CreatedOn,
        UpdatedOn
    )
    VALUES
    (
        @ProductId,
        @Name,
        @SKU,
        @Description,
        @AdminComment,
        @ManufacturerPartNumber,
        @IsDownload,
        @DownloadID,
        @IsShipEnabled,
        @IsFreeShipping,
		@AdditionalShippingCharge,
        @IsTaxExempt,
        @TaxCategoryID,
		@ManageInventory,
        @StockQuantity,
        @MinStockQuantity,
        @LowStockActivityID,
		@NotifyAdminForQuantityBelow,
		@OrderMinimumQuantity,
		@OrderMaximumQuantity,
        @WarehouseId,
        @DisableBuyButton,
        @RequiresTextOption,
        @TextOptionPrompt,
        @Price,
        @OldPrice,
        @Weight,
        @Length,
        @Width,
        @Height,
        @PictureID,
		@AvailableStartDateTime,
		@AvailableEndDateTime,
        @Published,
        @Deleted,
        @DisplayOrder,
        @CreatedOn,
        @UpdatedOn
    )

    set @ProductVariantID=@@identity
END
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantUpdate]
(
	@ProductVariantID int,
	@ProductId int,
	@Name nvarchar(400),
	@SKU nvarchar (400),
	@Description nvarchar(4000),
	@AdminComment nvarchar(4000),
	@ManufacturerPartNumber nvarchar(100),
	@IsDownload bit,
	@DownloadID int,
	@IsShipEnabled bit,
	@IsFreeShipping bit,
	@AdditionalShippingCharge money,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@ManageInventory bit,
	@StockQuantity int,
	@MinStockQuantity int,
	@LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
	@WarehouseId int,
	@DisableBuyButton bit,
	@RequiresTextOption bit,
	@TextOptionPrompt nvarchar(400),
	@Price money,
	@OldPrice money,
	@Weight float,
	@Length decimal(18, 4),
	@Width decimal(18, 4),
	@Height decimal(18, 4),
	@PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant]
	SET
		ProductId=@ProductId,
		[Name]=@Name,
		[SKU]=@SKU,
		[Description]=@Description,
		AdminComment=@AdminComment,
		ManufacturerPartNumber=@ManufacturerPartNumber,
		IsDownload=@IsDownload,
		DownloadID=@DownloadID,
		IsShipEnabled=@IsShipEnabled,
		IsFreeShipping=@IsFreeShipping,
		AdditionalShippingCharge=@AdditionalShippingCharge,
		IsTaxExempt=@IsTaxExempt,
		TaxCategoryID=@TaxCategoryID,
		ManageInventory=@ManageInventory,
		StockQuantity=@StockQuantity,
		MinStockQuantity=@MinStockQuantity,
		LowStockActivityID=@LowStockActivityID,
		NotifyAdminForQuantityBelow=@NotifyAdminForQuantityBelow,
		OrderMinimumQuantity=@OrderMinimumQuantity,
		OrderMaximumQuantity=@OrderMaximumQuantity,
		WarehouseId=@WarehouseId,
		DisableBuyButton=@DisableBuyButton,
		RequiresTextOption=@RequiresTextOption,
		TextOptionPrompt=@TextOptionPrompt,
		Price=@Price,
		OldPrice=@OldPrice,
		Weight=@Weight,
		[Length]=@Length,
		Width=@Width,
		Height=@Height,
		PictureID=@PictureID,
		AvailableStartDateTime=@AvailableStartDateTime,
		AvailableEndDateTime=@AvailableEndDateTime,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ProductVariantID = @ProductVariantID
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
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
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
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
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

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
(
	@ProductID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductVariant] pv
	WHERE 
			(@ShowHidden = 1 OR pv.Published = 1) 
		AND 
			pv.Deleted=0
		AND 
			pv.ProductID = @ProductID
		AND 
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
	order by DisplayOrder
END
GO

--Make configurable whether to show page debug info (page execution time)
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.PageExecutionTimeInfoEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.PageExecutionTimeInfoEnabled', N'false', N'')
END
GO

-- admin area. Show full error
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.AdminArea.ShowFullErrors')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.AdminArea.ShowFullErrors', N'false', N'')
END
GO

-- web analytics
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Analytics.GoogleEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Analytics.GoogleEnabled', N'false', N'Google analytics enabled')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Analytics.GoogleJS')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Analytics.GoogleJS', N'<script type="text/javascript"> var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www."); document.write(unescape("%3Cscript src=''" + gaJsHost + "google-analytics.com/ga.js'' type=''text/javascript''%3E%3C/script%3E")); </script> <script type="text/javascript"> try { var pageTracker = _gat._getTracker("UA-0000000-0"); pageTracker._trackPageview(); } catch(err) {}</script>', N'Formatted google JS code')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Analytics.GoogleID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Analytics.GoogleID', N'UA-0000000-0', N'Your Google Analytics ID')
END
GO

--database maintenance
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Maintenance_ReindexTables]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Maintenance_ReindexTables]
GO
CREATE PROCEDURE [dbo].[Nop_Maintenance_ReindexTables]
AS
BEGIN
	--indexing
	DECLARE @TableName sysname
	DECLARE cur_reindex CURSOR FOR
	SELECT table_name
	FROM information_schema.tables
	WHERE table_type = 'base table'
	OPEN cur_reindex
	FETCH NEXT FROM cur_reindex INTO @TableName
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT 'Reindexing ' + @TableName + ' table'
		DBCC DBREINDEX (@TableName, ' ', 80)
		FETCH NEXT FROM cur_reindex INTO @TableName
		END
	CLOSE cur_reindex
	DEALLOCATE cur_reindex
END
GO


--blog/news comments sort order

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BlogCommentLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BlogCommentLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_BlogCommentLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_BlogComment]
	ORDER BY CreatedOn
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_NewsCommentLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_NewsCommentLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_NewsCommentLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_NewsComment]
	order by CreatedOn
END
GO

--search page paging localization

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Search.First')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Search.First', N'First')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Search.Last')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Search.Last', N'Last')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Search.Next')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Search.Next', N'Next')
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Search.Previous')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Search.Previous', N'Previous')
END
GO

--move forum topic

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'PageTitle.Boards.MoveTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'PageTitle.Boards.MoveTopic', N'Move Topic')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.MoveTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.MoveTopic', N'Move Topic')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_LocaleStringResource]
		WHERE [LanguageID]=7 and [ResourceName] = N'Forum.SelectTheForumToMoveTopic')
BEGIN
	INSERT [dbo].[Nop_LocaleStringResource] ([LanguageID], [ResourceName], [ResourceValue])
	VALUES (7, N'Forum.SelectTheForumToMoveTopic', N'Select the forum you want to move the post to')
END
GO