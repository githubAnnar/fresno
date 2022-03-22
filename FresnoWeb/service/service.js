// Imports
var express = require("express");
var cors = require("cors");
var Helpers = require('./src/helpers/helpers');

const UserEndpoints = require("./src/endpoints/user_endpoints");

console.log(`${Helpers.getDateNowString()} Count of args: ${process.argv.length}`);

if (process.argv.length === 3) {
    if (process.argv[2] === 'dev') {

    }

    else if (process.argv[2] === 'prod') {

    }
}

else {
    console.log("Wrong Args!");
    exit;
}

// Create express app
var app = express();
var db = require("./src/controllers/database.js");

app.use(express.urlencoded({ extended: false }));
app.use(express.json());
app.use(cors());

const userEndpoints = new UserEndpoints("api", app, db);

// Server port
var HTTP_PORT = 8000
// Start server
app.listen(HTTP_PORT, () => {
    console.log(`${Helpers.getDateNowString()} Server running on port ${HTTP_PORT}`);
});

// Root endpoint
app.get("/", (req, res, next) => {
    res.json({ "message": "Ok" });
    console.log(`${Helpers.getDateNowString()} request on root`);
});