/* create schema */
create table if not exists Sales
(
  Id int primary key,
  ProductId int, 
  CustomerId int, 
  DateCreated datetime
);

/* fill with data */
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (1, 2, 3, date('2016-11-10'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (2, 2, 3, date('2016-11-11'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (3, 3, 4, date('2016-11-12'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (4, 2, 4, date('2016-11-13'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (5, 4, 3, date('2016-11-10'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (6, 4, 3, date('2016-11-10'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (7, 4, 5, date('2016-11-14'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (8, 2, 5, date('2016-11-15'));
insert into Sales (Id, ProductId, CustomerId, DateCreated) values (9, 5, 5, date('2016-11-15'));

/* execute fetch data */
select distinct s5.ProductId, case when s4.cnt is null then 0 else s4.cnt end cnt from Sales s5
left join (
  select s2.ProductId, count(distinct s2.DateCreated) cnt from Sales s2
  join (
    select s1.CustomerId, min(s1.DateCreated) DateCreated
    from Sales s1
    group by s1.CustomerId
  ) s3 on s3.CustomerId = s2.CustomerId and s3.DateCreated = s2.DateCreated
  group by s2.ProductId
) s4 on s4.ProductId = s5.ProductId