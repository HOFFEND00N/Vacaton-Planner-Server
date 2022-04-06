create table Employee
(
    Id     int           not null
        primary key,
    Name   nvarchar(200) not null,
    TeamId int           not null,
    Role   int
)
go

create table Team
(
    Id   int           not null
        primary key,
    Name nvarchar(100) not null
)
go

create table Vacation
(
    Id         int  not null
        primary key,
    Start      date not null,
    [End]      date not null,
    State      int  not null,
    EmployeeId int  not null
)
go
