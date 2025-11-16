create or alter procedure sp_update_register
(
@UserId int,
@Username varchar(50),
@Password varchar(50),
@Email varchar(50),
@Status int
)
as
begin
    update register_tb set
	                     Username= @Username,
						 Password = @Password,
						 Email = @Email,
						 Status= @Status
						 where UserId= @UserId
end
