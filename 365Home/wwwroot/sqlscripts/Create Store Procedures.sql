CREATE PROCEDURE usp_GetAllCategory
AS
SELECT * FROM dbo.Category
GO

CREATE PROCEDURE usp_GetAllLocation
AS
SELECT L.Name, L.Address
, P.Name AS [Province], D.Name AS [District],
W.Name AS [Ward], U.UserName as [User], L.Area, L.Description, 
I.Picture AS [Picture], L.LocationType
FROM dbo.Location AS L
LEFT JOIN Province AS P
ON L.ProvinceId = P.Id
LEFT JOIN District AS D
ON L.DistrictId = D.Id
LEFT JOIN Ward AS W
ON L.WardId = W.Id
LEFT JOIN AspNetUsers AS U
ON L.UserId = U.Id
LEFT JOIN WebImages AS I
ON L.ImageId = I.Id
GO
