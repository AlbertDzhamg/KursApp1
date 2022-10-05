
CREATE TABLE bank
(
  _id SERIAL PRIMARY KEY,
  _namefull VARCHAR(255)not null,
  _nameshort VARCHAR(255)not null,
  _inn VARCHAR(255)not null,
  _bik VARCHAR(255)not null,
  _corAccount VARCHAR(255)not null,
  _account VARCHAR(255)not null,
  _city VARCHAR(255)not null
);

CREATE TABLE aggrement
( 
  _id SERIAL PRIMARY KEY,
  _number integer,
  _dataOpen date default null,
  _dataClose date default null
);

CREATE TABLE typeaccount
(
  _id SERIAL PRIMARY KEY,
  _type  VARCHAR(255)not null
);

CREATE TABLE account
(
   _id SERIAL PRIMARY KEY,
   _bankid integer not null,
   _agreementid integer,
   _typeid integer,
  FOREIGN KEY (_bankid) REFERENCES bank (_id) ON DELETE CASCADE,
  FOREIGN KEY (_agreementid) REFERENCES aggrement (_id) ON DELETE CASCADE,
  FOREIGN KEY (_typeid) REFERENCES typeaccount (_id) ON DELETE CASCADE
);