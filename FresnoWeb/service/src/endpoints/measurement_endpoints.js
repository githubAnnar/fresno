const Helpers = require('../helpers/helpers');

// Database repositories
var MeasurementRepository = require('../repositories/measurement_repository');

class MeasurementEndpoints {
    constructor(rootPath, app, db) {
        this.rootPath = rootPath;
        this.app = app;
        this.db = db;
        this.repository = new MeasurementRepository(this.db);
        console.log(`${Helpers.getDateNowString()} HELLO from MeasurementEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for measurement`);
        const MODULE = "measurement";
        const ALL_MEASUREMENTS = "getallmeasurements";
        const ALL_MEASUREMENTS_BY_STEPTEST = "getallmeasurementsbysteptestid";
        const ONE_MEASUREMENT = "getmeasurementbyid";
        const POST_MEASUREMENT = "postnewmeasurement";
        const UPDATE_MEASUREMENT = "updatemeasurement";
        const DELETE_MEASUREMENT = "deletemeasurementbyid";

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_MEASUREMENTS}`, (req, res, next) => {
            this.repository.getAllMeasurements(res);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_MEASUREMENTS}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_MEASUREMENTS_BY_STEPTEST}/:id`, (req, res, next) => {
            this.repository.getAllMeasurementsByStepTestId(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_MEASUREMENTS_BY_STEPTEST}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_MEASUREMENT}/:id`, (req, res, next) => {
            this.repository.getMeasurementById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_MEASUREMENT}. req:${JSON.stringify(req.params)}`);
        });

        this.app.post(`/${this.rootPath}/${MODULE}/${POST_MEASUREMENT}/`, (req, res, next) => {
            this.repository.postNewMeasurement(res, req.body.Sequence, req.body.StepTestId, req.body.HeartRate, req.body.Lactate, req.body.Load, req.body.InCalculation);
            console.log(`${Helpers.getDateNowString()} request: POST ${POST_MEASUREMENT}. req: ${JSON.stringify(req.body)}`);
        });

        this.app.delete(`/${this.rootPath}/${MODULE}/${DELETE_MEASUREMENT}/:id`, (req, res, next) => {
            this.repository.deleteMeasurementById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: DELETE ${DELETE_MEASUREMENT}. req: ${JSON.stringify(req.params)}`);
        });

        this.app.patch(`/${this.rootPath}/${MODULE}/${UPDATE_MEASUREMENT}/`, (req, res, next) => {
            this.repository.updateMeasurementById(res, req.body.Id, req.body.Sequence, req.body.StepTestId, req.body.HeartRate, req.body.Lactate, req.body.Load, req.body.InCalculation);
            console.log(`${Helpers.getDateNowString()} request: UPDATE ${UPDATE_MEASUREMENT}. req: ${JSON.stringify(req.body)}`);
        });
    }
}

module.exports = MeasurementEndpoints;