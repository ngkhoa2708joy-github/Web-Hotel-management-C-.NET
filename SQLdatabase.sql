CREATE DATABASE HotelDB;
create table RoomTypes(
    RoomTypeId int not null primary key,
    RoomTypeName varchar(50)
)

create table Rooms(
    RoomId int IDENTITY(1,1) not null primary key,
    RoomNumber varchar(5)  NOT NULL,
    RoomImage nvarchar(550)  NOT NULL,
    RoomPrice decimal(18,2)  NOT NULL,
    BookingStatusId int  NOT NULL,
    RoomTypeId int  NOT NULL,
    RoomCapacity int  NOT NULL,
    RoomDescription nvarchar(550)  NULL,
    IsActive bit  NULL
)

CREATE TABLE BookingStatus (
    BookingStatusId int IDENTITY(1,1) NOT NULL,
    BookingStatus varchar(50)  NOT NULL
)

create table RoomBookings(
    BookingId int IDENTITY(1,1) not null primary key,
    CustomerName varchar(550)  NOT NULL,
    CustomerAddress varchar(550)  NOT NULL,
    CustomerPhone varchar(15)  NOT NULL,
    BookingFrom datetime  NOT NULL,
    BookingTo datetime  NOT NULL,
    AssignRoomId int  NOT NULL,
    NoOfMembers int  NOT NULL,
    TotalAmount decimal(18,2)
)


create table PaymentTypes(
    PaymentTypeId int not null primary key,
    PaymentType1 varchar(50)
)

create table CustomerServices(
    ServiceId int primary key IDENTITY(1,1),
    ServiceName nvarchar(255),
    ServicePrice decimal(18, 2)
)

create table Payments(
    PaymentId int not null primary key,
    BookingId int,
    PaymentTypeId int,
    PaymentAmount decimal(18, 2),
    IsActive bit
)

create table ServiceServed(
    ServiceID int ,
    BookingId int,
    Amount decimal(18,2)
)

create table Invoid(
	InvoidID int not null primary key,
	BookingID int,
	BookingAmount decimal(18,2),
	ServiceAmount decimal(18,2),
	ToTalPayment decimal(18,2),
	IsActive bit Default 1
)

CREATE TABLE Users (
    idUser int IDENTITY(1,1) NOT NULL,
    FirstName nvarchar(50)  NULL,
    LastName nvarchar(50)  NULL,
    Email nvarchar(50)  NULL,
    [Password] nvarchar(50)  NULL
)

--- Insert Booking status
INSERT INTO BookingStatus(BookingStatus)
VALUES ('serverd');

INSERT INTO BookingStatus(BookingStatus)
VALUES ('occupied');

INSERT INTO BookingStatus(BookingStatus)
VALUES ('free');

--- trigger to create invoice when booking room 
go
create trigger create_invoice
on RoomBookings
after INSERT
as begin
	declare   @TotalAmount decimal(18,2), @id int, @invID int
	SELECT  @TotalAmount = inserted.TotalAmount, @id = inserted.BookingId  from inserted
	Select @invID = Max(Invoid.InvoidID) from Invoid
	if (@invID is null)
		begin
			set @invID = 1
		end
	else
		begin
			set @invID = @invID +1 
		end

	Insert into Invoid(InvoidID,BookingID, BookingAmount, ServiceAmount, IsActive)
	values(@invID,@id, @TotalAmount, 0, 1)
end
go


--- trigger to update invoice when update booking room 

go
create trigger up_create_invoice
on RoomBookings
after update
as begin
	declare  @total decimal(18,2), @TotalAmount decimal(18,2), @id int
	SELECT @TotalAmount = inserted.TotalAmount, @id = inserted.BookingId  from inserted

	update Invoid	
	set BookingAmount = @TotalAmount
	from Invoid
	Where Invoid.BookingId = @id
end
go

---Proc to list all unpaid invoice
go
create proc sp_Invoice_ListAll_1
as
select * from Invoid where IsActive = 1
order by InvoidID asc
go
---Proc to list all paid invoice
go
create proc sp_Invoice_ListAll_0
as
select * from Invoid where IsActive = 1
order by InvoidID asc
go
---Proc to list all paid invoice
go
create proc sp_Invoice_ListAll
as
select * from Invoid
order by InvoidID asc
go
---Proc to list all service
go
create proc sp_service_ListAll
as
select * from CustomerServices
order by [ServicePrice] asc
go
--- trigger to check same SerSer table
go
create trigger bf_inSer
on ServiceServed	
instead of insert
as begin
	declare @insBid int, @insSid int, @insAmo decimal(18,2)
	Select @insBid = inserted.BookingId, @insSid = inserted.ServiceID, @insAmo = inserted.Amount from inserted
	if (@insSid in (Select ServiceID from ServiceServed where ServiceServed.BookingId = @insBid))
		begin
			update ServiceServed
			set Amount = (select Amount from ServiceServed where ServiceServed.BookingId = @insBid and ServiceServed.ServiceID = @insSid) + @insAmo
			where ServiceServed.BookingId = @insBid and ServiceServed.ServiceID = @insSid
		end
	else
		begin
			insert into ServiceServed(BookingId, ServiceID, Amount) values (@insBid,@insSid,@insAmo)
		end
end

go
--- trigger to update invoice when booking service
go
create trigger up_Invoice
on ServiceServed
after insert, update
as begin
	declare @SerBid int, @SerSid int, @SerAmo decimal(18,2), @total decimal(18,2)
	select @SerBid = inserted.BookingId, @SerSid = inserted.ServiceID, @SerAmo = inserted.Amount from inserted
	set @total = (select sum(ServiceServed.Amount*CustomerServices.ServicePrice) as toltal  from inserted, ServiceServed, CustomerServices where inserted.BookingId = ServiceServed.BookingId and CustomerServices.ServiceId = ServiceServed.ServiceID )
	update Invoid
	set ServiceAmount = @total
	where Invoid.BookingID = @SerBid and Invoid.IsActive = 1
end
go
--- trigger to calulator total payment in invoice after booking service
go
create trigger cal_Invoice
on Invoid
After insert, update
as begin
	declare @invBid int, @invBamount int, @invSAmo int, @total decimal(18,2)
	Select @invBid = inserted.BookingId, @invBamount = inserted.BookingAmount, @invSAmo = inserted.ServiceAmount from inserted
	set @total = @invBamount + @invSAmo
	update Invoid
	set ToTalPayment = @total
	where Invoid.BookingID = @invBid
end
go
--- proc to change status invoid when invoice was paid
go
create proc sp_changeActive_InvoidAndRooom
	@InvID int
as
begin
	declare @BookingId int, @roomid int
	update Invoid
	set Invoid.IsActive = 0
	where Invoid.InvoidID = @InvID

	select @BookingId = Invoid.BookingID from Invoid where Invoid.InvoidID = @InvID
	select @roomid = RoomBookings.AssignRoomId from RoomBookings where RoomBookings.BookingId = @BookingId
	
	update Rooms
	set Rooms.IsActive = 0
	where Rooms.RoomId = @roomid

end
go

--- proc get list roomBooing to booking service
go
create proc Sp_get_list_roomBooking_id
as 
select * from RoomBookings, Invoid where RoomBookings.BookingId = Invoid.BookingID and Invoid.IsActive = 1
order by RoomBookings.BookingId asc;
go
