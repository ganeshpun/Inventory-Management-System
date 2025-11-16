create or alter   procedure sp_Insert_SupplierPurchaseRepo
(
@From DateTime,
@To DateTime,
@SupplierId int
)
as
begin
     insert into PurchaseReport_Sup_tb ([From],[To],SupplierId) values (@From,@To,@SupplierId)
end
