var sqlite3 = require('sqlite3').verbose();
var Helpers = require('./../helpers/helpers.js');

console.log(`${Helpers.getDateNowString()} DB ARGS: ${process.argv[2]}`);

var DBSOURCE = "";
if (process.argv[2] === "dev") {
    DBSOURCE = "../database/Fresno.sqlite";
}

else if (process.argv[2] === "prodw") {
    // DBSOURCE = "C:/Users/annar/Google Drive/Sykkel/BCK Drakt/OrderDB.sqlite";
    DBSOURCE = "";
}

else if (process.argv[2] === "prodl") {
    // DBSOURCE = "/home/annar/Downloads/OrderDB.sqlite";
    DBSOURCE = "";
}

else {
    throw new Error("wrong db");
}

if (DBSOURCE == "") {
    throw new Error("No Db");
}

let db = new sqlite3.Database(DBSOURCE, (err) => {
    if (err) {
        // Cannot open database
        console.error(err.message)
        throw err
    } else {
        console.log(`${Helpers.getDateNowString()} Connected to the SQLite database ${DBSOURCE}.`);
    }
});


module.exports = db;