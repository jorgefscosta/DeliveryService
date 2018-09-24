CREATE (A:Warehouse {name:'A'})
CREATE (B:Warehouse {name:'B'})
CREATE (C:Warehouse {name:'C'})
CREATE (D:Warehouse {name:'D'})
CREATE (E:Warehouse {name:'E'})
CREATE (F:Warehouse {name:'F'})
CREATE (G:Warehouse {name:'G'})
CREATE (H:Warehouse {name:'H'})
CREATE (I:Warehouse {name:'I'})
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
  
  (I)-[:SHIPS_TO {time:65, cost:5}]->(B)
