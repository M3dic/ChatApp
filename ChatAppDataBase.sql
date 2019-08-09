create database if not exists ChatApp;
create table Participants(
SecretNumber varchar(200) not null primary key,
UserName varchar(50) not null,
Password varchar(100) not null,
Email varchar(70) default null
);
