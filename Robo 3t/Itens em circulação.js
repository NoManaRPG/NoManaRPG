db.getCollection("WafclastBaseItem").aggregate([
  { $group: 
      { _id: "$Name",
             "Quantidade":{$sum:"$Quantity"}
      }
  },
  {
    $sort : { "Quantidade": -1 }
  }
]);