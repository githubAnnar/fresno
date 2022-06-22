// Imports
var express = require("express");
var cors = require("cors");
var Helpers = require('./src/helpers/helpers');

const PersonEndpoints = require('./src/endpoints/person_endpoints');
const StepTestEndpoints = require('./src/endpoints/steptest_endpoints');
const MeasurementEndpoints = require('./src/endpoints/measurement_endpoints');
const PolyRegEndpoints = require('./src/endpoints/polyreg_endpoints');

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
var db = require('./src/controllers/database');
const AuthEndpoints = require('./src/endpoints/auth_endpoints');

app.use(express.urlencoded({ extended: false }));
app.use(express.json());
app.use(cors());

const personEndpoints = new PersonEndpoints("api", app, db);
const stepTestEndpoints = new StepTestEndpoints("api", app, db);
const measurementEndpoints = new MeasurementEndpoints("api", app, db);
const authEndpoints = new AuthEndpoints("api", app, db);
const polyRegEndpoints = new PolyRegEndpoints('api', app);

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

personEndpoints.endpoints();
stepTestEndpoints.endpoints();
measurementEndpoints.endpoints();
authEndpoints.endpoints();
polyRegEndpoints.endpoints();