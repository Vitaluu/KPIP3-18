create table libraries
(
	lib_id int identity primary key,
	lib_name nvarchar(100)
)

create table abonement(
	a_id int identity primary key,
	date_start date,
	date_end date,
	lib_id int foreign key references libraries(lib_id)
)

create table readers(
	r_id int identity primary key,
	a_id int foreign key references abonement(a_id),
	surname nvarchar(50),
	name nvarchar(50),
	middleName nvarchar(50),
	tel integer
)

create table teachers(
	r_id int primary key foreign key references readers(r_id),
	Образование nvarchar(100),
	Место_работы Nvarchar(100)
)

create table students(
	r_id int primary key foreign key references readers(r_id),
	Название_учебного_заведения Nvarchar(100),
	faculty nvarchar(50),
	course smallint,
	group_number nvarchar(10)
)
create table scientist(
	r_id int primary key foreign key references readers(r_id),
	org_name nvarchar(50),
	scientific_theme nvarchar(50)
)

create table storages(
	stor_id int identity primary key,
	lib_id int foreign key references libraries(lib_id)
)
create table book_types(
	[type_id] int identity primary key,
	[type_name] nvarchar(50) unique not null
)

create table authors(
	author_id int identity primary key,
	author_name nvarchar(100) not null,
	birthday date
)

create table books(
	book_id int identity primary key,
	stand int not null,
	shelf int not null,
	book_name nvarchar(50) not null,
	readOnly char(1) check(readOnly=1 or readOnly=0),
	stor_id int foreign key references storages(stor_id),
	[type_id] int foreign key references book_types([type_id])
)

create table m2m_books_authors(
	book_id int foreign key references books(book_id),
	author_id int foreign key references authors(author_id),
	primary key(book_id,author_id)
)


create table reading_room(
	room_id int identity primary key,
	l_num int,
	lib_id int foreign key references libraries(lib_id),
	unique(l_num,lib_id)
)

create table librarians(
	l_id int identity primary key,
	surname nvarchar(50) not null,
	name nvarchar(50) not null,
	middleName nvarchar(50) not null,
	r_id int foreign key references reading_room(room_id)
)

create table issue(
	l_id int foreign key references librarians(l_id),
	book_id int foreign key references books(book_id),
	issue_date date not null
)

create table debite(
	l_id int foreign key references librarians(l_id),
	book_id int foreign key references books(book_id),
	debite_date date not null
)

create table extraditions(
	ex_id int identity primary key,
	l_id  int foreign key references librarians(l_id),
	r_id int foreign key references readers(r_id),
	quantity int not null default 0,
	is_active char(1) check(is_active=1 or is_active = 0),
	book_id int foreign key references books(book_id)
)