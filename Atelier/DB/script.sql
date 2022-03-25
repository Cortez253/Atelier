
create table Users
(
	Id_user int identity primary key not null, 
	[Login] varchar(30) not null, 
	[Password] varchar(30) not null,
	[Access_rights] varchar(10) not null
)

create table Clients 
(
	Id_client int identity primary key not null,
	[Surname] nvarchar(50) not null,	 /* Фамилия */
	[Name] nvarchar(50) not null,		 /* Имя */
	[Patronymic] nvarchar(50) not null,  /* Отчество */
	[Telephone] varchar(12) not null,	 /* Номер телефона */
)

create table Products						/* Изделия */
(
	Id_product int identity primary key not null,
	Name_product nvarchar(50) not null,
	Example_product varchar(100)			/* Картинка изделия */
)

create table Materials
(
	Id_material int identity primary key not null, 
	Name_material nvarchar(50) not null
)

create table Workers
(
	Id_worker int identity primary key not null, 
	[Surname] nvarchar(50) not null,	 /* Фамилия */
	[Name] nvarchar(50) not null,		 /* Имя */
	[Patronymic] nvarchar(50) not null   /* Отчество */
)


create table Orders
(
	Id_order int identity primary key not null, 
	Id_client int not null constraint FK_client foreign key references Clients(Id_client) ,
	Id_product int not null constraint FK_product foreign key references Products(Id_product),
	Id_material int not null constraint FK_material foreign key references Materials(Id_material), 
	Id_worker int not null constraint FK_worker foreign key references Workers(Id_worker),
	Id_size int not null constraint FK_size foreign key references Sizes(Id_size), 
	Date_order date not null,						/* Дата заказа */
	Price money not null,							/* Цена */
	Order_status nvarchar(30) not null				/* Статус заказа */
)