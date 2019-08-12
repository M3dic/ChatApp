create database if not exists ChatApp;
create table Participants(
SecretNumber varchar(200) not null,
UserName varchar(50) not null primary key,
Password varchar(100) not null,
Email varchar(70) default null
);
create table Friends(
UserName varchar(50) not null,
FriendsUsername varchar(50) not null,
Accepted varchar(2) default 'N',
 FOREIGN KEY (UserName)
        REFERENCES Participants (UserName)
        ON DELETE CASCADE
);


