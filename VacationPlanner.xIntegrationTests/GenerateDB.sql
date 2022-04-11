IF DB_ID('VacationPlanner') IS NULL
BEGIN
create database [VacationPlanner]
END

IF NOT EXISTS (SELECT * FROM Employee)
begin
create table Employee
(
    Id     int identity (1,1) not null
        primary key,
    Name   nvarchar(200)      not null,
    TeamId int                not null,
    Role   int
)
end

IF NOT EXISTS (SELECT * FROM Team)
begin
create table Team
(
    Id   int identity (1,1) not null
        primary key,
    Name nvarchar(100)      not null
)
end

IF NOT EXISTS (SELECT * FROM Vacation)
begin
create table Vacation
(
    Id         int identity (1,1) not null
        primary key,
    Start      date               not null,
    [End]      date               not null,
    State      int                not null,
    EmployeeId int                not null
)
end