create or alter procedure sp_Insert_customer
(
@CustomerName varchar(50),
@Address varchar(50),
@Email varchar(50),
@Phone varchar(50)
)
as
begin
     insert into customer_tb (CustomerName,Address,Email,Phone) values (@CustomerName,@Address,@Email,@Phone)
end