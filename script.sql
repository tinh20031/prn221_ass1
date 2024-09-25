use master;
go
if exists (select name from sys.databases where name = 'SalesManagementDb')
begin
	drop database SalesManagementDb;
end;
go
create database SalesManagementDb;
go
use SalesManagementDb;
go

create table Member (
   MemberId int identity(1,1) not null,
   Email varchar(100) not null,
   CompanyName varchar(40) not null,
   City varchar(15) not null,
   Country varchar(15) not null,
   [Password] varchar(30) not null,

   primary key (MemberId)
);

create table [Order] (
  OrderId int identity(1,1) not null,
  MemberId int not null,
  OrderDate datetime not null default getdate(),
  RequiredDate datetime null,
  ShippedDate datetime null,
  Freight money null,
  [Status] varchar(50) not null,
  
  primary key (OrderId),
  foreign key (MemberId) references dbo.Member(MemberId)
);

create table Category(
  CategoryId int identity(1,1) not null,
  CategoryName varchar(100) not null,

  primary key (CategoryId)
);

create table Product (
  ProductId int identity(1,1) not null,
  CategoryId int not null,
  ProductName varchar(40) not null,
  [Weight] varchar(20) not null,
  UnitPrice money not null,
  UnitsInStock int not null,

  primary key (ProductId),
  foreign key (CategoryId) references dbo.Category(CategoryId)
);


create table OrderDetail (
  OrderId int not null,
  ProductId int not null,
  UnitPrice money not null,
  Quantity int not null,
  Discount float not null,

  primary key (OrderId, ProductId),
  foreign key (OrderId) references dbo.[Order](OrderId),
  foreign key (ProductId) references dbo.Product(ProductId)
);


insert into dbo.Member(Email,CompanyName,City,Country,[Password])
values ('minh@gmail.com', 'My Company', 'TPHCM', 'VN', '12345');

insert into Category (CategoryName)
VALUES
  ('Electronics'),
  ('Home Appliances'),
  ('Clothing'),
  ('Furniture'),
  ('Outdoor Gear');

-- Insert sample data for Product table
insert into Product (CategoryId, ProductName, [Weight], UnitPrice, UnitsInStock)
values
  (1, 'Laptop', '5 lbs', 899.99, 50),
  (1, 'Smartphone', '0.5 lbs', 499.99, 100),
  (2, 'Refrigerator', '200 lbs', 1199.99, 25),
  (2, 'Dishwasher', '80 lbs', 599.99, 30),
  (3, 'T-Shirt', '0.5 lbs', 19.99, 200),
  (3, 'Jeans', '1 lb', 49.99, 150),
  (4, 'Sofa', '150 lbs', 799.99, 20),
  (4, 'Dining Table', '100 lbs', 499.99, 15),
  (5, 'Hiking Backpack', '3 lbs', 99.99, 75),
  (5, 'Camping Tent', '10 lbs', 149.99, 40);
