create or alter procedure sp_Insert_register
(
@Username varchar(50),
@Password varchar(50),
@Email varchar(50),
@Status int
)
as
begin
     insert into register_tb (Username,Password,Email,Status ) values (@Username,@Password,@Email,@Status )
end