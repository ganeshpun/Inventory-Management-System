create or alter procedure sp_update_supplier
(
@SupplierID int,
@SupplierName varchar(50),
@Address varchar(50),
@Email varchar(50),
@Contact varchar(50)
)
as
begin
    update supplier_tb set
	                     SupplierName = @SupplierName,
	                     Address = @Address,
						 Email = @Email,
						 Contact = @Contact
						 where SupplierID = @SupplierID
end