create or alter procedure sp_Insert_supplier
(
@SupplierName varchar(50),
@Address varchar(50),
@Email varchar(50),
@Contact varchar(50)
)
as
begin
     insert into supplier_tb (SupplierName,Address,Email,Contact) values (@SupplierName,@Address,@Email,@Contact)
end