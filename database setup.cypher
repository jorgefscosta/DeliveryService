CREATE (A:Warehouse {Id:1,name:'A'})
CREATE (B:Warehouse {Id:2,name:'B'})
CREATE (C:Warehouse {Id:3,name:'C'})
CREATE (D:Warehouse {Id:4,name:'D'})
CREATE (E:Warehouse {Id:5,name:'E'})
CREATE (F:Warehouse {Id:6,name:'F'})
CREATE (G:Warehouse {Id:7,name:'G'})
CREATE (H:Warehouse {Id:8,name:'H'})
CREATE (I:Warehouse {Id:9,name:'I'})

CREATE
  (A)-[:SHIPS_TO {time:1, cost:20}]->(C),
  (A)-[:SHIPS_TO {time:30, cost:5}]->(E),
  (A)-[:SHIPS_TO {time:10, cost:1}]->(H),
  
  (C)-[:SHIPS_TO {time:1, cost:12}]->(B),
  
  (D)-[:SHIPS_TO {time:4, cost:50}]->(F),
  
  (E)-[:SHIPS_TO {time:3, cost:5}]->(D),
  
  (F)-[:SHIPS_TO {time:40, cost:50}]->(G),
  (F)-[:SHIPS_TO {time:45, cost:50}]->(I),
  
  (G)-[:SHIPS_TO {time:64, cost:73}]->(B),
  
  (H)-[:SHIPS_TO {time:30, cost:1}]->(E),
  
  (I)-[:SHIPS_TO {time:65, cost:5}]->(B);

//adding the unique constraint will implicitly add an index on that property, so we won’t have to do that separately
CREATE CONSTRAINT ON (w:Warehouse) ASSERT w.Id IS UNIQUE;
CREATE CONSTRAINT ON (w:Warehouse) ASSERT w.Name IS UNIQUE; 
