create or alter procedure sp_SupplierPurchaseReport
(
@From DateTime,
@To DateTime,
@SupplierId int,
@PurchaseId int
)
as
begin
    select SupplierId,ProductID,[Count],Cost,[Date] from Stock_tb
						 where [Date] between @From and @To and SupplierId=@SupplierId
end
