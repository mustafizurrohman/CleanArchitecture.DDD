#Basic Syntax

select [<column> [as <label>],]
[from stream
  [where <predicate>]
  [group by [time(<d>)|<grouping>,]]
  [having <predicate>]
  [order by [time|<label>] [asc|desc]]
  [limit <n>]
  [for refresh]]

##Demo1
select Method, RequestPath, StatusCode
from stream
where StatusCode > 399

##Demo2
select count(*)
from stream
group by RequestPath

Details here: https://github.com/datalust/seq-cheat-sheets