exports = async function(payload,response) {
  var event = payload.body.text();
  var conn = context.services.get("mongodb-atlas").db("babyreport").collection("events");
  var eobj = JSON.parse(event);
  eobj.mdb_created = new Date();
  await conn.insertOne(eobj);
};