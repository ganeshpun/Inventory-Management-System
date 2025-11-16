create or alter procedure sp_update_customer
(
@CustomerID int,
@CustomerName varchar(50),
@Address varchar(50),
@Email varchar(50),
@Phone varchar(50)
)
as
begin
    update Customer_tb  set
	                   CustomerName  = @CustomerName,
	                     Address = @Address,
						 Email = @Email,
						 Phone = @Phone
						 where CustomerID = @CustomerID 
end