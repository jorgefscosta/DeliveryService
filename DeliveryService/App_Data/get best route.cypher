MATCH path = ((startWarehouse)-[:SHIPS_TO*2..20]->(endWarehouse:Warehouse))
WHERE startWarehouse.name='A' and endWarehouse.name='B'
WITH path, [x IN nodes(path) WHERE (x:Warehouse) | x] AS routesList, 
[y IN relationships(path) | y] AS shipmentDetail,
length(path) as hops,
reduce(accumCost = 0, r IN relationships(path)| accumCost + r.cost) AS totalCost,
reduce(accumTime = 0, r IN relationships(path)| accumTime + r.time) AS totalTime
return routesList, shipmentDetail, totalCost, totalTime, hops
order by totalTime,totalCost, hops
limit 1