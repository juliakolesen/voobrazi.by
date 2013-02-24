USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_ProductLoadAllPaged]    Script Date: 02/24/2013 21:52:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
      @CategoryID             int = 0,
      @ManufacturerID         int = 0,
      @FeaturedProducts bit = null, --0 featured only , 1 not featured only, null - load all products
      @PriceMin               money = null,
      @PriceMax               money = null,
      @Keywords               nvarchar(MAX),    
      @SearchDescriptions bit = 0,
      @ShowHidden             bit = 0,
      @PageIndex              int = 0, 
      @PageSize               int = 2147483644,
      @FilteredSpecs          nvarchar(300) = null,   --filter by attributes (comma-separated list). e.g. 14,15,16
      @SortBy             int = 0, -- 0 - default by name; 1 - popularity; 2- price; 3 - novelty 
      @SortTo             bit = 0, --0-descending
      @HeightMin              decimal = 0,
      @WidthMin               decimal = 0, 
      @HeightMax              decimal = 2147483644,
      @WidthMax               decimal = 2147483644, 
      @TotalRecords           int = null OUTPUT
)
AS
BEGIN
      
      --init
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
      ELSE IF @ShowHidden = 1
            BEGIN
                  INSERT #DisplayOrder 
                  SELECT p.ProductID, 1 
                  FROM [Nop_Product] p
                  ORDER BY p.[Name]
            END
      ELSE
		BEGIN
			INSERT #DisplayOrder 
			SELECT DISTINCT p.ProductID, 1 
			FROM [Nop_Product] p
			JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
		END
      
      --rating
      CREATE TABLE #RatingTable
      (
                  ProductVariantID int not null PRIMARY KEY,
                  PriceExclTax int not null,
                  Total int not null
      )

      DECLARE @StartTime datetime;
      DECLARE @EndTime datetime;
      SET @EndTime = GETDATE();
      SET @StartTime = DATEADD(MONTH, -6, @EndTime);

      INSERT #RatingTable EXECUTE Nop_OrderProductVariantReport @OrderStatusID=null, @PaymentStatusID=null
      
      --filter by attributes
      SET @FilteredSpecs = isnull(@FilteredSpecs, '')
      CREATE TABLE #FilteredSpecs
      (
            SpecificationAttributeOptionID int not null
      )
      INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
      SELECT CAST(data as int) FROM dbo.[NOP_splitstring_to_table](@FilteredSpecs, ',');
      
      DECLARE @SpecAttributesCount int   
      SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

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
            ProductName nvarchar(400),
            ProductPrice money NOT NULL,
            ProductRatingSum int,
            ProductCreatedOn date NOT NULL
      )
      
      INSERT INTO #PageIndex (ProductID, DisplayOrder, ProductName, ProductPrice, ProductRatingSum, ProductCreatedOn)
      SELECT p.ProductID, do.DisplayOrder, p.Name, pv.Price, rt.Total, p.CreatedOn
      FROM Nop_Product p with (NOLOCK) 
      LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
      LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
      LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
      JOIN #DisplayOrder do on p.ProductID = do.ProductID
      LEFT OUTER JOIN #RatingTable rt on pv.ProductVariantId = rt.ProductVariantID
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
            AND   (
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
            AND
                  (
                        --filter by specs
                        @SpecAttributesCount = 0
                        OR
                        (
                             NOT EXISTS(
                                   SELECT 1 
                                   FROM #FilteredSpecs [fs]
                                   WHERE [fs].SpecificationAttributeOptionID NOT IN (
                                         SELECT psam.SpecificationAttributeOptionID
                                         FROM dbo.Nop_Product_SpecificationAttribute_Mapping psam
                                         WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
                                         )
                                   )
                             
                        )
                  )
             AND (
                   pv.Width between @WidthMin AND @WidthMax
                   )
             AND (
                   pv.Height between @HeightMin AND @HeightMax
                  )
            )
    GROUP BY p.ProductID, do.DisplayOrder, p.Name, pv.Price, rt.Total, p.CreatedOn, pv.ProductVariantID     
      ORDER BY 
            case when @SortBy = 0  and @SortTo = 0 then p.Name end desc
            ,case when @SortBy = 0  and @SortTo = 1 then p.Name end asc
            ,case when @SortBy = 1 and @SortTo = 0 then rt.Total end desc
            ,case when @SortBy = 1 and @SortTo = 1 then rt.Total end asc
            ,case when @SortBy = 2 and @SortTo = 0 then pv.Price end desc
            ,case when @SortBy = 2 and @SortTo = 1 then pv.Price end asc
            ,case when @SortBy = 3 and @SortTo = 0 then p.CreatedOn end desc 
            ,case when @SortBy = 3 and @SortTo = 1 then p.CreatedOn end asc
      /*ORDER BY do.DisplayOrder*/

      --total records
      SET @TotalRecords = @@rowcount     
      SET ROWCOUNT @RowsToReturn
      
      --return
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

      DROP TABLE #DisplayOrder
      DROP TABLE #FilteredSpecs
      DROP TABLE #PageIndex
      DROP TABLE #RatingTable
