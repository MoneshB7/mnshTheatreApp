USE [MovieDB]
GO
/****** Object:  Table [dbo].[BookingDetails]    Script Date: 23-05-2021 17:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MovieID] [varchar](5) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[EmailID] [nvarchar](50) NOT NULL,
	[NoOfSeats] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 23-05-2021 17:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[MovieId] [varchar](5) NOT NULL,
	[MovieName] [varchar](50) NOT NULL,
	[Language] [varchar](50) NOT NULL,
	[AverageRating] [numeric](2, 1) NOT NULL,
	[Description] [varchar](100) NULL,
	[PosterURL] [nvarchar](max) NULL,
	[Location] [nvarchar](50) NULL,
 CONSTRAINT [pk_Movie_MovieId] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BookingDetails] ON 

INSERT [dbo].[BookingDetails] ([ID], [MovieID], [Name], [EmailID], [NoOfSeats]) VALUES (1, N'M1001', N'Monesh B', N'janedoe@gmail.com', 2)

SET IDENTITY_INSERT [dbo].[BookingDetails] OFF
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1001', N'Wonder Woman', N'English', CAST(3.0 AS Numeric(2, 1)), N'Action', N'https://aatfweb.files.wordpress.com/2017/11/91i2jspdfll-_ri_.jpg', N'Chennai')
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1002', N'The Revanant', N'Hindi', CAST(4.0 AS Numeric(2, 1)), N'Adventure', N'https://m.media-amazon.com/images/M/MV5BMDE5OWMzM2QtOTU2ZS00NzAyLWI2MDEtOTRlYjIxZGM0OWRjXkEyXkFqcGdeQXVyODE5NzE3OTE@._V1_.jpg', N'Chennai')
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1003', N'The Social Network', N'English', CAST(5.0 AS Numeric(2, 1)), N'Drama', N'https://static.metacritic.com/images/products/movies/9/aa5515688c769b5ffb92d2b07e671c2a.jpg', N'Chennai')
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1004', N'Batman Begins', N'English', CAST(4.0 AS Numeric(2, 1)), N'Action', N'https://www.teahub.io/photos/full/257-2574839_batman-begins-batman-begins-movie-poster-original.jpg', N'Coimbatore')
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1005', N'Split', N'English', CAST(4.0 AS Numeric(2, 1)), N'Thriller', N'https://m.media-amazon.com/images/M/MV5BZTJiNGM2NjItNDRiYy00ZjY0LTgwNTItZDBmZGRlODQ4YThkL2ltYWdlXkEyXkFqcGdeQXVyMjY5ODI4NDk@._V1_.jpg', N'Coimbatore')
INSERT [dbo].[Movie] ([MovieId], [MovieName], [Language], [AverageRating], [Description], [PosterURL], [Location]) VALUES (N'M1006', N'Parasite', N'Hindi', CAST(5.0 AS Numeric(2, 1)), N'Thriller', N'https://images-na.ssl-images-amazon.com/images/I/A1UTpJzoPBL._RI_.jpg', N'Coimbatore')
ALTER TABLE [dbo].[Movie] ADD  DEFAULT ((0)) FOR [AverageRating]
GO
ALTER TABLE [dbo].[Movie]  WITH CHECK ADD  CONSTRAINT [chk_Movie_AverageRating] CHECK  (([AverageRating]>=(0) AND [AverageRating]<=(5)))
GO
ALTER TABLE [dbo].[Movie] CHECK CONSTRAINT [chk_Movie_AverageRating]
GO
ALTER TABLE [dbo].[Movie]  WITH CHECK ADD  CONSTRAINT [chk_Movie_Language] CHECK  (([Language]='Japanese' OR [Language]='Hindi' OR [Language]='English'))
GO
ALTER TABLE [dbo].[Movie] CHECK CONSTRAINT [chk_Movie_Language]
GO
ALTER TABLE [dbo].[Movie]  WITH CHECK ADD  CONSTRAINT [chk_Movie_MovieId] CHECK  (([MovieId] like 'M____'))
GO
ALTER TABLE [dbo].[Movie] CHECK CONSTRAINT [chk_Movie_MovieId]
GO
/****** Object:  StoredProcedure [dbo].[BookTickets]    Script Date: 23-05-2021 17:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BookTickets]
@MovieId varchar(5),
@Name nvarchar(50),
@EmailID nvarchar(50),
@NoOfSeats int

AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO DBO.BookingDetails(MovieID,Name,EmailID,NoOfSeats)
		VALUES(@MovieId,@Name,@EmailID,@NoOfSeats)
END
GO
/****** Object:  StoredProcedure [dbo].[GetMovieByID]    Script Date: 23-05-2021 17:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMovieByID]
@MovieId varchar(5)
AS
BEGIN
	SET NOCOUNT ON;
		SELECT * FROM DBO.Movie where MovieId=@MovieId
END
GO
/****** Object:  StoredProcedure [dbo].[GetMovies]    Script Date: 23-05-2021 17:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMovies]
AS
BEGIN
	SET NOCOUNT ON;
		SELECT * FROM DBO.Movie
END
GO
