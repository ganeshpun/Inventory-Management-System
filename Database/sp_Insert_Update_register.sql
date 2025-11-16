create or alter procedure sp_Insert_Update_register
(
@UserId int,
@Username varchar(50),
@Password varchar(50),
@Email varchar(50),
@Status int
)
as
     if(@UserId=0)

begin
   	 insert into register_tb (Username,Password,Email,Status ) values (@Username,@Password,@Email,@Status )
end

else
begin
        update register_tb set
	                     Username= @Username,
						 Password = @Password,
						 Email = @Email,
						 Status= @Status
						 where UserId= @UserId
end