END



GO

USE [voobrazi]
GO

/****** Object:  Table [dbo].[ViewedItem]    Script Date: 02/24/2013 21:55:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ViewedItem](
	[ViewedProductID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerSessionGUID] [uniqueidentifier] NOT NULL,
	[ProductVariantID] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ViewedItem] PRIMARY KEY CLUSTERED 
(
	[ViewedProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemUpdate]    Script Date: 02/24/2013 22:00:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ViewedItemUpdate]
( 
    @ViewedItemID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE ViewedItem
	SET
			CustomerSessionGUID=@CustomerSessionGUID,
			ProductVariantID=@ProductVariantID,	
			CreatedOn=@CreatedOn
	WHERE
		ViewedProductID = @ViewedItemID
END
GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemLoadByProductVariantID]    Script Date: 02/24/2013 22:00:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ViewedItemLoadByProductVariantID]
(
	@ProductVariantID int,
	@CustomerSessionGUID uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [ViewedItem]
	WHERE
		ProductVariantID = @ProductVariantID AND
		CustomerSessionGUID = @CustomerSessionGUID
END
GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemLoadByPrimaryKey]    Script Date: 02/24/2013 22:00:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ViewedItemLoadByPrimaryKey]
(
	@ViewedItemID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [ViewedItem]
	WHERE
		ViewedProductID = @ViewedItemID
END
GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemLoadByCustomerSessionGUID]    Script Date: 02/24/2013 22:00:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ViewedItemLoadByCustomerSessionGUID]
(
	@CustomerSessionGUID uniqueidentifier,
	@Count int = 2147483644,
	@PageIndex int = 0,
	@TotalRecords int= null OUTPUT
)
AS
BEGIN

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int
DECLARE @RowsToReturn int
	
SET @RowsToReturn = @Count * (@PageIndex + 1)	
SET @PageLowerBound = @Count * @PageIndex
SET @PageUpperBound = @PageLowerBound + @Count + 1

CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ViewedProductID int NOT NULL,
		CustomerSessionGUID uniqueidentifier NOT NULL,
		ProductVariantID int,
		CreatedOn datetime NOT NULL
	)

	INSERT INTO #PageIndex 
	SELECT *
	FROM [ViewedItem]
	WHERE CustomerSessionGUID=@CustomerSessionGUID 
	order by CreatedOn desc
	
	SET @TotalRecords = @@ROWCOUNT	
	SET ROWCOUNT @RowsToReturn	
	
	SELECT  
		*
	FROM
		#PageIndex [pi]
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY  
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
END

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemInsert]    Script Date: 02/24/2013 22:00:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ViewedItemInsert]
(
    @ViewedItemID int = NULL output,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [ViewedItem]
	(
		CustomerSessionGUID,
		ProductVariantID,
		CreatedOn
	)
	VALUES
	(
		@CustomerSessionGUID,
		@ProductVariantID,
		@CreatedOn
	)
	
	set @ViewedItemID=@@identity
END

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ViewedItemDelete]    Script Date: 02/24/2013 21:59:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[ViewedItemDelete]
(
	@ViewedProductID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [ViewedItem]
	WHERE
		[ViewedProductID] = @ViewedProductID
END
GO


USE [voobrazi]
GO

/****** Object:  Table [dbo].[IndividualOrder]    Script Date: 02/24/2013 22:03:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IndividualOrder](
	[IndividualOrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerSessionGUID] [uniqueidentifier] NOT NULL,
	[Price] [bigint] NOT NULL,
	[SerialNumberInShCart] [int] NOT NULL,
	[OrderText] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_IndividualOrder] PRIMARY KEY CLUSTERED 
(
	[IndividualOrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[IndividualOrderLoadByPrimaryKey]    Script Date: 02/24/2013 22:04:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[IndividualOrderLoadByPrimaryKey]
(
	@IndividualOrderID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [IndividualOrder]
	WHERE
		IndividualOrderID = @IndividualOrderID
END

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[IndividualOrderInsert]    Script Date: 02/24/2013 22:03:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[IndividualOrderInsert]
(
    @IndividualOrderID int = NULL output,
    @CustomerSessionGUID uniqueidentifier,
	@Price bigint,
	@OrderText nvarchar(MAX)
)
AS
BEGIN
	DECLARE @SerialNumberInShCart int
	
	SELECT * 
	FROM [IndividualOrder]
	WHERE CustomerSessionGUID = @CustomerSessionGUID
	
	SET @SerialNumberInShCart = @@ROWCOUNT
	SET @SerialNumberInShCart = @SerialNumberInShCart + 1
	
	INSERT
	INTO [IndividualOrder]
	(
		CustomerSessionGUID,
		Price,
		SerialNumberInShCart,
		OrderText
	)
	VALUES
	(
		@CustomerSessionGUID,
		@Price,
		@SerialNumberInShCart,
		@OrderText
	)
	
	set @IndividualOrderID = @@identity
END


GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[IndividualOrderGetByCustomerSessionGUID]    Script Date: 02/24/2013 22:03:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[IndividualOrderGetByCustomerSessionGUID]
(
	@CustomerSessionGUID uniqueidentifier
)

AS
BEGIN

	SELECT *
	FROM [IndividualOrder]
	WHERE CustomerSessionGUID = @CustomerSessionGUID 
	
END


GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[IndividualOrderDelete]    Script Date: 02/24/2013 22:03:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[IndividualOrderDelete]
(
	@IndividualOrderID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [IndividualOrder]
	WHERE
		[IndividualOrderID] = @IndividualOrderID
END

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_RelatedProductLoadByProductID1Paged]    Script Date: 02/24/2013 22:08:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Nop_RelatedProductLoadByProductID1Paged]
(
	@ProductID1 int,
	@ShowHidden bit,
	@PageIndex int = 1,
	@PageSize int = 2147483644,
	@TotalRecords int = null OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON
	
	CREATE TABLE #PageIndex 
      (
            IndexID int IDENTITY (1, 1) NOT NULL,
            RelatedProductID int NOT NULL,
			ProductID1 int NOT NULL,
			ProductID2 int NOT NULL,
			DisplayOrder int NOT NULL
      )
      
      
    INSERT INTO #PageIndex (RelatedProductID, ProductID1, ProductID2, DisplayOrder)
    SELECT rp.RelatedProductID, rp.ProductID1, rp.ProductID2, rp.DisplayOrder
    FROM Nop_RelatedProduct rp
	INNER JOIN Nop_Product p ON rp.ProductID2=p.ProductID
	WHERE rp.ProductID1=@ProductID1
		AND (p.Published = 1 or @ShowHidden = 1) and p.Deleted=0
	ORDER BY rp.DisplayOrder
	SET @TotalRecords = @@rowcount
	
	DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @RowsToReturn int
      
    SET @RowsToReturn = @PageSize * (@PageIndex + 1)     
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
            [pi].*
      FROM
            #PageIndex [pi]
      WHERE
            [pi].IndexID > @PageLowerBound AND 
            [pi].IndexID < @PageUpperBound
      ORDER BY  
            IndexID
      
      SET ROWCOUNT 0
      DROP TABLE #PageIndex
END

GO


USE [voobrazi]
GO

/****** Object:  Table [dbo].[Colors]    Script Date: 02/24/2013 22:10:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Colors](
	[ColorName] [nvarchar](max) NOT NULL,
	[ColorArgb] [bigint] NOT NULL,
	[ColorID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Colors_1] PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ColorUpdateName]    Script Date: 02/24/2013 22:11:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[ColorUpdateName]
(
	@OldColorName nvarchar(MAX),
	@NewColorName nvarchar(MAX)
)

AS
BEGIN

	UPDATE Colors
	SET ColorName=@NewColorName
	WHERE ColorName=@OldColorName
	
END




GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ColorsInsert]    Script Date: 02/24/2013 22:11:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ColorsInsert]
(
	@ColorName nvarchar(MAX),
	@ColorArgb bigint
)
AS
BEGIN
	SET NOCOUNT ON
	INSERT
	INTO [Colors]
	VALUES
		(@ColorName, @ColorArgb)
END



GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ColorsDelete]    Script Date: 02/24/2013 22:11:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[ColorsDelete]
(
	@ColorName nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Colors]
	WHERE
		[ColorName] = @ColorName
END



GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[ColorGetByColorName]    Script Date: 02/24/2013 22:11:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[ColorGetByColorName]
(
	@ColorName nvarchar(MAX)
)

AS
BEGIN

	SELECT *
	FROM [Colors]
	WHERE ColorName = @ColorName 
	
END




GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_ProductLoadSpecificationAttributesOptionsByCategory]    Script Date: 02/24/2013 22:14:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Nop_ProductLoadSpecificationAttributesOptionsByCategory]
(
      @CategoryID             int = 0,
      @SpecificationAttributeID int
)
AS
BEGIN

	CREATE TABLE #DisplayOrder
      (
            ProductID int not null PRIMARY KEY,
      )     

    IF @CategoryID IS NOT NULL AND @CategoryID > 0
        BEGIN
              INSERT #DisplayOrder 
              SELECT pcm.ProductID 
              FROM [Nop_Product_Category_Mapping] pcm WHERE pcm.CategoryID = @CategoryID
        END
    
    CREATE TABLE #FilteredSpecs
      (
            SpecificationAttributeOptionID int not null
      )
      
    INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)  
    SELECT sao.SpecificationAttributeOptionID FROM  dbo.Nop_SpecificationAttributeOption sao
    WHERE  sao.SpecificationAttributeID = @SpecificationAttributeID
        
    SELECT psam.SpecificationAttributeOptionID
    FROM #DisplayOrder p
    LEFT OUTER JOIN dbo.Nop_Product_SpecificationAttribute_Mapping psam 
    ON psam.ProductID=p.ProductID
    WHERE psam.SpecificationAttributeOptionID IN 
    (SELECT fs.SpecificationAttributeOptionID FROM #FilteredSpecs fs)
    GROUP BY psam.SpecificationAttributeOptionID
   
   DROP TABLE #FilteredSpecs
   DROP TABLE #DisplayOrder	
   
END



GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_ProductGetMaxPriceByCategory]    Script Date: 02/24/2013 22:17:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Nop_ProductGetMaxPriceByCategory]
(
      @CategoryID             int = 0
 )
 
 AS
BEGIN
    SET NOCOUNT ON
	SELECT Price = MAX(pv.Price)
	FROM [Nop_Product] p
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	WHERE (p.Published = 1) and p.Deleted=0 and pcm.CategoryID = @CategoryID
END

GO


USE [voobrazi]
GO

/****** Object:  Trigger [dbo].[short_description_update]    Script Date: 02/24/2013 22:21:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[short_description_update]
   ON  [dbo].[Nop_Product] 
   AFTER UPDATE, INSERT
AS 
BEGIN
declare @width decimal(18,4) = 0;
declare @height decimal(18,4) = 0;
declare @sDescr nvarchar(MAX);
select @sDescr = ShortDescription from inserted
declare @id int;
select @id = ProductId from inserted

	if (update(ShortDescription))
	BEGIN
	IF(CHARINDEX(N'(см)', @sDescr) > 0)
	BEGIN
			set @height = RTRIM(LTRIM(
			  CASE
				WHEN CHARINDEX('<br />', @sDescr) > 0 AND CHARINDEX('X', @sDescr) > 0 THEN
						SUBSTRING(@sDescr,
						CHARINDEX('<br />', @sDescr) + LEN('<br />'), 
						CHARINDEX('X', @sDescr) - (CHARINDEX('<br />', @sDescr) + LEN('<br />')))
	                    
				WHEN CHARINDEX(N'Высота', @sDescr) > 0 THEN
						REPLACE(SUBSTRING(@sDescr,
						CHARINDEX(N'Высота', @sDescr) + LEN(N'Высота'), 
						CHARINDEX(N'(см)', @sDescr) - (CHARINDEX(N'Высота', @sDescr) + LEN(N'Высота')))
						,'-','')
	             WHEN CHARINDEX('.', @sDescr) > 0 AND CHARINDEX('X', @sDescr) > 0 THEN
						SUBSTRING(@sDescr,
								CHARINDEX('.', @sDescr) + LEN('.'), 
								CHARINDEX('X', @sDescr) - (CHARINDEX('.', @sDescr) + LEN('.')))
				 WHEN CHARINDEX('X', @sDescr) > 0 THEN
					  SUBSTRING(@sDescr,1, CHARINDEX('X', @sDescr) - 1)
				 ELSE SUBSTRING(@sDescr,1, CHARINDEX(N'(см)', @sDescr) - 1)
			  END ))
	    
		   set @width = RTRIM(LTRIM(
				 CASE
					 WHEN CHARINDEX('X', @sDescr) > 0 THEN
						   SUBSTRING(@sDescr,CHARINDEX('X', @sDescr) + 1, CHARINDEX(N'(см)', @sDescr) - CHARINDEX('X', @sDescr) - 1)
			ELSE '0'
			END ))
			
			if(ISNUMERIC(@height) = 1 AND ISNUMERIC(@width) = 1)
			BEGIN
				UPDATE dbo.Nop_ProductVariant
				SET Height = @height, Width = @width
				WHERE ProductID = @id
			END
		END
    END
END


GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_ProductGetMaxPriceByCategory]    Script Date: 02/24/2013 22:22:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Nop_ProductGetMaxPriceByCategory]
(
      @CategoryID             int = 0
 )
 
 AS
BEGIN
    SET NOCOUNT ON
	SELECT Price = MAX(pv.Price)
	FROM [Nop_Product] p
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	WHERE (p.Published = 1) and p.Deleted=0 and pcm.CategoryID = @CategoryID
END

GO


USE [voobrazi]
GO

/****** Object:  StoredProcedure [dbo].[Nop_ProductGetMaxHeightByCategory]    Script Date: 02/24/2013 22:22:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Nop_ProductGetMaxHeightByCategory]
(
      @CategoryID             int = 0
 )
 
 AS
BEGIN
    SET NOCOUNT ON
	SELECT height = MAX(pv.Height)
	FROM [Nop_Product] p
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	WHERE (p.Published = 1) and p.Deleted=0 and pcm.CategoryID = @CategoryID
END

GO


Alter TABLE [dbo].[Nop_Product]
alter column ShortDescription nvarchar(MAX)

