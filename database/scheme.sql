create table Kategorie_Przepisy(
P_ID_KATEGORII int constraint PK_KATEGORIE_PRZEPISY primary key identity(1,1),
P_NAZWA varchar(25));

create table Przepisy(
ID_PRZEPISU int constraint PK_PRZEPISY primary key identity(1,1),
NAZWA varchar (50),
ID_KATEGORII int constraint FK_LINK_KATEGORIE_PRZEPISY references Kategorie_Przepisy on delete cascade,
OPIS varchar (5000),
NOTATKI varchar (5000),
ILOSC_PORCJI float,
CZAS_PRZYRZADZENIA int,
ZDJECIE image);

create table Kategorie_Skladniki(
S_ID_KATEGORII int constraint PK_KATEGORIE_SKLADNIKI primary key identity(1,1),
S_NAZWA varchar(25));

create table Skladniki(
ID_SKLADNIKA int constraint PK_SKLADNIKI primary key identity(1,1),
NAZWA varchar (50),
ID_KATEGORII int constraint FK_LINK_KATEGORIE_SKLADNIKI references Kategorie_Skladniki on delete cascade,
OPIS varchar (5000),
UWAGI varchar (5000),
ZDJECIE image);

create table Jednostki(
ID_JEDNOSTKI int constraint PK_JEDNOSTKI primary key identity(1,1),
NAZWA varchar(15));

create table Przepisy_Skladniki(
L_ID_PRZEPISU int constraint FK_LINK_PRZEPISY references Przepisy on delete cascade,
L_ID_SKLADNIKA int constraint FK_LINK_SKLADNIKI references Skladniki on delete cascade,
ILOSC float,
ID_JEDNOSTKI int constraint FK_LINK_Jednostki_Przepisy_Skladniki references Jednostki on delete cascade,);