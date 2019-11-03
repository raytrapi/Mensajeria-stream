create table emoticonos(
   id int not null,
   regex varchar(100) not null,
   url varchar(500) not null,
   ancho int not null,
   alto int not null,
   primary key (id)
);