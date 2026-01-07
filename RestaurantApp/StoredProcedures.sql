CREATE PROCEDURE InsertCustomer  
 @FullName NVARCHAR(100),  
 @Phone NVARCHAR(11),  
 @Addresss NVARCHAR(200)  
AS   
BEGIN  
If EXISTS(SELECT 1 FROM Customers WHERE Phone=@Phone)  
 BEGIN  
 RAISERROR(' IT IS REPEATED ',16,1)  
 RETURN  
 END  
 INSERT INTO Customers(FullName,Phone,Addresss,IsActive)  
 VALUES(@FullName,@Phone,@Addresss,1)  
 END

 --_______________________________________

   CREATE PROCEDURE dbo.UpdateCustomer  
  @CustomersId Int,  
  @FullName NVARCHAR(100),  
  @Phone NVARCHAR(11),  
  @Addresss NVARCHAR(200)  
  AS  
  BEGIN  
  SET NOCOUNT ON;  
  IF EXISTS  
  (  
  SELECT 1  
  FROM dbo.Customers  
  WHERE Phone=@Phone  
  AND CustomersId <> @CustomersId   
  )  
  BEGIN  
  RAISERROR('THIS PHONE NUMBER IS SAVED BEFORE',16,1);  
  RETURN;  
  END  
  UPDATE dbo.Customers  
  SET  
  FullName=@FullName,  
  Phone=@Phone,  
  Addresss=@Addresss  
  WHERE CustomersId=@CustomersId;  
  END  

  --_______________________________________

    
 CREATE PROCEDURE dbo.SearchCustomer  
  @Search NVARCHAR(100)  
  AS  
  BEGIN  
  SELECT  
  CustomersId,FullName,Phone,Addresss  
  FROM dbo.Customers  
  WHERE   
  FullName LIKE '%'+ @Search + '%'  
  OR Phone LIKE '%'+ @Search + '%'  
  END  

  --___________________________________________

  CREATE PROCEDURE dbo.DeleteCustomer  
  @CustomersId INT  
  as  
  begin  
  set NOCOUNT on;  
  update dbo.Customers  
  set IsActive=0  
  where CustomersId=@CustomersId;  
  end  

  ------------------------------------
    
create procedure dbo.InsertOrder  
@CustomersId int,  
@TotalAmount decimal(10,2)  
as  
begin  
set NOCOUNT on;  
declare @FinalAmount decimal(10,2);  
set @FinalAmount =dbo.CalculateFinalAmount(@TotalAmount);  
insert into dbo.Orders (CustomersId,TotalAmount,FinalAmount)  
values (@CustomersId,@TotalAmount,@FinalAmount);end  


-------------------------------------------------------------------

create procedure dbo.SalesReport  
as  
begin  
select c.FullName,  
count (o.OrderId) as OrderCount,  
sum (o.TotalAmount) as TotalAmount,  
sum (o.finalAmount) AS FinalAmount  
from dbo.Orders o  
inner join dbo.Customers c On o.CustomersId =c.CustomersId   
where c.IsActive=1  
group by c.FullName;  
end   